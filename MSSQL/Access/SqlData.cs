using MSSQL.Config;
using MSSQL.Connection;
using MSSQL.Execution;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MSSQL.Access
{
    internal partial class SqlData : SqlExecution
    {
        private bool disposed;

        public SqlData()
            : base(SqlConnectInfo.GetConnectionString())
        {
            disposed = false;
        }

        public Dictionary<string, object> ToDictionary(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = ExecuteReader<SqlDataReader>(sqlCommand))
                {
                    return sqlConvert.ToDictionary(sqlDataReader);
                }
            }

            using (DataSet dataSet = ExecuteReader<DataSet>(sqlCommand))
            {
                return sqlConvert.ToDictionary(dataSet);
            }
        }

        public List<Dictionary<string, object>> ToDictionaryList(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = ExecuteReader<SqlDataReader>(sqlCommand))
                {
                    return sqlConvert.ToDictionaryList(sqlDataReader);
                }
            }

            using (DataSet dataSet = ExecuteReader<DataSet>(sqlCommand))
            {
                return sqlConvert.ToDictionaryList(dataSet);
            }
        }

        public T To<T>(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = ExecuteReader<SqlDataReader>(sqlCommand))
                {
                    return sqlConvert.To<T>(sqlDataReader);
                }
            }

            using (DataSet dataSet = ExecuteReader<DataSet>(sqlCommand))
            {
                return sqlConvert.To<T>(dataSet);
            }
        }

        public List<T> ToList<T>(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = ExecuteReader<SqlDataReader>(sqlCommand))
                {
                    return sqlConvert.ToList<T>(sqlDataReader);
                }
            }

            using (DataSet dataSet = ExecuteReader<DataSet>(sqlCommand))
            {
                return sqlConvert.ToList<T>(dataSet);
            }
        }

        public object ToOriginalData(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
                return ExecuteReader<SqlDataReader>(sqlCommand);
            return ExecuteReader<DataSet>(sqlCommand);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                try
                {
                    if (disposing)
                    {

                    }
                    disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
