using MSSQL;

namespace Data.DAL
{
    public class FilmMetaLink : ISqlTable
    {
        public string FilmId { get; set; }
        public int MetaId { get; set; }
    }
}