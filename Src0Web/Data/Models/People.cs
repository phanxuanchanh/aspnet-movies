using MSSQL;
using MSSQL.Attributes;

namespace Data.Models
{
    public class People : SqlTableWithTimestamp
    {
        [SqlColumn("id", PrimaryKey = true, AutoIncrement = true)]
        public long Id { get; set; }

        [SqlColumn("name")]
        public string Name { get; set; }

        [SqlColumn("description")]
        public string Description { get; set; }

        [SqlColumn("type")]
        public string Type { get; set; }
    }
}