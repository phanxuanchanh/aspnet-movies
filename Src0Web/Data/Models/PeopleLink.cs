using MSSQL;
using MSSQL.Attributes;

namespace Data.Models
{
    [SqlTable("PeopleLinks")]
    public class PeopleLink : ISqlTable
    {
        public string FilmId { get; set; }
        public long PersonId { get; set; }
    }
}