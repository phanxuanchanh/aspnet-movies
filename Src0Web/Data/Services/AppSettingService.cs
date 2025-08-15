using Data.DAOs;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Data.Services
{
    public class AppSettingService
    {
        private readonly AppSettingDao _appSettingDao;

        public AppSettingService(AppSettingDao appSettingDao) {
            _appSettingDao = appSettingDao;
        }

        public async Task<AppSetting> GetSettingAsync(int id)
        {
            return await _appSettingDao.GetAsync(id);
        }

        public async Task<PagedList<AppSetting>> GetSettingsAsync(long pageIndex, long pageSize, string searchText = null)
        {
            long skip = (pageIndex - 1) * pageSize;
            List<AppSetting> settings = await _appSettingDao.GetManyAsync(skip, pageSize, searchText);
            long totalItems = await _appSettingDao.CountAsync(searchText);

            return new PagedList<AppSetting>
            {
                Items = settings,
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalItems = totalItems,
            };
        }

        public async Task<int> AddAsync(AppSetting setting)
        {
            return await _appSettingDao.AddAsync(setting);
        }

        public async Task<int> UpdateAsync(AppSetting setting)
        {
            return await _appSettingDao.UpdateAsync(setting);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _appSettingDao.DeleteAsync(id);
        }
    }
}
