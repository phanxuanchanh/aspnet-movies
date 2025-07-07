

using Data.DAL;

namespace Data.BLL
{
    public class TaxonomyDao
    {
        private readonly DBContext _context;

        public TaxonomyDao(DBContext context)
        {
            _context = context;
        }

    }
}
