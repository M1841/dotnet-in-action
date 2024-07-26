using CommandLine;

namespace HelloDotNet;

public record Options
{
  [Value(0, Required = true)]
  public IEnumerable<string>? Text { get; init; }

  [Option('f', "font")]
  public string? Font { get; init; }

  [Option('o', "--odds")]
  public bool ShowOdds { get; init; }

  [Option('e', "--evens")]
  public bool ShowEvens { get; init; }
}
