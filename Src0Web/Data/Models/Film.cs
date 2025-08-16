using Data.Base;
using MSSQL;
using MSSQL.Attributes;

namespace Data.DAL
{
    [SqlTable("Films")]
    public class Film : SqlTableWithTimestamp
    {
        [SqlColumn("Id", PrimaryKey = true)]
        public string Id { get; set; }

        [SqlColumn("Name")]
        public string Name { get; set; }

        [SqlColumn("Description")]
        public string Description { get; set; }

        [SqlColumn("ProductionCompany")]
        public string ProductionCompany { get; set; }

        [SqlColumn("Thumbnail")]
        public string Thumbnail { get; set; }

        [SqlColumn("ReleaseDate")]
        public string ReleaseDate { get; set; }

        [SqlColumn("Upvote")]
        public long Upvote { get; set; }

        [SqlColumn("Downvote")]
        public long Downvote { get; set; }

        [SqlColumn("Views")]
        public long Views { get; set; }

        [SqlColumn("Duration")]
        public string Duration { get; set; }

        [SqlColumn("Source")]
        public string Source { get; set; }
    }
}
