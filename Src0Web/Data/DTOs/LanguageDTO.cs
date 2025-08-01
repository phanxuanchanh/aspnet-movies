using System;

namespace Data.DTO
{
    public class LanguageDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateLanguageDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateLanguageDto : CreateLanguageDto
    {
        public int ID { get; set; }
    }
}
