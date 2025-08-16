using Data.Context;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.DAOs
{
    public class PeopleLinkDao
    {
        private readonly DBContext _context;

        public PeopleLinkDao(DBContext context)
        {
            _context = context;
        }

        public async Task<List<PeopleLink>> GetManyByFilmIdAsync(string filmId)
        {
            return await _context.PeopleLinks
                .Where(x => x.FilmId == filmId).ToListAsync();
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

        public async Task<int> DeleteManyByFilmIdAsync(string filmId)
        {
            return await _context.PeopleLinks.DeleteAsync(x => x.FilmId == filmId);
        }
    }
}
