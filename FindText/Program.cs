using FindText;
using CommandLine;
using System.Text.RegularExpressions;

static void SearchFile(
  FileInfo file,
  string query,
  bool regex
)
{
  using var reader = file.OpenText();

  Func<string, string, bool> comparison = (string line, string query) =>
  {
    return regex
      ? line.Contains(query, StringComparison.OrdinalIgnoreCase)
      : Regex.Match(line, query).Success;
  };

  string? line;

  while ((line = reader.ReadLine()) != null)
  {
    if (line.Contains(query, StringComparison.OrdinalIgnoreCase))
    {
      Console.WriteLine(line);
    }
  }
}

var results = Parser.Default.ParseArguments<Options>(args)
  .WithParsed(options =>
  {
    try
    {
      if (options.File != null)
      {
        SearchFile(
          new FileInfo(options.File),
          options.Query!,
          options.Regex
        );
      }
      else
      {
        string? file;
        while (!string.IsNullOrWhiteSpace(file = Console.ReadLine()))
        {
          SearchFile(
            new FileInfo(file),
            options.Query!,
            options.Regex
          );
        }
      }
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine($"{ex.GetType()}: {ex.Message}");
    }
  });

results.WithNotParsed(_ =>
    Console.WriteLine(CommandLine.Text.HelpText
      .RenderUsageText(results))
  );
