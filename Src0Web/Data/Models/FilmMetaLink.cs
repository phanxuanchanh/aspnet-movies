using MSSQL;
using MSSQL.Attributes;

namespace Data.DAL
{
    [SqlTable("FilmMetaLinks")]
    public class FilmMetaLink : ISqlTable
    {
        public string FilmId { get; set; }
        public int MetaId { get; set; }
    }
}