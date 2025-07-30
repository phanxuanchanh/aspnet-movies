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
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public LanguageDto Language { get; set; }
        public CountryDto Country { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public List<TagDto> Tags { get; set; }
        public List<DirectorDto> Directors { get; set; }
        public List<ActorDto> Actors { get; set; }
    }

    public class CreateFilmDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string ProductionCompany { get; set; }
        public string Thumbnail { get; set; }
        public string ReleaseDate { get; set; }
        public string Source { get; set; }
    }

    public class UpdateFilmDto : CreateFilmDto
    {
        public string ID { get; set; }
    }
}
