using ManningBooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

// using var keepAliveConnection =
//   new SqliteConnection(CatalogContext.ConnectionString);
// keepAliveConnection.Open();
CatalogContext.SeedBooks();

var userRequests = new[] {
  "Full-Stack Python Security",
  "C++ Concurrency in Action",
  "Get Programming with Go",
  "Code like a Pro in Rust",
  ".NET in Action"
};

var tasks = new List<Task>();
foreach (var userRequest in userRequests)
{
  tasks.Add(CatalogContext.WriteBookToConsoleAsync(userRequest));
}
Task.WaitAll([.. tasks]);
