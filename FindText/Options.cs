using CommandLine;

namespace FindText;

public record Options
{
  [Value(0, Required = true, HelpText = "Text to search for")]
  public string? Query { get; init; }

  [Value(1, Required = false, HelpText = "File to search in")]
  public string? File { get; init; }

  [Option('x', "regex", HelpText = "Search using regular-expressions")]
  public bool Regex { get; init; }
}
