using System.Configuration;
using System.Data.SqlClient;

namespace MSSQL.Connection
{
    public class SqlConnectInfo
    {
        private static string _connectionString;

        public static void CreateConnectionString(string dataSource, string initialCatalog, string userId, string password)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.DataSource = dataSource;
            sqlConnectionStringBuilder.InitialCatalog = initialCatalog;
            sqlConnectionStringBuilder.UserID = userId;
            sqlConnectionStringBuilder.Password = password;

            _connectionString = sqlConnectionStringBuilder.ConnectionString;
        }

        public static void ReadFromConfigFile(string connectionStringName)
        {
            _connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        public static string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
