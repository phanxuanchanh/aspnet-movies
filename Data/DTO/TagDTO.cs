using System;

namespace Data.DTO
{
    public class TagInfo
    {
        public long ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class TagCreation{
        public string name { get; set; }
        public string description { get; set; }
    }

    public class TagUpdate : TagCreation
    {
        public long ID { get; set; }
    }
}
