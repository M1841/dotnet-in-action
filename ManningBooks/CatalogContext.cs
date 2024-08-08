using Microsoft.EntityFrameworkCore;

namespace ManningBooks;

public class CatalogContext : DbContext
{
  public DbSet<Book> Books { get; set; }

  public static void SeedBooks()
  {
    using var dbContext = new CatalogContext();
    if (!dbContext.Database.EnsureCreated()) { return; }

    dbContext.Add(new Book { Title = "Full-Stack Python Security" });
    dbContext.Add(new Book { Title = "C++ Concurrency in Action" });
    dbContext.Add(new Book { Title = "Get Programming with Go" });
    dbContext.Add(new Book { Title = "Code like a Pro in Rust" });

    var dotNETBook = new Book { Title = ".NET in Action" };
    dotNETBook.Ratings.Add(new Rating { Comment = "Great!", Book = dotNETBook });
    dotNETBook.Ratings.Add(new Rating { Stars = 4, Book = dotNETBook });

    dbContext.Add(dotNETBook);
    dbContext.SaveChanges();
  }

  public static async Task WriteBookToConsoleAsync(string title)
  {
    using var dbContext = new CatalogContext();
    var book = await dbContext.Books
      .Include(b => b.Ratings)
      .FirstOrDefaultAsync(b => b.Title == title);
    if (book == null)
    {
      Console.WriteLine(@$"""{title}"" not found");
    }
    else
    {
      Console.WriteLine($"#{book.Id} \"{book.Title}\"");
      book.Ratings.ForEach(rating =>
        Console.WriteLine($"  {rating.Stars} stars: {rating.Comment ?? "_no_comment_"}")
      );
    }
  }

  public const string ConnectionString =
    "DataSource=manningbooks;cache=shared";

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlite(ConnectionString);
}
