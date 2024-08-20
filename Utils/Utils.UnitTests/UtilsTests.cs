namespace Utils.UnitTests;

public class UtilsTests
{
  [Fact]
  public void MemoryStream()
  {
    using var stream = new MemoryStream();
    var writer = new StreamWriter(stream);

    writer.WriteLine("abc");
    writer.WriteLine("def");
    writer.WriteLine("ghi");
    writer.Flush();
    stream.Seek(0, SeekOrigin.Begin);

    var line = Utils.FindFirstMatchingLine(stream, "f");
    Assert.Equal("def", line);
  }

  [Fact]
  public void FileStream()
  {
    var file = new FileInfo("testdata.txt");
    using var stream = file.OpenRead();

    var line = Utils.FindFirstMatchingLine(stream, "f");
    Assert.Equal("def", line);
  }

  [Fact]
  public void EmbeddedResource()
  {
    var type = this.GetType();
    var asm = type.Assembly;
    using var stream = asm.GetManifestResourceStream(
      type.Namespace + ".dontcopy.txt");

    var line = Utils.FindFirstMatchingLine(stream!, "f");
    Assert.Equal("def", line);
  }
}
