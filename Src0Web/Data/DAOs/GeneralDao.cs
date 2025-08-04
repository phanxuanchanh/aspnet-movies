using Data.DAL;
using Data.DAOs;
using System;

namespace Data.BLL
{
    public class GeneralDao : IDisposable
    {
        private readonly DBContext _context;
        private bool disposedValue;

        private AppSettingDao _appSettingDao;
        private FilmMetadataDao _filmMetadataDao;
        private FilmMetaLinkDao _filmMetaLinkDao;
        private PeopleDao _peopleDao;
        private PeopleLinkDao _peopleLinkDao;
        private TaxonomyDao _taxonomyDao;
        private TaxonomyLinkDao _taxonomyLinkDao;
        private FilmDao _filmDao;
        private RoleDao _roleDao;
        private UserDao _userDao;
        private PaymentMethodDao _paymentMethodDao;

        public GeneralDao() {
            _context = new DBContext();

            _appSettingDao = new AppSettingDao(_context);
            _filmMetadataDao = new FilmMetadataDao(_context);
            _filmMetaLinkDao = new FilmMetaLinkDao(_context);
            _peopleDao = new PeopleDao(_context);
            _peopleLinkDao = new PeopleLinkDao(_context);
            _taxonomyDao = new TaxonomyDao(_context);
            _taxonomyLinkDao = new TaxonomyLinkDao(_context);
            _filmDao = new FilmDao(_context);
            _roleDao = new RoleDao(_context);
            _userDao = new UserDao(_context);
            _paymentMethodDao = new PaymentMethodDao(_context);
        }

        public AppSettingDao AppSettingDao { get { return _appSettingDao; } }
        public FilmMetadataDao FilmMetadataDao { get { return _filmMetadataDao; } }
        public FilmMetaLinkDao FilmMetaLinkDao { get { return _filmMetaLinkDao; } }
        public PeopleDao PeopleDao { get { return _peopleDao; } }
        public PeopleLinkDao PeopleLinkDao { get { return _peopleLinkDao; } }
        public TaxonomyDao TaxonomyDao { get { return _taxonomyDao; } }
        public TaxonomyLinkDao TaxonomyLinkDao { get { return _taxonomyLinkDao; } }
        public FilmDao FilmDao { get { return _filmDao; } }
        public RoleDao RoleDao { get { return _roleDao; } }
        public UserDao UserDao { get { return _userDao; } }
        public PaymentMethodDao PaymentMethodDao { get { return _paymentMethodDao; } }

        public DBContext Context => _context;

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