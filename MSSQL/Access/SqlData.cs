using MSSQL.Config;
using MSSQL.Connection;
using MSSQL.Execution;
using MSSQL.Mapper;
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

        public T To<T>(SqlCommand sqlCommand) where T : ISqlTable, new()
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = ExecuteReader<SqlDataReader>(sqlCommand))
                {
                    return SqlMapper.MapRow<T>(sqlDataReader);
                    //return sqlConvert.To<T>(sqlDataReader);
                }
            }

            using (DataSet dataSet = ExecuteReader<DataSet>(sqlCommand))
            {
                if (dataSet.Tables[0].Rows[0] == null || dataSet.Tables[0].Rows.Count == 0)
                    return default(T);

                return SqlMapper.MapRow<T>(dataSet.Tables[0].Rows[0]);
                //return sqlConvert.To<T>(dataSet);
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
