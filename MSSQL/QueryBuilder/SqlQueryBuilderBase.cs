﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MSSQL.QueryBuilder
{
    public class SqlQueryBuilderBase
    {
        protected string _tableName;
        protected readonly StringBuilder query;
        protected readonly List<SqlParameter> _parameters;

        protected readonly List<string> columns;
        protected readonly List<string> conditions;

        protected readonly List<string> _orderClause;

        protected SqlQueryBuilderBase()
        {
            _tableName = null;
            query = new StringBuilder();
            _parameters = new List<SqlParameter>();
            columns = new List<string>();
            conditions = new List<string>();
            _orderClause = new List<string>();
        }

        public string BuildQuery()
        {
            string whereClause = "";
            if (conditions.Count > 0)
            {
                whereClause = "WHERE " + string.Join(" AND ", conditions);
            }

            string orderByClause = "";
            if (_orderClause.Count > 0)
            {
                orderByClause = " ORDER BY " + string.Join(", ", _orderClause);
            }

            return $"{query.ToString()} {whereClause} {orderByClause}";
        }

        public List<SqlParameter> GetParameters()
        {
            return _parameters;
        }
    }
}