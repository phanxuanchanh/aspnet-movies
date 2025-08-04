using Data.DAL;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MSSQL.Access;

namespace Data.DAOs
{
    public class AppSettingDao
    {
        private readonly DBContext _context;

        internal AppSettingDao(DBContext context)
        {
            _context = context;
        }

        public async Task<AppSetting> GetAsync(int id)
        {
            return await _context.AppSettings.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppSetting> GetByNameAsync(string name)
        {
            return await _context.AppSettings.SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<AppSetting>> GetManyAsync(long skip = 1, long take = 10, string searchText = null)
        {
            SqlAccess<AppSetting> access = _context.AppSettings
                .OrderBy(o => new { o.Id });

            if (!string.IsNullOrEmpty(searchText))
                access.Where(x => x.Name.Contains(searchText));

            return await access.ToListAsync();
        }

        public async Task<long> CountAsync(string searchText = null)
        {
            SqlAccess<AppSetting> access = _context.AppSettings;

            if (!string.IsNullOrEmpty(searchText))
                access.Where(x => x.Name.Contains(searchText));

            return await access.CountAsync();
        }

        public async Task<int> AddAsync(AppSetting setting)
        {
            return await _context.AppSettings.InsertAsync(setting, new List<string> { "Id", "UpdatedAt", "DeletedAt" });
        }

        public async Task<int> UpdateAsync(AppSetting setting)
        {
            return await _context.AppSettings
                .Where(x => x.Id == setting.Id)
                .UpdateAsync(setting, s => new { s.Name, s.Value });
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _context.AppSettings.DeleteAsync(x => x.Id == id);
        }
    }
}
