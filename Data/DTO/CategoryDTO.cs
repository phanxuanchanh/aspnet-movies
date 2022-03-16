using System;

namespace Data.DTO
{
    public class CategoryInfo
    {
        public int ID { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class CategoryCreation
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class CategoryUpdate : CategoryCreation
    {
        public int ID { get; set; }
    }
}
