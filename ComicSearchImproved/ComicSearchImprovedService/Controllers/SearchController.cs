using Microsoft.AspNetCore.Mvc;
using ComicSearchImproved;

namespace ComicSearchImprovedService.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController(ComicFinder comicFinder) : ControllerBase
{
  [HttpGet]
  public Task<IAsyncEnumerable<Comic>> FindAsync(string query) =>
    _comicFinder.FindAsync(query);

  private readonly ComicFinder _comicFinder = comicFinder;
}
