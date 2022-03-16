using System;

namespace Data.DAL
{
    internal class DirectorOfFilm
    {
        public string filmId { get; set; }
        public long directorId { get; set; }
        public string role { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
