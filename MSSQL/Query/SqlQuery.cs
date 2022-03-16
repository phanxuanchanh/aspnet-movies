using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace MSSQL.Query
{
    internal class SqlQuery : SqlQueryBase
    {
        public static bool EnclosedInSquareBrackets = true;

        public SqlQuery()
            : base(EnclosedInSquareBrackets)
        {

        }

        public SqlCommand CreateDatabase(string databaseName)
        {
            string query = string.Format("Create database {0}", databaseName);
            return InitSqlCommand(query);
        }

        public SqlCommand UseDatabase(string databaseName)
        {
            string query = string.Format("Use {0}", databaseName);
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>()
        {
            string query = string.Format("Select * from {0}", sqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            string query = string.Format(
                "Select * from {0} {1}", 
                sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                GetOrderByStatement<T>(orderBy, sqlOrderByOption)
            );
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(long skip, long take)
        {
            string query = string.Format(
                "Select * from {0} order by current_timestamp {1}",
                sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                GetSkipTakeStatement(skip, take)
            );
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption, long skip, long take)
        {
            string query = string.Format(
                "Select * from {0} {1} {2}",
                sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                GetOrderByStatement(orderBy, sqlOrderByOption),
                GetSkipTakeStatement(skip, take)
            );
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(long top)
        {
            string query = string
                .Format("Select top {0} * from {1}", top, sqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(long top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            string query = string.Format(
                "Select top {0} * from {1} {2}",
                top,
                sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                GetOrderByStatement<T>(orderBy, sqlOrderByOption)
            );
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string
                .Format("Select * from {0} {1}", sqlMapping.GetTableName<T>(EnclosedInSquareBrackets), sqlQueryData.Statement);
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, bool>> where, long skip, long take)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string.Format(
                "Select * from {0} {1} order by current_timestamp {2}",
                sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                sqlQueryData.Statement,
                GetSkipTakeStatement(skip, take)
            );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string.Format(
                "Select * from {0} {1} {2}", 
                sqlMapping.GetTableName<T>(EnclosedInSquareBrackets), 
                sqlQueryData.Statement,
                GetOrderByStatement(orderBy, sqlOrderByOption)
            );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption, long skip, long take)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string.Format(
                "Select * from {0} {1} {2} {3}",
                sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                sqlQueryData.Statement,
                GetOrderByStatement(orderBy, sqlOrderByOption),
                GetSkipTakeStatement(skip, take)
            );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, bool>> where, long top)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string
                .Format(
                    "Select top {0} * from {1} {2}",
                    top, sqlMapping.GetTableName<T>(EnclosedInSquareBrackets), sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, bool>> where, long top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string
                .Format(
                    "Select top {0} * from {1} {2} {3}",
                    top, 
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets), 
                    sqlQueryData.Statement,
                    GetOrderByStatement<T>(orderBy, sqlOrderByOptions)
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select)
        {
            string query = string
                .Format(
                    "{0} from {1}",
                    GetSelectStatement<T>(select),
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets)
                );
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, long skip, long take)
        {
            string query = string
                .Format(
                    "{0} from {1} order by current_timestamp {2}",
                    GetSelectStatement<T>(select),
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    GetSkipTakeStatement(skip, take)
                );
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            string query = string
                .Format(
                    "{0} from {1} {2}",
                    GetSelectStatement<T>(select),
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    GetOrderByStatement(orderBy, sqlOrderByOptions)
                );
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            string query = string
                .Format(
                    "{0} from {1} {2} {3}",
                    GetSelectStatement<T>(select),
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    GetOrderByStatement<T>(orderBy, sqlOrderByOptions),
                    GetSkipTakeStatement(skip, take)
                );
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, long top)
        {
            string selectStatement = GetSelectStatement<T>(select)
                .Replace("Select ", string.Format("Select top {0} ", top));
            string query = string.Format("{0} from {1}", selectStatement, sqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, long top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            string selectStatement = GetSelectStatement<T>(select)
                .Replace("Select ", string.Format("Select top {0} ", top));
            string query = string.Format(
                "{0} from {1} {2}",
                selectStatement, 
                sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                GetOrderByStatement<T>(orderBy, sqlOrderByOptions)
            );
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string
                .Format(
                    "{0} from {1} {2}",
                    GetSelectStatement<T>(select),
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, long skip, long take)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string
                .Format(
                    "{0} from {1} {2} order by current_timestamp {3}",
                    GetSelectStatement<T>(select),
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement,
                    GetSkipTakeStatement(skip, take)
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string
                .Format(
                    "{0} from {1} {2} {3}",
                    GetSelectStatement<T>(select),
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement,
                    GetOrderByStatement<T>(orderBy, sqlOrderByOptions)
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string
                .Format(
                    "{0} from {1} {2} {3} {4}",
                    GetSelectStatement<T>(select),
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement,
                    GetOrderByStatement<T>(orderBy, sqlOrderByOptions),
                    GetSkipTakeStatement(skip, take)
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, long top)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string selectStatement = GetSelectStatement<T>(select)
                .Replace("Select ", string.Format("Select top {0} ", top));
            string query = string
                .Format(
                    "{0} from {1} {2}",
                    selectStatement,
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, long top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string selectStatement = GetSelectStatement<T>(select)
                .Replace("Select ", string.Format("Select top {0} ", top));
            string query = string
                .Format(
                    "{0} from {1} {2} {3}",
                    selectStatement,
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement,
                    GetOrderByStatement<T>(orderBy, sqlOrderByOptions)
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Count<T>()
        {
            string query = string
                .Format("Select cast(count(*) as varchar(20)) from {0}", sqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public SqlCommand Count<T>(Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string
                .Format(
                    "Select cast(count(*) as varchar(20)) from {0} {1}",
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Count<T>(string propertyName, Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string
                .Format(
                    "Select cast(count({0}) as varchar(20)) from {1} {2}",
                    (EnclosedInSquareBrackets) ? string.Format("[{0}]", propertyName) : propertyName,
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Insert<T>(T model)
        {
            SqlQueryData sqlQueryData = GetInsertQueryData<T>(model);
            return InitSqlCommand(sqlQueryData.Statement, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Insert<T>(T model, List<string> excludeProperties)
        {
            if (excludeProperties == null)
                return Insert<T>(model);
            if (excludeProperties.Count == 0)
                return Insert<T>(model);
            SqlQueryData sqlQueryData = GetInsertQueryData<T>(model, excludeProperties);
            return InitSqlCommand(sqlQueryData.Statement, sqlQueryData.SqlQueryParameters);
        }
        public SqlCommand Update<T>(T model, Expression<Func<T, object>> set)
        {
            SqlQueryData sqlQueryData = GetSetStatement<T>(model, set);
            string query = string
                .Format("Update {0} {1}", sqlMapping.GetTableName<T>(EnclosedInSquareBrackets), sqlQueryData.Statement);
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Update<T>(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData1 = GetSetStatement<T>(model, set);
            SqlQueryData sqlQueryData2 = GetWhereStatement<T>(where);
            string query = string
                .Format(
                    "Update {0} {1} {2}",
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData1.Statement,
                    sqlQueryData2.Statement
                );
            List<SqlQueryParameter> sqlQueryParameters = sqlQueryData1.SqlQueryParameters
                .Concat(sqlQueryData2.SqlQueryParameters).ToList();

            return InitSqlCommand(query, sqlQueryParameters);
        }

        public SqlCommand Delete<T>()
        {
            string query = string.Format("Delete from {0}", sqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public SqlCommand Delete<T>(Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where);
            string query = string
                .Format("Delete from {0} {1}", sqlMapping.GetTableName<T>(EnclosedInSquareBrackets), sqlQueryData.Statement);
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }
    }
}
