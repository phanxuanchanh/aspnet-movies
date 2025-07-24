using MSSQL;

namespace Data.DAL
{
    public class PeopleLink : ISqlTable
    {
        public string FilmId { get; set; }
        public long PersonId { get; set; }
    }
}