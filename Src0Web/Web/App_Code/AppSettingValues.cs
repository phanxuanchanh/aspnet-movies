using Data.DAL;
using Data.Models;
using System.Collections.Generic;

namespace Web.App_Code
{
    public class AppSettingValues
    {
        private static readonly object _lock = new object();
        private static List<AppSetting> _appSettings = new List<AppSetting>();

        private static void LoadFromDb()
        {
            using (DBContext context = new DBContext())
            {
                _appSettings.Clear();
                _appSettings.AddRange(context.AppSettings.ToList());
            }
        }

        public static List<AppSetting> Get()
        {
            if (_appSettings.Count > 0)
                return _appSettings;

            lock (_lock)
            {
                if (_appSettings.Count == 0)
                    LoadFromDb();

                return _appSettings;
            }
        }
    }
}