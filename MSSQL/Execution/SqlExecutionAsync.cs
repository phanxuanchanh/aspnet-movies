using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MSSQL.Execution
{
    internal partial class SqlExecution
    {
        public async Task ConnectAsync()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                await sqlConnection.OpenAsync();
        }

        public async Task<int> ExecuteNonQueryAsync(SqlCommand sqlCommand)
        {
            InitSqlCommand(sqlCommand);
            return await sqlCommand.ExecuteNonQueryAsync();
        }

        public async Task<T> ExecuteReaderAsync<T>(SqlCommand sqlCommand)
        {
            Type type = typeof(T);
            if (type.Name != "SqlDataReader" && type.Name != "DataSet")
                throw new Exception("Invalid type, must be @'SqlDataReader' or @'DataSet'");

            InitSqlCommand(sqlCommand);
            if (type.Name == "SqlDataReader")
            {
                SqlDataReader reader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                return (T)Convert.ChangeType(reader, type);
            }

            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
            {
                DataSet dataSet = sqlConvert.GetDataSetFromSqlDataAdapter(sqlDataAdapter);
                return (T)Convert.ChangeType(dataSet, type);
            }
        }

        public async Task<object> ExecuteScalarAsync(SqlCommand sqlCommand)
        {
            InitSqlCommand(sqlCommand);
            return await sqlCommand.ExecuteScalarAsync();
        }
    }
}
