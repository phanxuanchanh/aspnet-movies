using Data.DAL;
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

        public async Task AddAsync(string filmId, int metaId)
        {
            FilmMetaLink metaLink = new FilmMetaLink { FilmId = filmId, MetaId = metaId };
            await _context.FilmMetaLinks.InsertAsync(metaLink);
        }

        public async Task DeleteAsync(string filmId, int metaId)
        {
            await _context.FilmMetaLinks.DeleteAsync(x => x.FilmId == filmId && x.MetaId == metaId);
        }
    }
}
