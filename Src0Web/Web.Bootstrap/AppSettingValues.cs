using Data.DAL;
using Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Web
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

        public static List<AppSetting> GetList()
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

        public static AppSetting Get(string name)
        {
            if (_appSettings.Count > 0)
                return _appSettings.Where(x => x.Name == name).SingleOrDefault();

            lock (_lock)
            {
                if (_appSettings.Count == 0)
                    LoadFromDb();

                return _appSettings.Where(x => x.Name == name).SingleOrDefault();
            }
        }

        public static Dictionary<string, Tval> GetAndParseValue<Tval>(string name)
        {
            AppSetting appSetting = Get(name);
            return JsonSerializer.Deserialize<Dictionary<string, Tval>>(appSetting.Value);
        }
    }
}