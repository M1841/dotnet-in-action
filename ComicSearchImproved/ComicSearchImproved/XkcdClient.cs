using System.Text.Json;

namespace ComicSearchImproved;

public class XkcdClient(HttpClient httpClient) : IXkcdClient
{
  public async Task<Comic> GetLatestAsync()
  {
    var stream = await _httpClient.GetStreamAsync(PageUri);
    return JsonSerializer.Deserialize<Comic>(stream)!;
  }
  public async Task<Comic?> GetByNumberAsync(int number)
  {
    try
    {
      var path = $"{number}/{PageUri}";
      var stream = await _httpClient.GetStreamAsync(path);
      return JsonSerializer.Deserialize<Comic>(stream);
    }
    catch (AggregateException ex)
      when (ex.InnerException is HttpRequestException)
    {
      return null;
    }
    catch (HttpRequestException) { return null; }
  }
  private const string PageUri = "info.0.json";
  private readonly HttpClient _httpClient = httpClient;
}
