using Data.DAL;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Threading;
using System.Web;
using System.IO;
using Common.Hash;
using System.Linq;
using System.Text;

namespace Web.Install
{
    public class Installer
    {
        public static void RunIfNotInstalled(HttpContext httpContext)
        {
            if (HasAnyTable())
                return;

            CreateDatabase();
            Thread.Sleep(500);
            MigrateRole();
            MigrateUser();

            string currentUrl = httpContext.Request.Url.AbsolutePath;

            if (!currentUrl.EndsWith("/Install/AppSettings.aspx"))
            {
                httpContext.Response.Redirect("~/Install/AppSettings.aspx", false);
                httpContext.ApplicationInstance.CompleteRequest();
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
                HashFunction hash = new HashFunction();
                string id = hash.MD5_Hash(random.NextString(10));
                if (IDs.Any(i => i.Equals(id)) == false)
                {
                    IDs.Add(id);
                    count++;
                }
                random = null;
                hash = null;
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

        private static void MigrateUser()
        {
            HashFunction hash = new HashFunction();
            string salt = hash.MD5_Hash(new Random().NextString(25));

            Data.DAL.User user = new Data.DAL.User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "systemadmin",
                SurName = "System",
                MiddleName = "",
                Name = "Admin",
                Description = "Tài khoản quản trị cấp cao",
                Email = "systemadmin@admin.com",
                PhoneNumber = "00000000",
                Password = hash.PBKDF2_Hash("admin12341234", salt, 30),
                Salt = salt,
                Activated = true,
                CreatedAt = DateTime.Now
            };

            using (DBContext db = new DBContext())
            {
                long recordNumber = db.Users.Count();
                if (recordNumber == 0)
                {
                    Role role = db.Roles.Select(s => new { s.Id }).SingleOrDefault(x => x.Name == "Admin");
                    user.RoleId = role.Id;

                    int affected = db.Users.Insert(user);
                }
            }
        }
    }
}
