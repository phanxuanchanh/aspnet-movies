using System;

namespace Data.DAL
{
    public class FilmMetadata
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Custom { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}