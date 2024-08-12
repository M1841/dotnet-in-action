using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManningBooksApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RatingController(CatalogContext dbContext) : ControllerBase
{
  [HttpGet("{id}")]
  public Task<Rating?> GetOne(int id)
  {
    return _dbContext.Ratings
      .FirstOrDefaultAsync(r => r.Id == id);
  }

  [HttpGet]
  public IAsyncEnumerable<Rating> GetMany(
    int? limit,
    int? offset,
    int? bookId,
    string? order = null
  )
  {
    IQueryable<Rating> query = _dbContext.Ratings.AsNoTracking();

    if (bookId.HasValue)
    {
      query = query.Where(r => r.BookId == bookId);
    }
    else { query = query.OrderBy(r => r.BookId); }
    if ("ascending".Contains(
      order ?? "desc",
      StringComparison.OrdinalIgnoreCase
    )) { query = query.OrderBy(r => r.Stars); }

    return query
      .Skip(offset ?? 0)
      .Take(limit.HasValue ? Math.Min(limit.Value, 10) : 10)
      .AsAsyncEnumerable();
  }

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status201Created)]
  public async Task<IActionResult> CreateAsync(
    RatingCreateCommand command,
    CancellationToken cancellationToken)
  {
    var book = await _dbContext.FindAsync<Book>(
        [command.BookId],
        cancellationToken);
    if (book == null) { return NotFound(); }

    var rating = new Rating
    {
      Stars = command.Stars,
      BookId = command.BookId
    };
    if (command.Comment != null) { rating.Comment = command.Comment; }
    _dbContext.Ratings.Add(rating);
    await _dbContext.SaveChangesAsync(cancellationToken);

    return CreatedAtAction(nameof(GetOne), new { id = rating.Id }, rating);
  }

  [HttpPatch("{id}")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> UpdateAsync(
    int id,
    RatingUpdateCommand command,
    CancellationToken cancellationToken)
  {
    var rating = await _dbContext.FindAsync<Rating>(
      [id],
      cancellationToken);

    if (rating == null) { return NotFound(); }
    if (command.Stars.HasValue) { rating.Stars = command.Stars.Value; }
    if (command.Comment != null) { rating.Comment = command.Comment; }

    await _dbContext.SaveChangesAsync(cancellationToken);
    return Ok(rating);
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> DeleteAsync(
    int id,
    CancellationToken cancellationToken)
  {
    var rating = await _dbContext.FindAsync<Rating>(
        [id],
        cancellationToken);

    if (rating == null) { return NotFound(); }
    _dbContext.Remove(rating);

    await _dbContext.SaveChangesAsync(cancellationToken);
    return NoContent();
  }

  public record RatingCreateCommand(
    int Stars, string? Comment, int BookId)
  { }
  public record RatingUpdateCommand(
    int? Stars, string? Comment)
  { }

  private readonly CatalogContext _dbContext = dbContext;
}
