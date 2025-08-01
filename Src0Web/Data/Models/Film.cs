using MSSQL;
using MSSQL.Attributes;

namespace Data.DAL
{
    [SqlTable("Film")]
    public class Film : SqlTableWithTimestamp
    {
        [SqlColumn("id", PrimaryKey = true)]
        public string ID { get; set; }

        [SqlColumn("name")]
        public string Name { get; set; }

        [SqlColumn("description")]
        public string Description { get; set; }

        [SqlColumn("productionCompany")]
        public string ProductionCompany { get; set; }

        [SqlColumn("thumbnail")]
        public string Thumbnail { get; set; }

        [SqlColumn("releaseDate")]
        public string ReleaseDate { get; set; }

        [SqlColumn("upvote")]
        public long Upvote { get; set; }

        [SqlColumn("downvote")]
        public long Downvote { get; set; }

        [SqlColumn("views")]
        public long Views { get; set; }

        [SqlColumn("duration")]
        public string Duration { get; set; }

        [SqlColumn("source")]
        public string Source { get; set; }
    }
}
