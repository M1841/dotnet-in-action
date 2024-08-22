using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ComicSearchImproved.Tests;

public class ComicRepositoryTests : IDisposable
{
  [Fact]
  public async Task NoComics_GetLatest()
  {
    var latest = await _comicRepo.GetLatestNumberAsync();
    Assert.Equal(0, latest);
  }

  [Fact]
  public async Task Comics_GetLatest()
  {
    _comicDbContext.AddRange(
      new Comic() { Number = 1 },
      new Comic() { Number = 12 },
      new Comic() { Number = 4 });
    await _comicDbContext.SaveChangesAsync();

    var latest = await _comicRepo.GetLatestNumberAsync();
    Assert.Equal(12, latest);
  }

  [Fact]
  public async Task Add()
  {
    await _comicRepo.AddComicAsync(
      new Comic() { Number = 3 });

    var addedComic = _comicDbContext.Find<Comic>(3);
    Assert.NotNull(addedComic);
  }

  [Fact]
  public async Task Found()
  {
    _comicDbContext.AddRange(
      new Comic() { Number = 1, Title = "a" },
      new Comic() { Number = 12, Title = "b" },
      new Comic() { Number = 4, Title = "c" });
    await _comicDbContext.SaveChangesAsync();

    var foundComics = _comicRepo
      .Find("b")
      .ToBlockingEnumerable();
    Assert.Single(foundComics);
    Assert.Single(foundComics,
      c => c.Number == 12);
  }

  public ComicRepositoryTests()
  {
    (_comicDbContext, _keepAliveConn) = SetupSqlite("comics");
    _comicRepo = new ComicRepository(_comicDbContext);
  }
  internal static (ComicDbContext, SqliteConnection) SetupSqlite(string dbName)
  {
    var connStr = string.Format(ConnStrTemplate, dbName);
    var keepAlive = new SqliteConnection(connStr);
    keepAlive.Open();

    var optionsBuilder = new DbContextOptionsBuilder();
    optionsBuilder.UseSqlite(connStr);
    var options = optionsBuilder.Options;
    var dbContext = new ComicDbContext(options);

    Assert.True(dbContext.Database.EnsureCreated());
    return (dbContext, keepAlive);
  }
  internal const string ConnStrTemplate =
    "DataSource={0};mode=memory;cache=shared";
  private readonly ComicDbContext _comicDbContext;
  private readonly ComicRepository _comicRepo;
  private readonly SqliteConnection _keepAliveConn;
  private bool _disposed;
  private void Dispose(bool disposing)
  {
    if (!_disposed)
    {
      if (disposing)
      {
        _keepAliveConn.Close();
        _comicDbContext.Dispose();
      }
      _disposed = true;
    }
  }
  ~ComicRepositoryTests() => Dispose(false);
  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
}
