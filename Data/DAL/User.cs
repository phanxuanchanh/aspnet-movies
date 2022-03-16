using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAL
{
    internal class User
    {
        public string ID { get; set; }
        public string userName { get; set; }
        public string surName { get; set; }
        public string middleName { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public string description { get; set; }
        public bool activated { get; set; }
        public string roleId { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
