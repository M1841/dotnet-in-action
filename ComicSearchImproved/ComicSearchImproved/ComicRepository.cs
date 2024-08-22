using Microsoft.EntityFrameworkCore;

namespace ComicSearchImproved;

public class ComicRepository(ComicDbContext dbContext) : IComicRepository
{
  public Task<int> GetLatestNumberAsync() =>
    _dbContext.Comics
      .OrderByDescending(c => c.Number)
      .Select(c => c.Number)
      .FirstOrDefaultAsync();
  public Task AddComicAsync(Comic comic)
  {
    _dbContext.Add(comic);
    return _dbContext.SaveChangesAsync();
  }
  public IAsyncEnumerable<Comic> Find(string query) =>
    _dbContext.Comics
      .Where(c =>
        (c.Title != null && c.Title.Contains(query))
        || (c.AltText != null && c.AltText.Contains(query)))
      .AsAsyncEnumerable();
  private readonly ComicDbContext _dbContext = dbContext;
}
