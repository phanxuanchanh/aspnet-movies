using MSSQL;
using MSSQL.Attributes;

namespace Data.Models
{
    [SqlTable("AppSettings")]
    public class AppSetting : ISqlTable
    {
        [SqlColumn("Id", PrimaryKey = true, AutoIncrement = true)]
        public int Id { get; set; }

        [SqlColumn("Name")]
        public string Name { get; set; }

        [SqlColumn("Value")]
        public string Value { get; set; }
    }
}
