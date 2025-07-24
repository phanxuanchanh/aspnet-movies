using MSSQL.Config;
using MSSQL.Execution;
using MSSQL.Mapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MSSQL.Access
{
    internal partial class SqlData : SqlExecution
    {
        public async Task<Dictionary<string, object>> ToDictionaryAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = await ExecuteReaderAsync<SqlDataReader>(sqlCommand))
                {
                    return sqlConvert.ToDictionary(sqlDataReader);
                }
            }

            using (DataSet dataSet = await ExecuteReaderAsync<DataSet>(sqlCommand))
            {
                return sqlConvert.ToDictionary(dataSet);
            }
        }

        public async Task<List<Dictionary<string, object>>> ToDictionaryListAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = await ExecuteReaderAsync<SqlDataReader>(sqlCommand))
                {
                    return sqlConvert.ToDictionaryList(sqlDataReader);
                }
            }

            using (DataSet dataSet = await ExecuteReaderAsync<DataSet>(sqlCommand))
            {
                return sqlConvert.ToDictionaryList(dataSet);
            }
        }

        public async Task<T> ToAsync<T>(SqlCommand sqlCommand) where T : ISqlTable, new()
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = await ExecuteReaderAsync<SqlDataReader>(sqlCommand))
                {
                    return SqlMapper.MapRow<T>(sqlDataReader);
                    //return sqlConvert.To<T>(sqlDataReader);
                }
            }

            using (DataSet dataSet = await ExecuteReaderAsync<DataSet>(sqlCommand))
            {
                if(dataSet.Tables[0].Rows[0] == null || dataSet.Tables[0].Rows.Count == 0)
                    return default(T);

                return SqlMapper.MapRow<T>(dataSet.Tables[0].Rows[0]);
                
                //return sqlConvert.To<T>(dataSet);
            }
        }

        public async Task<List<T>> ToListAsync<T>(SqlCommand sqlCommand) where T : ISqlTable, new()
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
            {
                using (SqlDataReader sqlDataReader = await ExecuteReaderAsync<SqlDataReader>(sqlCommand))
                {
                    List<T> list = new List<T>();
                    while(sqlDataReader.Read())
                        list.Add(SqlMapper.MapRow<T>(sqlDataReader));

                    return list;
                    //return sqlConvert.ToList<T>(sqlDataReader);
                }
            }

            using (DataSet dataSet = await ExecuteReaderAsync<DataSet>(sqlCommand))
            {
                if (dataSet.Tables[0].Rows.Count == 0)
                    return new List<T>();

                List<T> list = new List<T>();
                foreach(DataRow row in dataSet.Tables[0].Rows)
                    list.Add(SqlMapper.MapRow<T>(row));

                return list;
            }
        }

        public async Task<object> ToOriginalDataAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
                return await ExecuteReaderAsync<SqlDataReader>(sqlCommand);
            return await ExecuteReaderAsync<DataSet>(sqlCommand);
        }
    }
}
