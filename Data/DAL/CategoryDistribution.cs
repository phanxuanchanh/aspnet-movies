using System;

namespace Data.DAL
{
    internal class CategoryDistribution
    {
        public int categoryId { get; set; }
        public string filmId { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
