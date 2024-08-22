using Microsoft.EntityFrameworkCore;

namespace ComicSearchImproved;

public class ComicDbContext(DbContextOptions options) : DbContext(options)
{
  public DbSet<Comic> Comics { get; set; } = null!;
}
