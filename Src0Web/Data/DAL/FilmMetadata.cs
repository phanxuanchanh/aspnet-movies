using MSSQL;
using MSSQL.Attributes;

namespace Data.DAL
{
    [SqlTable("FilmMetadata")]
    public class FilmMetadata : SqlTableWithTimestamp
    {
        [SqlColumn("id", PrimaryKey = true)]
        public int Id { get; set; }

        [SqlColumn("name")]
        public string Name { get; set; }

        [SqlColumn("description")]
        public string Description { get; set; }

        [SqlColumn("createdAt")]
        public string Custom { get; set; }

        [SqlColumn("updatedAt")]
        public string Type { get; set; }
    }
}