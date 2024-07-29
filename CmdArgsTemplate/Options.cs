using CommandLine;

namespace CmdArgsTemplate;

public record Options
{
  [Value(0, Required = true, HelpText = "Any word")]
  public string? Text { get; init; }
}