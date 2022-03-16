using System;

namespace Data.DTO
{
    public class DirectorInfo
    {
        public long ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class DirectorCreation
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class DirectorUpdate : DirectorCreation
    {
        public long ID { get; set; }
    }
}
