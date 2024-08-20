namespace Utils;

public static class Utils
{
  public static string? FindFirstMatchingLine(
    Stream stream, string query)
  {
    var reader = new StreamReader(stream);
    string? line;

    while ((line = reader.ReadLine()) != null)
    {
      if (line.Contains(query,
        StringComparison.OrdinalIgnoreCase))
      {
        return line;
      }
    }

    return null;
  }
}
