using Data.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class TaxonomyService
    {
        private readonly GeneralDao _generalDao;
        private readonly TaxonomyDao _taxonomyDao;

        public TaxonomyService(GeneralDao generalDao)
        {
            _generalDao = generalDao;
            _taxonomyDao = _generalDao.TaxonomyDao;
        }
    }
}