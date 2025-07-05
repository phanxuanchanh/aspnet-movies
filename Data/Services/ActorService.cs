using Data.BLL;

namespace Data.Services
{
    public class ActorService
    {
        private readonly GeneralDao _generalDao;
        private readonly CastBLL _actorDao;

        public ActorService(GeneralDao generalDao) {
            _generalDao = generalDao;
            _actorDao = _generalDao.ActorDao;
        }


    }
}