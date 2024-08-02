using System.Text.Json;
using System.Text.Json.Serialization;

namespace ComicSearch;

public record Comic
{
  public static Comic? GetComic(int number)
  {
    try
    {
      var path = number == 0
        ? "info.0.json"
        : $"{number}/info.0.json";
      var stream = client
        .GetStreamAsync(path)
        .Result;
      return JsonSerializer
        .Deserialize<Comic>(stream);
    }
    catch (Exception ex) when (
      (ex is AggregateException
        && ex.InnerException is HttpRequestException
      )
      || ex is HttpRequestException
    )
    { return null; }
  }

  public static async Task<Comic?> GetComicAsync(
    int number,
    CancellationToken cancellationToken
  )
  {
    if (cancellationToken.IsCancellationRequested) { return null; }
    try
    {
      var path = number == 0
        ? "info.0.json"
        : $"{number}/info.0.json";
      var stream = await client
        .GetStreamAsync(
          path,
          cancellationToken
        );
      return await JsonSerializer
        .DeserializeAsync<Comic>(
          stream,
          cancellationToken: cancellationToken
        );
    }
    catch (Exception ex) when (
      (ex is AggregateException
        && ex.InnerException is HttpRequestException
      )
      || ex is HttpRequestException
      || ex is TaskCanceledException
    )
    {
      return null;
    }
  }

  private static readonly HttpClient client = new()
  { BaseAddress = new Uri("https://xkcd.com") };

  [JsonPropertyName("num")]
  public int Number { get; init; }

  [JsonPropertyName("safe_title")]
  public string? Title { get; init; }

  [JsonPropertyName("year")]
  public string? Year { get; init; }

  [JsonPropertyName("month")]
  public string? Month { get; init; }

  [JsonPropertyName("day")]
  public string? Day { get; init; }

  [JsonIgnore]
  public DateOnly Date => DateOnly.Parse(
    $"{Year}-{Month}-{Day}"
  );
}
