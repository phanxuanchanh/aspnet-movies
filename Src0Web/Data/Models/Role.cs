using MSSQL;
using MSSQL.Attributes;

namespace Data.DAL
{
    [SqlTable("Role")]
    public class Role : SqlTableWithTimestamp
    {
        [SqlColumn("ID")]
        public string Id { get; set; }

        [SqlColumn("Name")]
        public string Name { get; set; }
    }
}
