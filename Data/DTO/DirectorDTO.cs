using System;

namespace Data.DTO
{
    public class DirectorDto
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateDirectorDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateDirectorDto : CreateDirectorDto
    {
        public long ID { get; set; }
    }
}
