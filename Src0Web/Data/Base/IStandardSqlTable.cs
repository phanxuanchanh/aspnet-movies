using MSSQL;

namespace Data.Base
{
    public interface IStandardSqlTable<TId> : ISqlTable
    {
        TId Id { get; set; }
    }
}
