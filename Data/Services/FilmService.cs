using Common;
using Data.BLL;
using Data.DAL;
using Data.DTO;
using System.Threading.Tasks;

namespace Data.Services
{
    public class FilmService
    {
        private readonly GeneralDao _generalDao;
        private readonly FilmMetadataDao _filmMetadataDao;
        private readonly FilmMetaLinkDao _filmMetaLinkDao;
        private readonly FilmBLL _filmDao;

        public FilmService(GeneralDao generalDao)
        {
            _generalDao = generalDao;
            _filmMetadataDao = _generalDao.FilmMetadataDao;
            _filmMetaLinkDao = _generalDao.FilmMetaLinkDao;
            _filmDao = new FilmBLL();
        }

        public async Task<ExecResult> CreateFilm(FilmInfo input)
        {
            if (input == null)
                return new ExecResult { Status = ExecStatus.Invalid, Message = "Input cannot be null." };

            int id = 1;// await _filmDao.AddAsync(input);

            if (id <= 0)
                return new ExecResult { Status = ExecStatus.Failure, Message = "Failed to create film." };

            await _filmMetaLinkDao.AddAsync(input.ID, 1);

            return new ExecResult { Status = ExecStatus.Success, Message = "Film created successfully." };
        }
    }
}