using ComicSearchImproved;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ComicSearchImprovedService;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    var services = builder.Services;
    var cfg = builder.Configuration;

    var connStr = cfg.GetConnectionString("Sqlite");
    var baseAddr = cfg.GetValue<string>("BaseAddr");

    services.AddScoped<IComicRepository, ComicRepository>();
    services.AddScoped<IXkcdClient, XkcdClient>();
    services.AddScoped<ComicFinder>();
    services.AddControllers();

    services.AddDbContext<ComicDbContext>(
      option =>
        option.UseSqlite(connStr));

    services.AddHttpClient<IXkcdClient, XkcdClient>(
      client =>
        client.BaseAddress = new Uri(baseAddr!));

    var app = builder.Build();
    app.MapControllers();

    using var keepAliveConn = new SqliteConnection(connStr);
    keepAliveConn.Open();

    using (var scope = app.Services.CreateScope())
    {
      var dbContext = scope.ServiceProvider
        .GetRequiredService<ComicDbContext>();
      dbContext.Database.EnsureCreated();
    }

    app.Run();
  }
}
