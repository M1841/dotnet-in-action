using System.Text;
using FakeItEasy;

namespace CustomerBatchImporter.Tests;

public class CsvImporterTests
{
  [Fact]
  public async Task ValidLine()
  {
    // Arrange
    const string email = "jdoe@email.com";
    const string name = "John";
    const string license = "Pro";
    const string csv = $"{email},{name},{license}";
    A.CallTo(() => _fakeCustomerRepo
      .GetByEmailAsync(email))
      .Returns(default(Customer));

    // Act
    var stream = GetStreamFromString(csv);
    await _csvImporter.ReadAsync(stream);

    // Assert
    A.CallTo(() => _fakeCustomerRepo
      .GetByEmailAsync((email)))
      .MustHaveHappened();
    A.CallTo(() => _fakeCustomerRepo
      .CreateAsync(A<NewCustomerDto>.That
        .Matches(customer =>
          customer.Email.Equals(email)
          && customer.Name.Equals(name)
          && customer.License.Equals(license))))
      .MustHaveHappened();
  }

  [Fact]
  public async Task InvalidLine()
  {
    // Arrange
    var stream = GetStreamFromString("no commas here");

    // Act
    await _csvImporter.ReadAsync(stream);

    // Assert
    var calls = Fake
      .GetCalls(_fakeCustomerRepo);
    Assert.Empty(calls);
  }

  [Fact]
  public async Task TwoValidOneInvalid()
  {
    // Arrange
    A.CallTo(() => _fakeCustomerRepo
      .GetByEmailAsync(A<string>.Ignored))
      .Returns(default(Customer));

    // Act
    var stream = GetStreamFromString(
      "a@b.com,customer1,None\ninvalid\nc@d.com,customer2,None");
    await _csvImporter.ReadAsync(stream);

    // Assert
    A.CallTo(() => _fakeCustomerRepo
      .CreateAsync(A<NewCustomerDto>.Ignored))
      .MustHaveHappenedTwiceExactly();
  }

  [Fact]
  public async Task NullEmail()
  {
    A.CallTo(() => _fakeCustomerRepo
      .GetByEmailAsync(""))
      .Throws<ArgumentException>();

    var stream = GetStreamFromString(",name,license");
    await Assert.ThrowsAsync<ArgumentException>(
      () => _csvImporter.ReadAsync(stream));
  }

  [Fact]
  public async Task ValidUpdate()
  {
    // Arrange
    const string email = "jdoe@email.com";
    const string name = "John";
    const string license = "Pro";
    const string csv = $"{email},{name},{license}";
    const string newLicense = "Free";
    const string newCsv = $"{email},{name},{newLicense}";

    // Act
    var stream = GetStreamFromString(csv);
    await _csvImporter.ReadAsync(stream);
    var id = (await _fakeCustomerRepo.GetByEmailAsync(email))!.Id;
    stream = GetStreamFromString(csv);
    await _csvImporter.ReadAsync(stream);

    // Assert
    A.CallTo(() => _fakeCustomerRepo
      .UpdateAsync(A<UpdateCustomerDto>.That
        .Matches(customer =>
          customer.Id == id
          && customer.NewLicense != null
          && customer.NewLicense == license)))
      .MustHaveHappened();
  }

  public CsvImporterTests()
  {
    _fakeCustomerRepo = A.Fake<ICustomerRepository>();
    _csvParser = new GenericCsvParser();
    _csvImporter = new CsvImporter(
      _fakeCustomerRepo,
      _csvParser);
  }

  private readonly ICustomerRepository _fakeCustomerRepo;
  private readonly CsvImporter _csvImporter;
  private readonly ICsvParser _csvParser;

  private static Stream GetStreamFromString(string content)
    => new MemoryStream(Encoding.UTF8.GetBytes(content));
}
