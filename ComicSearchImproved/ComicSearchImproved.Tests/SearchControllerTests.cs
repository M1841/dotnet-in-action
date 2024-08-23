using System.Net;
using System.Text.Json;
using ComicSearchImprovedService;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ComicSearchImproved.Tests;

public class SearchControllerTests
{
  [Fact]
  public async Task FoundB()
  {
    ComicFinderTests.SetResponseComics(
      _fakeMsgHandler,
      new Comic() { Number = 12, Title = "b" },
      new Comic() { Number = 1, Title = "a" },
      new Comic() { Number = 4, Title = "c" });

    HttpClient client = _factory.CreateClient();
    var response = await client
      .GetAsync("/search?query=b");

    Assert.Equal(HttpStatusCode.OK,
      response.StatusCode);
    string content = await response.Content.ReadAsStringAsync();
    var comics = JsonSerializer
      .Deserialize<Comic[]>(content);

    Assert.NotNull(comics);
    Assert.Single(comics);
    Assert.Single(comics,
      c => c.Number == 12);
  }

  public SearchControllerTests()
  {
    _fakeMsgHandler = A.Fake<HttpMessageHandler>();

    _factory = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(builder =>
      {
        builder.UseEnvironment("Development");
        builder.ConfigureServices(services =>
        {
          var sd = services.First(
            s => s.ServiceType == typeof(HttpClient));
          services.Remove(sd);
          services.AddHttpClient<IXkcdClient, XkcdClient>(
            h =>
            h.BaseAddress = new Uri(BaseAddress))
            .ConfigurePrimaryHttpMessageHandler(
              () => _fakeMsgHandler);
        });
      });
  }

  private const string BaseAddress = "https://xkcd.com";
  private readonly WebApplicationFactory<Program> _factory;
  private readonly HttpMessageHandler _fakeMsgHandler;
}
