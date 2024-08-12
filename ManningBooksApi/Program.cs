using ManningBooksApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CatalogContext>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

CatalogContext.SeedBooks();

app.Run();
