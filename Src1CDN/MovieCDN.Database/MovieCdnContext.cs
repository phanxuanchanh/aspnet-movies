using Microsoft.EntityFrameworkCore;

namespace MovieCDN.Database;

public class MovieCdnContext : DbContext
{
    public MovieCdnContext(DbContextOptions<MovieCdnContext> options) 
        : base(options)
    {

    }

    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<File> Files { get; set; }
}