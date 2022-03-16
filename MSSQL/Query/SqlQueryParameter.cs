using System.Data;

namespace MSSQL.Query
{
    internal class SqlQueryParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public SqlDbType SqlType { get; set; }
    }
}
