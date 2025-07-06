using Data.DAL;
using System;

namespace Data.BLL
{
    public class GeneralDao : IDisposable
    {
        private readonly DBContext _context;
        private bool disposedValue;

        public GeneralDao() {
            _context = new DBContext();
        }

        public FilmMetadataDao FilmMetadataDao { get { return new FilmMetadataDao(_context); } }

        public CastBLL ActorDao { get { return new CastBLL(); } }
        public CategoryBLL CategoryDao { get { return new CategoryBLL(); } }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                _context.Dispose();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}