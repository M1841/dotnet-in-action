using GetJokes;
using CommandLine;
using System.Text.Json;

static void _FindJsonWithDom(
  Stream input,
  Stream output,
  string category
)
{
  using var writer = new Utf8JsonWriter(
    output,
    new JsonWriterOptions { Indented = true }
  );
  using var jsonDoc = JsonDocument.Parse(input);

  writer.WriteStartArray();
  foreach (
    var jokeElement
    in jsonDoc.RootElement.EnumerateArray()
  )
  {
    string? type = jokeElement
      .GetProperty("type")
      .GetString();

    if (string.Equals(
      category,
      type,
      StringComparison.OrdinalIgnoreCase
    ))
    {
      string? setup = jokeElement
        .GetProperty("setup")
        .GetString();
      string? punchline = jokeElement
        .GetProperty("punchline")
        .GetString();

      writer.WriteStartObject();
      writer.WriteString("setup", setup);
      writer.WriteString("punchline", punchline);
      writer.WriteEndObject();
    }
  }
  writer.WriteEndArray();
}

static void FindJsonWithSerialization(
  Stream input,
  Stream output,
  string category
)
{
  var serialOptions =
    new JsonSerializerOptions
    {
      WriteIndented = true,
      PropertyNameCaseInsensitive = true
    };
  var jokes = JsonSerializer.Deserialize<Joke[]>(
    input,
    serialOptions
  );

  JsonSerializer.Serialize(
    output,
    jokes?
      .Where(j => string.Equals(
        category,
        j.Type,
        StringComparison.OrdinalIgnoreCase
      ))
      .Select(j => new
      {
        setup = j.Setup,
        punchline = j.Punchline
      })
      .ToArray(),
    serialOptions
  );
}

var results = Parser.Default
  .ParseArguments<Options>(args)
  .WithParsed(options =>
  {
    try
    {
      var input = new FileInfo(options.Input!);
      FileInfo? output = null;

      if (options.Output != null)
      {
        output = new FileInfo(options.Output);
        if (output.Exists) { output.Delete(); }
      }

      using var inputStream = input.OpenRead();
      using var outputStream = output != null
        ? output.OpenWrite()
        : Console.OpenStandardOutput();

      FindJsonWithSerialization(
        inputStream,
        outputStream,
        options.Category
      );
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine($"{ex.GetType()}: {ex.Message}");
    }
  });

results.WithNotParsed(_ =>
    Console.WriteLine(CommandLine.Text
      .HelpText.RenderUsageText(results)
    )
  );
