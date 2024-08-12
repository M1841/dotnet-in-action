using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManningBooksApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController(CatalogContext dbContext) : ControllerBase
{
  [HttpGet("{id}")]
  public Task<Book?> GetOne(int id)
  {
    return _dbContext.Books
      .Include(b => b.Ratings)
      .FirstOrDefaultAsync(b => b.Id == id);
  }

  [HttpGet]
  public IAsyncEnumerable<Book> GetMany(
    int? limit,
    int? offset,
    string? title = null,
    string? order = null)
  {
    IQueryable<Book> query = _dbContext.Books
      .Include(b => b.Ratings)
      .AsNoTracking();

    if (title != null)
    {
      query = query.Where(b =>
        b.Title.ToLower().Contains(title.ToLower()));
    }
    if ("descending".Contains(
      order ?? "asc",
      StringComparison.OrdinalIgnoreCase
    )) { query = query.OrderByDescending(b => b.Title); }

    return query
      .Skip(offset ?? 0)
      .Take(limit.HasValue ? Math.Min(limit.Value, 10) : 10)
      .AsAsyncEnumerable();
  }

  [HttpPost]
  public async Task<Book> CreateAsync(
    BookCreateCommand command,
    CancellationToken cancellationToken)
  {
    var book = new Book(
      command.Title,
      command.Description
    );

    var entity = _dbContext.Books.Add(book);

    await _dbContext.SaveChangesAsync(cancellationToken);
    return entity.Entity;
  }

  [HttpPatch("{id}")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<IActionResult> UpdateAsync(
    int id,
    BookUpdateCommand command,
    CancellationToken cancellationToken)
  {
    var book = await _dbContext.FindAsync<Book>(
      [id],
      cancellationToken);

    if (book == null) { return NotFound(); }
    if (command.Title != null) { book.Title = command.Title; }
    if (command.Description != null) { book.Description = command.Description; }

    await _dbContext.SaveChangesAsync(cancellationToken);
    return Ok(book);
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> DeleteAsync(
    int id,
    CancellationToken cancellationToken)
  {
    var book = await _dbContext.Books
      .Include(b => b.Ratings)
      .FirstOrDefaultAsync(
        b => b.Id == id,
        cancellationToken
      );

    if (book == null) { return NotFound(); }
    _dbContext.Remove(book);

    await _dbContext.SaveChangesAsync(cancellationToken);
    return NoContent();
  }

  public record BookCreateCommand(
    string Title, string? Description)
  { }
  public record BookUpdateCommand(
    string? Title, string? Description)
  { }

  private readonly CatalogContext _dbContext = dbContext;
}
