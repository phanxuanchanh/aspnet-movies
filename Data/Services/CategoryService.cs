using Data.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class CategoryService
    {
        private readonly GeneralDao _generalDao;
        private readonly CategoryBLL _categoryBLL;

        public CategoryService(GeneralDao generalDao)
        {
            _generalDao = generalDao;
            _categoryBLL = _generalDao.CategoryDao;

        }


    }
}