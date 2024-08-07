using ManningBooks;

using var dbContext = new CatalogContext();
// dbContext.Add(new Book { Title = "Full-Stack Python Security" });
// dbContext.Add(new Book { Title = "C++ Concurrency in Action" });
// dbContext.Add(new Book { Title = ".NET in Action" });
// dbContext.Add(new Book { Title = "Get Programming with Go" });
// dbContext.Add(new Book { Title = "Code like a Pro in Rust" });
// dbContext.SaveChanges();
// foreach (var book in dbContext.Books.OrderBy(b => b.Id))
// {
// Console.WriteLine($"{book.Id}\t{book.Title}");
// }

var dotNETBook = new Book { Title = ".NET in Action" };
dotNETBook.Ratings.Add(new Rating { Comment = "Great!", Book = dotNETBook });
dotNETBook.Ratings.Add(new Rating { Stars = 4, Book = dotNETBook });

dbContext.Add(dotNETBook);
dbContext.SaveChanges();

var dotNETRatings = (
  from book
  in dbContext.Books
  where book.Title == ".NET in Action"
  select book.Ratings
).FirstOrDefault();

dotNETRatings?.ForEach(rating =>
  Console.WriteLine($"{rating.Stars} stars: {rating.Comment ?? "_no_comment_"}")
);