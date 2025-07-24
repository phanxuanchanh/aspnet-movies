using Common.Hash;
using Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.BLL.Migration
{
    internal class RoleMigration
    {
        private static List<Role> AddData()
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

            return roles;
        }

        public static void Migrate()
        {
            using (DBContext db = new DBContext())
            {
                long recordNumber = db.Roles.Count();
                if (recordNumber == 0)
                {
                    List<Role> roles = AddData();

                    foreach (Role role in roles)
                    {
                        int affected = db.Roles.Insert(role);
                        if (affected == 0)
                            break;
                    }
                }
            }
        }

        public static async Task MigrateAsync()
        {
            using (DBContext db = new DBContext())
            {
                long recordNumber = await db.Roles.CountAsync();
                if (recordNumber == 0)
                {
                    List<Role> roles = AddData();

                    foreach (Role role in roles)
                    {
                        int affected = await db.Roles.InsertAsync(role);
                        if (affected == 0)
                            break;
                    }
                }
            }
        }
    }
}
