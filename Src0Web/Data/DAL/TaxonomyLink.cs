using MSSQL;

namespace Data.DAL
{
    public class TaxonomyLink : ISqlTable
    {
        public string FilmId { get; set; }
        public int TaxonomyId { get; set; }
    }
}