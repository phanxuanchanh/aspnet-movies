using MSSQL;
using System;

namespace Data.DAL
{
    public class UserReaction : ISqlTable
    {
        public string filmId { get; set; }
        public string userId { get; set; }
        public bool upvoted { get; set; }
        public bool downvoted { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
