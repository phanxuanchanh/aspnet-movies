using System;

namespace Data.DTO
{
    public class TagDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateTagDto {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateTagDto : CreateTagDto
    {
        public int ID { get; set; }
    }
}
