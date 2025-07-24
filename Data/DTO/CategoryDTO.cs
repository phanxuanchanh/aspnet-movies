using System;

namespace Data.DTO
{
    public class CategoryDto
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateCategoryDto : CreateCategoryDto
    {
        public int ID { get; set; }
    }
}
