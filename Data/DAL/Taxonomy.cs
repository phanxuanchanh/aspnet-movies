using MSSQL;
using MSSQL.Attributes;

namespace Data.DAL
{
    [SqlTable("Taxonomy")]
    public class Taxonomy : SqlTableWithTimestamp
    {
        [SqlColumn("id", PrimaryKey = true)]
        public int Id { get; set; }

        [SqlColumn("name")]
        public string Name { get; set; }

        [SqlColumn("description")]
        public string Description { get; set; }

        [SqlColumn("type")]
        public string Type { get; set; }
    }
}