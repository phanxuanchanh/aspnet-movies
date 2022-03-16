using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MSSQL.Access
{
    public partial class SqlContext
    {
        public async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                if (sqlParameters != null)
                    sqlCommand.Parameters.AddRange(sqlParameters);

                return await sqlData.ExecuteNonQueryAsync(sqlCommand);
            }
        }

        public async Task<object> ExecuteScalarAsync(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                if (sqlParameters != null)
                    sqlCommand.Parameters.AddRange(sqlParameters);

                return await sqlData.ExecuteScalarAsync(sqlCommand);
            }
        }

        public async Task<object> Execute_ToOriginalDataAsync(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                if (sqlParameters != null)
                    sqlCommand.Parameters.AddRange(sqlParameters);

                return await sqlData.ToOriginalDataAsync(sqlCommand);
            }
        }

        public async Task<T> Execute_ToAsync<T>(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                if (sqlParameters != null)
                    sqlCommand.Parameters.AddRange(sqlParameters);

                return await sqlData.ToAsync<T>(sqlCommand);
            }
        }

        public async Task<List<T>> Execute_ToListAsync<T>(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                if (sqlParameters != null)
                    sqlCommand.Parameters.AddRange(sqlParameters);

                return await sqlData.ToListAsync<T>(sqlCommand);
            }
        }

        public async Task<Dictionary<string, object>> Execute_ToDictionaryAsync(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                if (sqlParameters != null)
                    sqlCommand.Parameters.AddRange(sqlParameters);

                return await sqlData.ToDictionaryAsync(sqlCommand);
            }
        }

        public async Task<List<Dictionary<string, object>>> Execute_ToDictionaryListAsync(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                if (sqlParameters != null)
                    sqlCommand.Parameters.AddRange(sqlParameters);

                return await sqlData.ToDictionaryListAsync(sqlCommand);
            }
        }
    }
}
