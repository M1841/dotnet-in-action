using ComicSearch;
using CommandLine;

static Comic? GetComicWithTitle(string title)
{
  var lastComic = Comic.GetComic(0);
  for (int num = lastComic!.Number; num > 0; --num)
  {
    var comic = Comic.GetComic(num);
    if (
      comic != null
      && string.Equals(
        title,
        comic!.Title,
        StringComparison.OrdinalIgnoreCase
      )
    ) { return comic; }
  }
  return null;
}

static async Task<Comic?> GetComicWithTitleAsync(string title)
{
  var cancellationToken = new CancellationTokenSource();
  var lastComic = await Comic.GetComicAsync(
    0,
    cancellationToken.Token
  );

  var tasks = new List<Task>();
  Comic? foundComic = null;

  for (int num = lastComic!.Number; num > 0; --num)
  {
    var numCopy = num;
    var getComicTask = Comic.GetComicAsync(
      numCopy,
      cancellationToken.Token
    );
    var continuationTask = getComicTask.ContinueWith(
      task =>
      {
        try
        {
          var comic = task.Result;
          if (
            comic != null
            && string.Equals(
              title,
              comic!.Title,
              StringComparison.OrdinalIgnoreCase
            )
          )
          {
            cancellationToken.Cancel();
            foundComic = comic;
          }
        }
        catch (TaskCanceledException) { }
      }
    );
    tasks.Add(continuationTask);
  }
  await Task.WhenAll(tasks);
  return foundComic;
}

var results = await Parser.Default
  .ParseArguments<Options>(args)
  .WithParsedAsync(async options =>
  {
    var stopwatch = new System.Diagnostics.Stopwatch();

    stopwatch.Start();
    var comic = await GetComicWithTitleAsync(options.Title!);
    stopwatch.Stop();

    if (comic != null)
    {
      Console.WriteLine(
        $"xkcd \"{options.Title}\" is comic #{comic.Number}, published on {comic.Date}"
      );
    }
    else
    {
      Console.WriteLine(
        $"xkcd comic with title \"{options.Title}\" not found"
      );
    }

    Console.WriteLine(
      $"Result returned in {stopwatch.ElapsedMilliseconds} ms"
    );
  });

// results.WithNotParsed(_ =>
//     Console.WriteLine(CommandLine.Text.HelpText.RenderUsageText(results))
//   );
