namespace CustomerBatchImporter.Src;

public interface ICsvParser
{
  IAsyncEnumerable<NewCustomerDto> ParseAsync(Stream stream);
}
