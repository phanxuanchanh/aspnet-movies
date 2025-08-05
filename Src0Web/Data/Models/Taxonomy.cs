using MSSQL;
using MSSQL.Attributes;

namespace Data.DAL
{
    [SqlTable("Taxonomy")]
    public class Taxonomy : SqlTableWithTimestamp
    {
        [SqlColumn("Id", PrimaryKey = true, AutoIncrement = true)]
        public int Id { get; set; }

        [SqlColumn("Name")]
        public string Name { get; set; }

        [SqlColumn("Description")]
        public string Description { get; set; }

        [SqlColumn("Type")]
        public string Type { get; set; }
    }
}