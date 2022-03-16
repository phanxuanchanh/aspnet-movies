using MSSQL.Mapping;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MSSQL.Execution
{
    internal partial class SqlExecution : IDisposable
    {
        private SqlConnection sqlConnection;
        protected SqlConvert sqlConvert;
        private bool disposedValue;

        public SqlExecution(string sqlConnectionString)
        {
            sqlConnection = new SqlConnection(sqlConnectionString);
            sqlConvert = new SqlConvert();
            disposedValue = false;
        }

        public void Connect()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }

        public void Disconnect()
        {
            if (sqlConnection.State == ConnectionState.Open)
                sqlConnection.Close();
        }

        private void InitSqlCommand(SqlCommand sqlCommand)
        {
            sqlCommand.Connection = sqlConnection;
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand)
        {
            InitSqlCommand(sqlCommand);
            return sqlCommand.ExecuteNonQuery();
        }

        public T ExecuteReader<T>(SqlCommand sqlCommand)
        {
            Type type = typeof(T);
            if (type.Name != "SqlDataReader" && type.Name != "DataSet")
                throw new Exception("Invalid type, must be @'SqlDataReader' or @'DataSet'");

            InitSqlCommand(sqlCommand);
            if (type.Name == "SqlDataReader")
            {
                SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                return (T)Convert.ChangeType(reader, type);
            }

            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
            {
                DataSet dataSet = sqlConvert.GetDataSetFromSqlDataAdapter(sqlDataAdapter);
                return (T)Convert.ChangeType(dataSet, type);
            }
        }

        public object ExecuteScalar(SqlCommand sqlCommand)
        {
            InitSqlCommand(sqlCommand);
            return sqlCommand.ExecuteScalar();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    sqlConnection.Dispose();
                    sqlConnection = null;
                    sqlConvert = null;
                }
                disposedValue = true;
            }
        }
        ~SqlExecution()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
