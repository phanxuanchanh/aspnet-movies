using System;

namespace Data.DTO
{
    public class CastInfo
    {
        public long ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class CastCreation
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class CastUpdate : CastCreation
    {
        public long ID { get; set; }
    }
}
