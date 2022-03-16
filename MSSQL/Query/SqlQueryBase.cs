using MSSQL.LambdaExpression;
using MSSQL.Mapping;
using MSSQL.Reflection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MSSQL.Query
{
    internal class SqlQueryBase
    {
        private ExpressionExtension expressionExtension;
        protected SqlMapping sqlMapping;
        private ObjReflection objReflection;
        private bool enclosedInSquareBrackets;

        public SqlQueryBase(bool enclosedInSquareBrackets)
        {
            expressionExtension = new ExpressionExtension();
            sqlMapping = new SqlMapping();
            objReflection = new ObjReflection();
            this.enclosedInSquareBrackets = enclosedInSquareBrackets;
        }

        protected bool IsComparisonOperator(ExpressionType expressionType)
        {
            if (
                expressionType == ExpressionType.Equal
               || expressionType == ExpressionType.NotEqual
               || expressionType == ExpressionType.GreaterThan
               || expressionType == ExpressionType.LessThan
               || expressionType == ExpressionType.GreaterThanOrEqual
               || expressionType == ExpressionType.LessThanOrEqual
            )
            {
                return true;
            }
            return false;
        }

        protected bool IsLogicalOperator(ExpressionType expressionType)
        {
            if (
                expressionType == ExpressionType.AndAlso
                || expressionType == ExpressionType.OrElse
                || expressionType == ExpressionType.Not
                || expressionType == ExpressionType.And
                || expressionType == ExpressionType.Or
            )
            {
                return true;
            }
            return false;
        }

        protected ExpressionTree GetExpressionTree<T>(Expression<Func<T, bool>> expression)
        {
            return GetExpressionTree(expression.Body as BinaryExpression);
        }

        protected ExpressionTree GetExpressionTree(BinaryExpression binaryExpression)
        {
            if (binaryExpression == null)
                throw new Exception("@'binaryExpression' must not be null");
            if (IsLogicalOperator(binaryExpression.NodeType))
            {
                ExpressionTree expressionTreeLeft = GetExpressionTree(binaryExpression.Left as BinaryExpression);
                ExpressionTree expressionTreeRight = GetExpressionTree(binaryExpression.Right as BinaryExpression);
                return new ExpressionTree
                {
                    Data = new ExpressionData { NodeType = binaryExpression.NodeType },
                    Left = expressionTreeLeft,
                    Right = expressionTreeRight
                };
            }
            else if (IsComparisonOperator(binaryExpression.NodeType))
            {
                ExpressionData expressionData = GetExpressionData(binaryExpression);
                return new ExpressionTree
                {
                    Data = expressionData,
                    Left = null,
                    Right = null,
                };
            }
            return null;
        }

        protected ExpressionData GetExpressionData(BinaryExpression binaryExpression)
        {
            if (binaryExpression == null)
                throw new Exception("@'binaryExpression' must not be null");

            Expression expressionLeft = binaryExpression.Left;
            Expression expressionRight = binaryExpression.Right;

            if (!(expressionLeft is MemberExpression))
                throw new Exception("");

            string key = (expressionLeft as MemberExpression).Member.Name;
            UnaryExpression unaryExpression = Expression.Convert(expressionRight, typeof(object));
            Func<object> func = Expression.Lambda<Func<object>>(unaryExpression).Compile();
            object value = func();

            return new ExpressionData
            {
                Key = key,
                Value = value,
                NodeType = binaryExpression.NodeType
            };
        }

        protected string GetWherePatternStatement(ExpressionTree expressionTree)
        {
            if (expressionTree == null)
                return null;
            ExpressionData expressionData = expressionTree.Data;
            if (expressionData == null)
                throw new Exception("");
            if (IsLogicalOperator(expressionData.NodeType))
            {
                string left = GetWherePatternStatement(expressionTree.Left);
                string right = GetWherePatternStatement(expressionTree.Right);
                string nodeType = expressionExtension.ConvertExpressionTypeToString(expressionData.NodeType);
                return string.Format("({0} {1} {2})", left, nodeType, right);
            }
            else if (IsComparisonOperator(expressionData.NodeType))
            {
                if (expressionData.Key == null)
                    throw new Exception("");
                string propName = (enclosedInSquareBrackets) ? string.Format("[{0}]", expressionData.Key) : expressionData.Key;
                string nodeType = expressionExtension.ConvertExpressionTypeToString(expressionData.NodeType);
                return string.Format("({0} {1} {2})", propName, nodeType, string.Format("@{0}_where", expressionData.Key));
            }
            return null;
        }

        protected List<SqlQueryParameter> GetKeyAndValueOfExpressionTree(ExpressionTree expressionTree)
        {
            if (expressionTree == null)
                return null;
            ExpressionData expressionData = expressionTree.Data;
            if (expressionData == null)
                throw new Exception("");
            if (IsLogicalOperator(expressionData.NodeType))
            {
                List<SqlQueryParameter> left = GetKeyAndValueOfExpressionTree(expressionTree.Left);
                List<SqlQueryParameter> right = GetKeyAndValueOfExpressionTree(expressionTree.Right);
                return left.Concat(right).ToList();
            }
            else if (IsComparisonOperator(expressionData.NodeType))
            {
                if (expressionData.Key == null)
                    throw new Exception("");
                List<SqlQueryParameter> sqlParameters = new List<SqlQueryParameter>();
                sqlParameters.Add(new SqlQueryParameter
                {
                    Name = string.Format("@{0}_where", expressionData.Key),
                    Value = expressionData.Value,
                    SqlType = default(System.Data.SqlDbType)
                });
                return sqlParameters;
            }
            return null;
        }

        protected SqlQueryData GetWhereStatement<T>(Expression<Func<T, bool>> where)
        {
            ExpressionTree expressionTree = GetExpressionTree<T>(where);
            string whereStatement = GetWherePatternStatement(expressionTree);
            List<SqlQueryParameter> sqlParameters = GetKeyAndValueOfExpressionTree(expressionTree);
            return new SqlQueryData { Statement = string.Format("where {0}", whereStatement), SqlQueryParameters = sqlParameters };
        }

        protected string GetInsertPattern<T>(T model)
        {
            string query = string.Format("Insert into {0}(", sqlMapping.GetTableName<T>(enclosedInSquareBrackets));
            PropertyInfo[] props = objReflection.GetProperties(model);
            string into = null;
            string values = null;
            foreach (PropertyInfo prop in props)
            {
                into += string.Format("{0}, ", sqlMapping.GetPropertyName(prop, enclosedInSquareBrackets));
                values += string.Format("{0}, ", "@" + sqlMapping.GetPropertyName(prop, false));
            }
            into = into.TrimEnd(' ').TrimEnd(',');
            values = values.TrimEnd(' ').TrimEnd(',');
            return string.Format("{0} {1}) values ({2})", query, into, values);
        }

        protected string GetInsertPattern<T>(T model, List<string> excludeProperties)
        {
            if (excludeProperties == null)
                throw new Exception("");
            if (excludeProperties.Count == 0)
                throw new Exception("");
            string query = string.Format("Insert into {0}(", sqlMapping.GetTableName<T>(enclosedInSquareBrackets));
            PropertyInfo[] props = objReflection.GetProperties(model);
            string into = null;
            string values = null;
            foreach (PropertyInfo prop in props)
            {
                if (excludeProperties.Any(e => e.Equals(prop.Name)))
                    continue;
                into += string.Format("{0}, ", sqlMapping.GetPropertyName(prop, enclosedInSquareBrackets));
                values += string.Format("{0}, ", "@" + sqlMapping.GetPropertyName(prop, false));
            }
            into = into.TrimEnd(' ').TrimEnd(',');
            values = values.TrimEnd(' ').TrimEnd(',');
            return string.Format("{0} {1}) values ({2})", query, into, values);
        }

        protected List<SqlQueryParameter> GetParameterOfInsertQuery<T>(T model)
        {
            PropertyInfo[] props = objReflection.GetProperties(model);
            List<SqlQueryParameter> sqlQueryParameters = new List<SqlQueryParameter>();
            foreach(PropertyInfo prop in props)
            {
                sqlQueryParameters.Add(new SqlQueryParameter
                {
                    Name = "@" + prop.Name,
                    Value = prop.GetValue(model)
                });
            }
            return sqlQueryParameters;
        }

        protected List<SqlQueryParameter> GetParameterOfInsertQuery<T>(T model, List<string> excludeProperties)
        {
            if (excludeProperties == null)
                throw new Exception("");
            if (excludeProperties.Count == 0)
                throw new Exception("");
            PropertyInfo[] props = objReflection.GetProperties(model);
            List<SqlQueryParameter> sqlQueryParameters = new List<SqlQueryParameter>();
            foreach (PropertyInfo prop in props)
            {
                if (excludeProperties.Any(e => e.Equals(prop.Name)))
                    continue;
                sqlQueryParameters.Add(new SqlQueryParameter
                {
                    Name = "@" + prop.Name,
                    Value = prop.GetValue(model)
                });
            }
            return sqlQueryParameters;
        }

        protected SqlQueryData GetInsertQueryData<T>(T model)
        {
            return new SqlQueryData
            {
                Statement = GetInsertPattern<T>(model),
                SqlQueryParameters = GetParameterOfInsertQuery<T>(model)
            };
        }

        protected SqlQueryData GetInsertQueryData<T>(T model, List<string> excludeProperties)
        {
            return new SqlQueryData
            {
                Statement = GetInsertPattern<T>(model, excludeProperties),
                SqlQueryParameters = GetParameterOfInsertQuery<T>(model, excludeProperties)
            };
        }

        protected string GetSelectStatement<T>(Expression<Func<T, object>> select)
        {
            string selectStatement = "Select ";
            if (select.ToString().Contains("<>f__AnonymousType"))
            {
                Func<T, object> func = select.Compile();
                T model = objReflection.CreateInstance<T>();
                object obj = func(model);
                PropertyInfo[] properties = objReflection.GetProperties(obj);
                foreach (PropertyInfo property in properties)
                {
                    selectStatement += sqlMapping.GetPropertyName(property, enclosedInSquareBrackets) + ", ";
                }
                selectStatement = selectStatement.TrimEnd(' ').TrimEnd(',');
            }
            else
            {
                if (!(select.Body is MemberExpression))
                    throw new Exception("");
                string propName = (select.Body as MemberExpression).Member.Name;
                selectStatement += ((enclosedInSquareBrackets) ? string.Format("[{0}]", propName) : propName);
            }
            return selectStatement;
        }

        protected string GetOrderByStatement<T>(Expression<Func<T, object>> selectedProperty, SqlOrderByOptions sqlOrderByOption)
        {
            string orderbyStatement = "order by ";
            if (selectedProperty.ToString().Contains("<>f__AnonymousType"))
            {
                Func<T, object> func = selectedProperty.Compile();
                T model = objReflection.CreateInstance<T>();
                object obj = func(model);
                PropertyInfo[] properties = objReflection.GetProperties(obj);
                if (properties.Length == 0 || properties.Length > 1)
                    throw new Exception("");

                PropertyInfo property = properties.First();
                orderbyStatement += 
                    sqlMapping.GetPropertyName(property, enclosedInSquareBrackets)
                    + ((sqlOrderByOption == SqlOrderByOptions.Asc) ? " asc" : " desc");
            }
            else
            {
                if (!(selectedProperty.Body is MemberExpression))
                    throw new Exception("");
                string propName = (selectedProperty.Body as MemberExpression).Member.Name;
                orderbyStatement += 
                    ((enclosedInSquareBrackets) ? string.Format("[{0}]", propName) : propName)
                    + ((sqlOrderByOption == SqlOrderByOptions.Asc) ? " asc" : " desc"); ;
            }
            return orderbyStatement;
        }

        protected SqlQueryData GetSetStatement<T>(T model, Expression<Func<T, object>> set)
        {
            string setStatement = "set ";
            Func<T, object> func = set.Compile();
            object obj = func(model);
            List<SqlQueryParameter> sqlQueryParameters = new List<SqlQueryParameter>();
            string paramName;
            if (set.ToString().Contains("<>f__AnonymousType"))
            {
                PropertyInfo[] properties = objReflection.GetProperties(obj);
                foreach (PropertyInfo property in properties)
                {
                    paramName = string.Format("@{0}_set", sqlMapping.GetPropertyName(property, false));
                    setStatement += string.Format(
                        "{0} = {1}, ",
                        sqlMapping.GetPropertyName(property, enclosedInSquareBrackets),
                        paramName
                    );
                    sqlQueryParameters.Add(new SqlQueryParameter
                    {
                        Name = paramName,
                        Value = property.GetValue(obj)
                    });
                }
                setStatement = setStatement.TrimEnd(' ').TrimEnd(',');
            }
            else
            {
                if (!(set.Body is MemberExpression))
                    throw new Exception("");
                string propName = (set.Body as MemberExpression).Member.Name;
                paramName = string.Format("@{0}_set", propName);
                setStatement += string.Format(
                    "{0} = {1}",
                    (enclosedInSquareBrackets) ? string.Format("[{0}]", propName) : propName,
                    paramName
                );
                sqlQueryParameters.Add(new SqlQueryParameter
                {
                    Name = paramName,
                    Value = obj
                });
            }
            return new SqlQueryData { Statement = setStatement, SqlQueryParameters = sqlQueryParameters };
        }

        protected string GetSkipTakeStatement(long skip, long take)
        {
            return string.Format("offset {0} rows fetch next {1} rows only", skip, take);
        }

        protected SqlCommand InitSqlCommand(string commandText, List<SqlQueryParameter> sqlQueryParameters)
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand();
            sqlCommand.CommandText = commandText;

            foreach (SqlQueryParameter sqlQueryParameter in sqlQueryParameters)
            {
                sqlCommand.Parameters.Add(new SqlParameter(
                    sqlQueryParameter.Name, 
                    (sqlQueryParameter.Value == null) ? DBNull.Value : sqlQueryParameter.Value)
                );
            }
            return sqlCommand;
        }

        protected SqlCommand InitSqlCommand(string commandText)
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand();
            sqlCommand.CommandText = commandText;
            return sqlCommand;
        }
    }
}
