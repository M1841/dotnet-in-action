namespace ManningBooksApi;

public class Book(
  string title,
  string? description = null
)
{
  public int Id { get; set; }
  public string Title { get; set; } = title;
  public string? Description { get; set; } = description;
  public List<Rating> Ratings { get; } = [];
  public List<Tag> Tags { get; } = [];
}
