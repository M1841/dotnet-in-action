namespace ComicSearchImproved;

public class ComicFinder(IXkcdClient xkcdClient, IComicRepository repo)
{
  public async Task<IAsyncEnumerable<Comic>> FindAsync(string query)
  {
    var latestComic = await _xkcdClient.GetLatestAsync();
    int latestInRepo = await _repo.GetLatestNumberAsync();

    if (latestComic.Number > latestInRepo)
    {
      await FetchAsync(latestComic, latestInRepo);
    }
    return _repo.Find(query);
  }

  private async Task FetchAsync(Comic latestComic, int latestInRepo)
  {
    var getTasks = new List<Task<Comic?>>();
    var addTasks = new List<Task>();

    int newComicsCount = latestComic.Number - latestInRepo;
    Enumerable.Range(latestInRepo + 1, newComicsCount)
      .Reverse().ToList()
      .ForEach(i =>
        getTasks.Add(_xkcdClient.GetByNumberAsync(i)));

    (await Task.WhenAll(getTasks))
      .Where(c => c != null)
      .ToList()
      .ForEach(c => addTasks.Add(_repo.AddComicAsync(c!)));

    await Task.WhenAll(addTasks);
  }

  private readonly IXkcdClient _xkcdClient = xkcdClient;
  private readonly IComicRepository _repo = repo;
}
