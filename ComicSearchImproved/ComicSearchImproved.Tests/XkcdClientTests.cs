using System.Net;
using FakeItEasy;

namespace ComicSearchImproved.Tests;

public class XkcdClientTests
{
  [Fact]
  public async Task GetLatest()
  {
    SetResponse(HttpStatusCode.OK, LatestJson);
    var comic = await xkcdClient.GetLatestAsync();
    Assert.Equal(2975, comic.Number);
  }

  [Fact]
  public async Task NoComicFound()
  {
    SetResponse(HttpStatusCode.NotFound);
    var comic = await xkcdClient.GetByNumberAsync(1);
    Assert.Null(comic);
  }

  [Fact]
  public async Task GetByNumber()
  {
    SetResponse(HttpStatusCode.OK, LatestJson);
    var comic = await xkcdClient.GetByNumberAsync(2975);
    Assert.NotNull(comic);
    Assert.Equal(2975, comic.Number);
  }

  public XkcdClientTests()
  {
    _fakeMsgHandler = A.Fake<HttpMessageHandler>();
    var httpClient = SetupHttpClient(_fakeMsgHandler);
    xkcdClient = new XkcdClient(httpClient);
  }
  internal static HttpClient SetupHttpClient(HttpMessageHandler msgHandler) =>
    new(msgHandler)
    {
      BaseAddress = new Uri("https://xkcd.com")
    };
  private void SetResponse(
    HttpStatusCode statusCode,
    string content = "")
  {
    A.CallTo(_fakeMsgHandler)
      .WithReturnType<Task<HttpResponseMessage>>()
      .Where(c => c.Method.Name == "SendAsync")
      .Returns(new HttpResponseMessage()
      {
        StatusCode = statusCode,
        Content = new StringContent(content)
      });
  }
  private readonly XkcdClient xkcdClient;
  private readonly HttpMessageHandler _fakeMsgHandler;
  private const string LatestJson =
@"{
  ""month"": ""8"",
  ""num"": 2975,
  ""link"": """",
  ""year"": ""2024"",
  ""news"": """",
  ""safe_title"": ""Classical Periodic Table"",
  ""transcript"": """",
  ""alt"": ""Personally I think mercury is more of a 'wet earth' hybrid element."",
  ""img"": ""https://imgs.xkcd.com/comics/classical_periodic_table.png"",
  ""title"": ""Classical Periodic Table"",
  ""day"": ""21""
}";
}
