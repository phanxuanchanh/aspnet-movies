using System;

namespace Data.DAL
{
    internal class CastOfFilm
    {
        public string filmId { get; set; }
        public long castId { get; set; }
        public string role { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
