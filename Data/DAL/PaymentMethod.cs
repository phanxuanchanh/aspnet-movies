using MSSQL;
using MSSQL.Attributes;
using System;

namespace Data.DAL
{
    [SqlTable("PaymentMethod")]
    public class PaymentMethod : SqlTableWithTimestamp
    {
        [SqlColumn("ID", PrimaryKey = true)]
        public int Id { get; set; }

        [SqlColumn("name")]
        public string Name { get; set; }
    }
}