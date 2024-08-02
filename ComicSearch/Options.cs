using CommandLine;

namespace ComicSearch;

public record Options
{
  [Value(0, Required = true, HelpText = "Comic title to search for")]
  public string? Title { get; init; }
}