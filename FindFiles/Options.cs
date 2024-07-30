using CommandLine;

namespace FindFiles;

public record Options
{
  [Value(0, Required = true, HelpText = "Root directory")]
  public string RootDir { get; init; } = ".";

  [Value(1, Required = false, HelpText = "File name query")]
  public string Query { get; init; } = "*";

  [Option('r', "recurse", HelpText = "Enable recursive sub-directory search")]
  public bool Recurse { get; init; } = false;
}
