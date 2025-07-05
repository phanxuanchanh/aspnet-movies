using Data.BLL;

namespace Data.Services
{
    public class FilmService
    {
        private readonly GeneralDao _generalDao;
        private readonly FilmMetadataDao _filmMetadataDao;

        public FilmService(GeneralDao generalDao)
        {
            _generalDao = generalDao;
            _filmMetadataDao = _generalDao.FilmMetadataDao;
        }
    }
}