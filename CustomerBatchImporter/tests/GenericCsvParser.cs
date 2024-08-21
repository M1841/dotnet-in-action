using System.Globalization;
using CsvHelper;

namespace CustomerBatchImporter.Tests;

public class GenericCsvParser : ICsvParser
{
  public async IAsyncEnumerable<NewCustomerDto> ParseAsync(Stream stream)
  {
    using var reader = new StreamReader(stream);
    using var parser = new CsvReader(reader, CultureInfo.InvariantCulture);
    while (await parser.ReadAsync())
    {
      if (parser.ColumnCount == 3)
      {
        var email = parser.GetField(index: 0);
        var name = parser.GetField(1);
        var license = parser.GetField(2);
        if (email != null && name != null && license != null)
        {
          yield return new NewCustomerDto(email, name, license);
        }
      }
    }
  }
}
