using System;

namespace Data.DAL
{
    internal class Film
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int countryId { get; set; }
        public string productionCompany { get; set; }
        public string thumbnail { get; set; }
        public int languageId { get; set; }
        public string releaseDate { get; set; }
        public long upvote { get; set; }
        public long downvote { get; set; }
        public long views { get; set; }
        public string duration { get; set; }
        public string source { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
