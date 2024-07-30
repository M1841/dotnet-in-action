using FindFiles;
using CommandLine;

static void RecursiveFind(
  DirectoryInfo rootDir,
  string query,
  bool recurse
)
{
  var fileEnumerator = rootDir.EnumerateFiles(query);

  if (fileEnumerator.Any())
  {
    Console.WriteLine($"Files found in {rootDir}:");

    foreach (var file in fileEnumerator)
    {
      Console.WriteLine($"  {file.Name}");
    }

  }

  if (recurse)
  {
    foreach (var subDir in rootDir.GetDirectories())
    {
      RecursiveFind(subDir, query, recurse);
    }
  }
}

var results = Parser.Default
  .ParseArguments<Options>(args)
  .WithParsed(options =>
  {
    try
    {
      RecursiveFind(
        new DirectoryInfo(options.RootDir),
        options.Query,
        options.Recurse
      );
    }
    catch (DirectoryNotFoundException)
    {
      Console.Error.WriteLine($"Can't find directory {options.RootDir}");
    }
    catch (UnauthorizedAccessException)
    {
      Console.Error.WriteLine($"Access to {options.RootDir} is not allowed");
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine($"{ex.GetType()}: {ex.Message}");
    }
  });

results.WithNotParsed(_ =>
    Console.WriteLine(CommandLine.Text.HelpText.RenderUsageText(results))
  );
