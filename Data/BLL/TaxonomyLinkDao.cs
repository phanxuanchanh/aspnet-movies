using Data.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class TaxonomyLinkDao
    {
        private readonly DBContext _context;

        internal TaxonomyLinkDao(DBContext context)
        {
            _context = context;
        }

        public async Task<List<TaxonomyLink>> GetsByFilmIdAsync(string filmId)
        {
            return await _context.TaxonomyLinks
                .Where(x => x.FilmId == filmId).ToListAsync();
        }

        public async Task<int> AddAsync(string filmId, int taxonomyId)
        {
            TaxonomyLink taxonomyLink = new TaxonomyLink { FilmId = filmId, TaxonomyId = taxonomyId };
            return await _context.TaxonomyLinks.InsertAsync(taxonomyLink);
        }

        public async Task<int> DeleteAsync(string filmId, int taxonomyId)
        {
            return await _context.TaxonomyLinks.DeleteAsync(x => x.FilmId == filmId && x.TaxonomyId == taxonomyId);
        }
    }
}
