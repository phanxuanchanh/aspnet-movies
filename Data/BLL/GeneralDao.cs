using Data.DAL;

namespace Data.BLL
{
    public class GeneralDao
    {
        private readonly DBContext _context;

        public GeneralDao() {
            _context = new DBContext();
        }

        public FilmMetadataDao FilmMetadataDao { get { return new FilmMetadataDao(_context); } }

        public CastBLL ActorDao { get { return new CastBLL(); } }
        public CategoryBLL CategoryDao { get { return new CategoryBLL(); } }
    }
}