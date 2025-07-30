using MSSQL.Attributes;
using MSSQL.Cache;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace MSSQL.QueryBuilder
{
    /*** SqlQueryBuilder class
    * 
    * This class is used to build SQL queries
    * Author: Phan Xuân Chánh { Chinese Charater: 潘春正, EnglishName1: Chanh Xuan Phan, EnglishName2: StevePhan }
    *  - www.phanxuanchanh.com
    *  
    */
    public partial class SqlQueryBuilder<T> : SqlQueryBuilderBase where T : ISqlTable, new()
    {
        private readonly bool _createInstance;
        private readonly bool _getType;

        private readonly Type _ttype;
        private readonly T _instance;

        private int _anonymousParamsCount;

        protected SqlQueryBuilder(bool createInstance = false, bool getType = false)
            : base()
        {
            _createInstance = createInstance;
            _getType = getType;

            if (createInstance)
                _instance = ReflectionCache.GetObject<T>();

            if (getType)
                _ttype = typeof(T);

            _anonymousParamsCount = 0;
        }

        /*** Set table name
         * 
         * @param ttype
         */
        private void SetTableName()
        {
            SqlTableAttribute tableAttribute = _ttype.GetCustomAttribute<SqlTableAttribute>();
            _tableName = tableAttribute is null ? _ttype.Name : tableAttribute.TableName;
        }

        public SqlQueryBuilder<T> Where(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            PropertyInfo[] tProperties = ReflectionCache.GetProperties<T>();
            List<string> paramKeys = new List<string>();
            string where = ParseExpression(expression.Body, tProperties, paramKeys);

            foreach (string paramKey in paramKeys)
            {
                string pattern = $@"\[(\w+)\]\s*=\s*{Regex.Escape(paramKey)}";

                Match match = Regex.Match(where, pattern);
                if (match.Success)
                {
                    string matchedWhere = match.Captures[0].Value;
                    object paramVal = _parameters.Where(x => x.ParameterName == paramKey).Select(s => s.Value).Single();
                    if (paramVal is DBNull)
                    {
                        string columnName = matchedWhere.Replace($"= {paramKey}", "");

                        where = where.Replace(matchedWhere, $"{columnName} IS NULL");
                    }
                }
            }

            conditions.Add(where);

            return this;
        }

        private string ParseExpression(Expression expression, PropertyInfo[] properties, List<string> paramKeys)
        {
            string paramKey = null;
            switch (expression)
            {
                case BinaryExpression binaryExpression:
                    return ParseBinaryExpression(binaryExpression, properties, paramKeys);
                case MemberExpression memberExpression:
                    PropertyInfo property = properties.FirstOrDefault(p => p.Name == memberExpression.Member.Name);
                    if (property is null)
                    {
                        if (!(memberExpression.Member is FieldInfo fieldInfo))
                            throw new Exception($"");

                        ConstantExpression closureExpression = (ConstantExpression)memberExpression.Expression;

                        object val = fieldInfo.GetValue(closureExpression.Value);
                        paramKey = $"@{memberExpression.Member.Name}";
                        _parameters.Add(new SqlParameter(paramKey, val ?? DBNull.Value));
                        paramKeys.Add(paramKey);

                        return paramKey;
                    }

                    SqlColumnAttribute columnAttribute = property.GetCustomAttribute<SqlColumnAttribute>();

                    return columnAttribute is null ? $"[{memberExpression.Member.Name}]" : $"[{columnAttribute.ColumnName}]";
                case ConstantExpression constantExpression:
                    paramKey = $"@val{++_anonymousParamsCount}";
                    _parameters.Add(new SqlParameter(paramKey, constantExpression.Value ?? DBNull.Value));
                    paramKeys.Add(paramKey);

                    return paramKey;
                default:
                    throw new NotSupportedException($"Expression type {expression.GetType()} is not supported");
            }
        }

        private string ParseBinaryExpression(BinaryExpression binaryExpression, PropertyInfo[] properties, List<string> paramKeys)
        {
            var left = ParseExpression(binaryExpression.Left, properties, paramKeys);
            var right = ParseExpression(binaryExpression.Right, properties, paramKeys);
            var operatorString = GetSqlOperator(binaryExpression.NodeType);

            return $"{left} {operatorString} {right}";
        }

        private string GetSqlOperator(ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "!=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.OrElse:
                    return "OR";
                default:
                    throw new NotSupportedException($"Operator {nodeType} not supported");
            }
        }

        /*** 
         * 
         * @return SqlQueryBuilder
         */
        public SqlQueryBuilder<T> OrderBy(Expression<Func<T, object>> orderBy, bool descending = false)
        {
            if (orderBy == null)
                throw new ArgumentNullException(nameof(orderBy));

            if (_instance == null)
                throw new NullReferenceException(nameof(_instance));

            (Type, string) cacheKey = (_ttype ?? throw new NullReferenceException(nameof(_ttype)), orderBy.ToString());
            string cacheValue = null;
            if (SqlQueryCache.OrderByClauseCache.TryGetValue(cacheKey, out cacheValue))
            {
                _orderClause.Add(cacheValue);
                return this;
            }

            object orderByObject = orderBy.Compile().Invoke(_instance);
            IEnumerable<string> columnEnumrable = orderByObject.GetType().GetProperties().Select(p => p.Name);
            if (!columnEnumrable.Any())
                throw new ArgumentException("No columns selected for ordering");

            if(columnEnumrable.Count() > 1)
                throw new ArgumentException("OrderBy expression should select only one column");

            string selectedColumn = columnEnumrable.SingleOrDefault();

            string orderClause = (descending) ? $"[{selectedColumn}] DESC" : $"[{selectedColumn}] ASC";
            _orderClause.Append(orderClause);
            SqlQueryCache.OrderByClauseCache.AddOrUpdate(cacheKey, orderClause, (key, oldvalue) => orderClause);

            return this;
        }
    }

    public partial class SqlQueryBuilder<T>
    {
        private static void Selector(SqlQueryBuilder<T> builder, Expression<Func<T, object>> selector, out object selectorObject, out string[] selectorColumnList, out (Type, string) cacheKey)
        {
            selectorObject = null;
            selectorColumnList = null;
            cacheKey = (builder._ttype ?? throw new NullReferenceException(nameof(builder._ttype)), null);

            if (selector != null)
            {
                if (builder._instance == null)
                    throw new NullReferenceException(nameof(builder._instance));

                selectorObject = selector.Compile().Invoke(builder._instance);
                cacheKey.Item2 = selector.ToString();
                selectorColumnList = selectorObject.GetType().GetProperties().Select(p => p.Name).ToArray();
            }
        }

        /*** Select statement builder
         * 
         * @param selector
         * @return SqlQueryBuilder
         */
        public static SqlQueryBuilder<T> Select(Expression<Func<T, object>> selector = null)
        {
            SqlQueryBuilder<T> builder = new SqlQueryBuilder<T>(createInstance: true, getType: true);
            Selector(builder, selector, out object selectorObject, out string[] selectorColumnList, out (Type, string) cacheKey);

            string cacheValue = null;
            if (SqlQueryCache.TryGetSelectStatement(cacheKey, out cacheValue))
            {
                builder.query.Append(cacheValue);

                return builder;
            }

            builder.SetTableName();

            if (selectorObject is null)
            {
                builder.query.Append($"SELECT * FROM [{builder._tableName}]");
                SqlQueryCache.AddOrUpdateSelectStatement(cacheKey, builder.query.ToString());

                return builder;
            }

            if (selectorColumnList.Length == 0)
                throw new ArgumentException("No columns selected");

            PropertyInfo[] tProperties = ReflectionCache.GetProperties<T>();
            List<string> columnList = new List<string>();
            foreach (string selectorColumn in selectorColumnList)
            {
                PropertyInfo tProperty = tProperties.FirstOrDefault(p => p.Name == selectorColumn);
                if (tProperty is null)
                    continue;

                SqlColumnAttribute sqlColumnAttribute = tProperty.GetCustomAttribute<SqlColumnAttribute>();
                if (sqlColumnAttribute is null)
                    columnList.Add($"[{selectorColumn}]");
                else
                    columnList.Add($"[{sqlColumnAttribute.ColumnName}]");
            }

            builder.query.Append($"SELECT {string.Join(", ", columnList)} FROM [{builder._tableName}]");
            SqlQueryCache.AddOrUpdateSelectStatement(cacheKey, builder.query.ToString());

            return builder;
        }

        /*** Insert statement builder
         * 
         * @param record
         * @return SqlQueryBuilder
         */
        public static SqlQueryBuilder<T> Insert(T record)
        {
            SqlQueryBuilder<T> builder = new SqlQueryBuilder<T>(getType: true);
            PropertyInfo[] recordProperties = ReflectionCache.GetProperties<T>();

            if (SqlQueryCache.InsertStatementCache.TryGetValue(builder._ttype ?? throw new NullReferenceException(nameof(builder._ttype)), out string cacheValue))
            {
                builder.query.Append(cacheValue);
                return builder;
            }

            builder.SetTableName();

            StringBuilder insertColumnBuilder = new StringBuilder();
            StringBuilder setValueBuilder = new StringBuilder();

            foreach (PropertyInfo property in recordProperties)
            {
                SqlColumnAttribute columnAttribute = property.GetCustomAttribute<SqlColumnAttribute>();
                string columnName = null;

                if (columnAttribute is null)
                {
                    columnName = property.Name;
                    insertColumnBuilder.Append($"[{columnName}], ");
                    setValueBuilder.Append($"@{columnName}, ");
                    builder._parameters.Add(new SqlParameter($"@{columnName}", property.GetValue(record) ?? DBNull.Value));
                }
                else
                {
                    columnName = columnAttribute.ColumnName;
                    if (columnAttribute.PrimaryKey && columnAttribute.AutoIncrement)
                    {
                        continue;
                    }
                    else
                    {
                        insertColumnBuilder.Append($"[{columnName}], ");
                        setValueBuilder.Append($"@{columnName}, ");
                        builder._parameters.Add(new SqlParameter($"@{columnName}", property.GetValue(record) ?? DBNull.Value));
                    }
                }
            }

            insertColumnBuilder.Length -= 2;
            setValueBuilder.Length -= 2;
            builder.query.Append($"INSERT INTO [{builder._tableName}] ({insertColumnBuilder}) VALUES ({setValueBuilder})");
            SqlQueryCache.InsertStatementCache.AddOrUpdate(builder._ttype, builder.query.ToString(), (key, oldvalue) => builder.query.ToString());

            return builder;
        }

        /*** Update statement builder
         * 
         * @param record
         * @param selector
         * @return SqlQueryBuilder
         */
        public static SqlQueryBuilder<T> Update(T record, Expression<Func<T, object>> selector = null)
        {
            SqlQueryBuilder<T> builder = new SqlQueryBuilder<T>(getType: true);
            Type ttype = typeof(T);
            //Selector(builder, selector, out object? selectorObject, out string[]? selectorColumnList, out (Type, string) cacheKey);

            object selectorObject = null;
            string[] selectorColumnList = null;
            (Type, string) cacheKey = (ttype, null);
            if (selector != null)
            {
                selectorObject = selector.Compile().Invoke(record);
                cacheKey.Item2 = selector.ToString();
                selectorColumnList = selectorObject.GetType().GetProperties().Select(p => p.Name).ToArray();
            }

            string cacheValue = null;
            if (SqlQueryCache.UpdateStatementCache.TryGetValue(cacheKey, out cacheValue))
            {
                builder.query.Append(cacheValue);
                return builder;
            }

            builder.SetTableName();

            PropertyInfo[] recordProperties = null;
            if (selectorObject is null)
                recordProperties = ReflectionCache.GetProperties<T>();
            else
            {
                if (selectorColumnList.Length == 0)
                    throw new ArgumentException("No columns selected");

                recordProperties = ReflectionCache.GetProperties<T>()
                    .Where(x => selectorColumnList.Any(columnName => columnName == x.Name)).ToArray();
            }

            StringBuilder setValueBuilder = new StringBuilder();

            foreach (PropertyInfo property in recordProperties)
            {
                SqlColumnAttribute columnAttribute = property.GetCustomAttribute<SqlColumnAttribute>();
                string columnName = null;

                if (columnAttribute is null)
                {
                    columnName = property.Name;
                    setValueBuilder.Append($"[{columnName}] = @{columnName}, ");
                    builder._parameters.Add(new SqlParameter($"@{columnName}", property.GetValue(record) ?? DBNull.Value));
                }
                else
                {
                    columnName = columnAttribute.ColumnName;
                    if (columnAttribute.PrimaryKey)
                    {
                        continue;
                    }
                    else
                    {
                        setValueBuilder.Append($"[{columnName}] = @{columnName}, ");
                        builder._parameters.Add(new SqlParameter($"@{columnName}", property.GetValue(record) ?? DBNull.Value));
                    }
                }
            }
            setValueBuilder.Length -= 2;
            builder.query.Append($"UPDATE [{builder._tableName}] SET {setValueBuilder}");
            SqlQueryCache.UpdateStatementCache.AddOrUpdate(cacheKey, builder.query.ToString(), (key, oldvalue) => builder.query.ToString());

            return builder;
        }

        /*** Delete statement builder
         * 
         * @return SqlQueryBuilder
         */
        public static SqlQueryBuilder<T> Delete()
        {
            SqlQueryBuilder<T> builder = new SqlQueryBuilder<T>(getType: true);

            builder.SetTableName();
            builder.query.Append($"DELETE FROM [{builder._tableName}]");

            return builder;
        }

        /*** 
         * 
         * @return SqlQueryBuilder
         */
        public static SqlQueryBuilder<T> Count()
        {
            SqlQueryBuilder<T> builder = new SqlQueryBuilder<T>(getType: true);

            builder.SetTableName();
            builder.query.Append($"SELECT CAST(COUNT(*) AS BIGINT) FROM [{builder._tableName}]");

            return builder;
        }

    }
}
