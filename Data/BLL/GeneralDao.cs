using Data.DAL;
using System;

namespace Data.BLL
{
    public class GeneralDao : IDisposable
    {
        private readonly DBContext _context;
        private bool disposedValue;

        private FilmMetadataDao _filmMetadataDao;
        private FilmMetaLinkDao _filmMetaLinkDao;
        private PeopleDao _peopleDao;
        private TaxonomyDao _taxonomyDao;

        public GeneralDao() {
            _context = new DBContext();

            _filmMetadataDao = new FilmMetadataDao(_context);
            _filmMetaLinkDao = new FilmMetaLinkDao(_context);
            _peopleDao = new PeopleDao(_context);
            _taxonomyDao = new TaxonomyDao(_context);
        }

        public FilmMetadataDao FilmMetadataDao { get { return _filmMetadataDao; } }
        public FilmMetaLinkDao FilmMetaLinkDao { get { return _filmMetaLinkDao; } }
        public PeopleDao PeopleDao { get { return _peopleDao; } }
        public TaxonomyDao TaxonomyDao { get { return _taxonomyDao; } }

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