using System.Text.Json.Serialization;

namespace ManningBooksApi;

public class Rating
{
  public int Id { get; set; }
  public int Stars { get; set; } = 5;
  public string? Comment { get; set; }
  [JsonIgnore]
  public Book? Book { get; set; }
  public int BookId { get; set; }
}
