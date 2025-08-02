using Common;
using Data.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class FilmMetaLinkDao
    {
        private readonly DBContext _context;

        public FilmMetaLinkDao(DBContext context)
        {
            _context = context;
        }

        public async Task<List<FilmMetaLink>> GetManyByFilmIdAsync(string filmId)
        {
            return await _context.FilmMetaLinks
                .Where(x => x.FilmId == filmId).ToListAsync();
        }

        public async Task<int> AddAsync(string filmId, int metaId)
        {
            FilmMetaLink metaLink = new FilmMetaLink { FilmId = filmId, MetaId = metaId };
            return await _context.FilmMetaLinks.InsertAsync(metaLink);
        }

        public async Task<int> DeleteAsync(string filmId, int metaId)
        {
           return await _context.FilmMetaLinks.DeleteAsync(x => x.FilmId == filmId && x.MetaId == metaId);
        }

        public async Task<int> DeleteManyByFilmIdAsync(string filmId)
        {
            return await _context.FilmMetaLinks.DeleteAsync(x => x.FilmId == filmId);
        }
    }
}
