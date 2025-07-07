using System;
using System.Collections.Generic;

namespace Data.DTO
{
    public class FilmInfo
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string productionCompany { get; set; }
        public string thumbnail { get; set; }
        public string releaseDate { get; set; }
        public long upvote { get; set; }
        public long downvote { get; set; }
        public long views { get; set; }
        public string url { get; set; }
        public double scoreRating { get; set; }
        public int starRating { get; set; }
        public string duration { get; set; }
        public string source { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }

        public LanguageDto Language { get; set; }
        public CountryDto Country { get; set; }
        public List<CategoryInfo> Categories { get; set; }
        public List<TagInfo> Tags { get; set; }
        public List<DirectorInfo> Directors { get; set; }
        public List<ActorDto> Casts { get; set; }
    }

    public class FilmCreation
    {
        public string name { get; set; }
        public string description { get; set; }
        public string duration { get; set; }
        public int countryId { get; set; }
        public string productionCompany { get; set; }
        public string thumbnail { get; set; }
        public int languageId { get; set; }
        public string releaseDate { get; set; }
        public string source { get; set; }
    }

    public class FilmUpdate : FilmCreation
    {
        public string ID { get; set; }
    }
}
