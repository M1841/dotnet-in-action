namespace CustomerBatchImporter.Src;

public class CsvImporter(ICustomerRepository customerRepo, ICsvParser csvParser)
{
  public async Task ReadAsync(Stream stream)
  {
    await foreach (var customer in _csvParser.ParseAsync(stream))
    {
      var existing = await _customerRepo
        .GetByEmailAsync(customer.Email);

      if (existing == null)
      {
        await _customerRepo.CreateAsync(customer);
      }
      else
      {
        await _customerRepo.UpdateAsync(
          new UpdateCustomerDto(
            existing.Id,
            customer.Name,
            customer.License
          )
        );
      }
    }
  }

  private readonly ICustomerRepository _customerRepo = customerRepo;
  private readonly ICsvParser _csvParser = csvParser;
}
