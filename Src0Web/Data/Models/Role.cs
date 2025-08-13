using Data.Base;
using MSSQL;
using MSSQL.Attributes;

namespace Data.DAL
{
    [SqlTable("Roles")]
    public class Role : SqlTableWithTimestamp, IStandardSqlTable<string>
    {
        [SqlColumn("ID")]
        public string Id { get; set; }

        [SqlColumn("Name")]
        public string Name { get; set; }
    }
}
