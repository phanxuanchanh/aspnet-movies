using System;

namespace Data.DTO
{
    public class CategoryInfo
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CategoryCreation
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CategoryUpdate : CategoryCreation
    {
        public int ID { get; set; }
    }
}
