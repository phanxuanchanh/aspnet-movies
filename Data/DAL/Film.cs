using System;

namespace Data.DAL
{
    public class Film
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductionCompany { get; set; }
        public string Thumbnail { get; set; }
        public string ReleaseDate { get; set; }
        public long Upvote { get; set; }
        public long Downvote { get; set; }
        public long Views { get; set; }
        public string Duration { get; set; }
        public string Source { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
