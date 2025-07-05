using System;

namespace Data.DTO
{
    public class ActorDto
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateActorDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateActorDto : CreateActorDto
    {
        public long ID { get; set; }
    }
}
