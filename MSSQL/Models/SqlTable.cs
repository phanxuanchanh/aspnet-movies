using MSSQL.Attributes;
using System;

namespace MSSQL
{
    public interface ISqlTable
    {

    }

    public class SqlTableWithTimestamp : ISqlTable
    {
        [SqlColumn("createdAt")]
        public DateTime? CreatedAt { get; set; } = null;

        [SqlColumn("updatedAt")]
        public DateTime? UpdatedAt { get; set; } = null;

        [SqlColumn("deletedAt")]
        public DateTime? DeletedAt { get; set; } = null;
    }
}
