using MSSQL;
using MSSQL.Attributes;
using System;

namespace Data.DAL
{
    public class People : SqlTableWithTimestamp
    {
        [SqlColumn("id", PrimaryKey = true)]
        public long Id { get; set; }

        [SqlColumn("name")]
        public string Name { get; set; }

        [SqlColumn("description")]
        public string Description { get; set; }

        [SqlColumn("type")]
        public string Type { get; set; }
    }
}