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
        private PeopleLinkDao _peopleLinkDao;
        private TaxonomyDao _taxonomyDao;
        private TaxonomyLinkDao _taxonomyLinkDao;
        private FilmDao _filmDao;

        public GeneralDao() {
            _context = new DBContext();

            _filmMetadataDao = new FilmMetadataDao(_context);
            _filmMetaLinkDao = new FilmMetaLinkDao(_context);
            _peopleDao = new PeopleDao(_context);
            _peopleLinkDao = new PeopleLinkDao(_context);
            _taxonomyDao = new TaxonomyDao(_context);
            _taxonomyLinkDao = new TaxonomyLinkDao(_context);
            _filmDao = new FilmDao(_context);
        }

        public FilmMetadataDao FilmMetadataDao { get { return _filmMetadataDao; } }
        public FilmMetaLinkDao FilmMetaLinkDao { get { return _filmMetaLinkDao; } }
        public PeopleDao PeopleDao { get { return _peopleDao; } }
        public PeopleLinkDao PeopleLinkDao { get { return _peopleLinkDao; } }
        public TaxonomyDao TaxonomyDao { get { return _taxonomyDao; } }
        public TaxonomyLinkDao TaxonomyLinkDao { get { return _taxonomyLinkDao; } }
        public FilmDao FilmDao { get { return _filmDao; } }

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