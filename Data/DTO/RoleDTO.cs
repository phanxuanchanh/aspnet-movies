using System;

namespace Data.DTO
{
    public class RoleInfo
    {
        public string ID { get; set; }
        public string name { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class RoleCreation
    {
        public string ID { get; set; }
        public string name { get; set; }
    }

    public class RoleUpdate: RoleCreation
    {

    }
}
