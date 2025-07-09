using Data.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class PeopleLinkDao
    {
        private readonly DBContext _context;

        internal PeopleLinkDao(DBContext context)
        {
            _context = context;
        }

        public async Task<List<PeopleLink>> GetsByFilmIdAsync(string filmId)
        {
            return await _context.PeopleLinks.ToListAsync(x => x.FilmId == filmId);
        }

        public async Task<int> AddAsync(string filmId, long personId)
        {
            PeopleLink peopleLink = new PeopleLink { FilmId = filmId, PersonId = personId };
            return await _context.PeopleLinks.InsertAsync(peopleLink);
        }

        public async Task<int> DeleteAsync(string filmId, long personId)
        {
            return await _context.PeopleLinks.DeleteAsync(x => x.FilmId == filmId && x.PersonId == personId);
        }
    }
}
