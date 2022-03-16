using MSSQL.Access;
using MSSQL.Config;
using MSSQL.Connection;

namespace Data.Config
{
    public static class DatabaseConfig
    {
        public static void CreateConnectionString(string dataSource, string initialCatalog, string userId, string password)
        {
            SqlConnectInfo.CreateConnectionString(dataSource, initialCatalog, userId, password);
        }

        public static void ReadFromConfigFile(string connectionStringName)
        {
            SqlConnectInfo.ReadFromConfigFile(connectionStringName);
        }

        public static void OtherSettings()
        {
            SqlConfig.objectReceivingData = ObjectReceivingData.DataSet;
        }
    }
}
