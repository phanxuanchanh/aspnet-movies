using Common.Hash;
using Data.DAL;
using System;
using System.Threading.Tasks;

namespace Data.BLL.Migration
{
    internal class UserMigration
    {
        public static User AddData()
        {
            HashFunction hash = new HashFunction();
            string salt = hash.MD5_Hash(new Random().NextString(25));

            return new User
            {
                //ID = Guid.NewGuid().ToString(),
                //userName = "systemadmin",
                //surName = "System",
                //middleName = "",
                //name = "Admin",
                //description = "Tài khoản quản trị cấp cao",
                //email = "systemadmin@admin.com",
                //phoneNumber = "00000000",
                //password = hash.PBKDF2_Hash("admin12341234", salt, 30),
                //salt = salt,
                //activated = true,
                //createAt = DateTime.Now,
                //updateAt = DateTime.Now
            };
        }

        public static void Migrate()
        {
            using(DBContext db = new DBContext())
            {
                long recordNumber = db.Users.Count();
                if(recordNumber == 0)
                {
                    User user = AddData();
                    Role role = db.Roles.Select(s => new { s.Id }).SingleOrDefault(x => x.Name == "Admin");
                    user.RoleId = role.Id;

                    int affected = db.Users.Insert(user);
                }
            }
        }

        public static async Task MigrateAsync()
        {
            using (DBContext db = new DBContext())
            {
                long recordNumber = await db.Users.CountAsync();
                if (recordNumber == 0)
                {
                    User user = AddData();
                    Role role = db.Roles.Select(s => new { s.Id }).SingleOrDefault(x => x.Name == "Admin");
                    user.RoleId = role.Id;

                    int affected = await db.Users.InsertAsync(user);

                }
            }
        }
    }
}
