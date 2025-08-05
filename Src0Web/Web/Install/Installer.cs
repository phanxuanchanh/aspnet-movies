using Data.DAL;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Threading;
using Common.Hash;
using System.Linq;
using System.Text;

namespace Web.Install
{
    public class Installer
    {
        private static bool _isAppSettingsMissing = true;
        private static readonly object _lock = new object();

        public static bool IsAppSettingsMissing
        {
            get
            {
                if (!_isAppSettingsMissing)
                    return _isAppSettingsMissing; // nhanh, không cần lock nếu đã true

                //Nếu chưa biết(false) thì mới lock lại để kiểm tra chắc chắn
                //Giả sử nhiều thread vào cùng lúc, chỉ 1 thread sẽ chạy phần này và gọi DB
                lock (_lock)
                {
                    if (!_isAppSettingsMissing)
                        return _isAppSettingsMissing; // Cần kiểm tra lại sau khi đã vào lock

                    _isAppSettingsMissing = !HasAppSettings();
                    return _isAppSettingsMissing;
                }
            }
        }

        public static void RunIfNotInstalled()
        {
            if (!HasAnyTable())
            {
                CreateDatabase();
                Thread.Sleep(500);
                MigrateRole();
            }
        }

        private static bool HasAnyTable()
        {
            using (DBContext context = new DBContext())
            {
                string query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                int tableNumbers = context.GetHelper().ExecuteScalarQuery<int>(query);

                return tableNumbers > 0;
            }
        }

        private static bool HasAppSettings()
        {
            using (DBContext db = new DBContext())
            {
                long count1 = db.AppSettings.Count(x => x.Name == "cdn-server");
                long count2 = db.Users.Count();

                return count1 != 0 && count2 != 0;
            }
        }


        private static void CreateDatabase()
        {
            byte[] rawSql = InstallationResource.movie;
            string sql = Encoding.UTF8.GetString(rawSql);
            IEnumerable<string> commandStrings = Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            using (DBContext db = new DBContext())
            {
                foreach (string commandString in commandStrings)
                {
                    if (!string.IsNullOrWhiteSpace(commandString.Trim()))
                    {
                        db.GetHelper().ExecuteNonQuery(commandString);
                    }
                }
            }
        }

        private static void MigrateRole()
        {
            List<string> IDs = new List<string>();
            int count = 0;
            while (count < 3)
            {
                Random random = new Random();
                string id = HashFunction.MD5_Hash(random.NextString(10));
                if (IDs.Any(i => i.Equals(id)) == false)
                {
                    IDs.Add(id);
                    count++;
                }
                random = null;
            }

            List<Role> roles = new List<Role>();
            roles.Add(new Role { Id = IDs[0], Name = "Admin", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            roles.Add(new Role { Id = IDs[1], Name = "Editor", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            roles.Add(new Role { Id = IDs[2], Name = "User", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });

            using (DBContext db = new DBContext())
            {
                long recordNumber = db.Roles.Count();
                if (recordNumber == 0)
                {
                    foreach (Role role in roles)
                    {
                        int affected = db.Roles.Insert(role);
                        if (affected == 0)
                            break;
                    }
                }
            }
        }
    }
}
