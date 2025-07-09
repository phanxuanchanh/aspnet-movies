using System;
using System.Collections.Generic;

namespace Data.DTO
{
    public class FilmDto
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
        public string Url { get; set; }
        public double ScoreRating { get; set; }
        public int StarRating { get; set; }
        public string Duration { get; set; }
        public string Source { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public LanguageDto Language { get; set; }
        public CountryDto Country { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public List<TagDto> Tags { get; set; }
        public List<DirectorDto> Directors { get; set; }
        public List<ActorDto> Actors { get; set; }
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
