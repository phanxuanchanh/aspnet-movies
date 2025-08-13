using MSSQL;
using MSSQL.Attributes;

namespace Data.DAL
{
    [SqlTable("TaxonomyLinks")]
    public class TaxonomyLink : ISqlTable
    {
        public string FilmId { get; set; }
        public int TaxonomyId { get; set; }
    }
}