using Microsoft.EntityFrameworkCore;

namespace ManningBooksApi;

public class CatalogContext : DbContext
{
  public DbSet<Book> Books { get; set; }
  public DbSet<Rating> Ratings { get; set; }

  public static void SeedBooks()
  {
    using var dbContext = new CatalogContext();
    if (!dbContext.Database.EnsureCreated()) { return; }

    dbContext.Add(new Book("Full-Stack Python Security"));
    dbContext.Add(new Book("C++ Concurrency in Action"));
    dbContext.Add(new Book("Get Programming with Go"));
    dbContext.Add(new Book("Code like a Pro in Rust"));

    var dotNETBook = new Book(".NET in Action");
    dotNETBook.Ratings.Add(new Rating { Comment = "Great!" });
    dotNETBook.Ratings.Add(new Rating { Stars = 4 });

    dbContext.Add(dotNETBook);
    dbContext.SaveChanges();
  }

  public const string ConnectionString =
    "DataSource=manningbooks;cache=shared";

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlite(ConnectionString);
}
