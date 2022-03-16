using Data.DTO;
using System.Collections.Generic;

namespace Web.Models
{
    public class UserSession
    {
        public string userId { get; set; }
        public string username { get; set; }
        public string role { get; set; }
        public List<History> Histories { get; set; }
    }
}