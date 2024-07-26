using System.Reflection;
using Figgle;

namespace HelloDotNet;

public static class AsciiArt
{
  public static void Write(Options options)
  {
    if (options?.Text != null)
    {
      options.Text.ToList().ForEach(text =>
        Console.WriteLine(GetFont(options.Font)
          .Render(options.ShowEvens
            ? (options.ShowOdds ? text : text.EvenChars())
            : (options.ShowOdds ? text.OddChars() : text))
        )
      );

      Console.WriteLine($"Brought to you by {typeof(AsciiArt).FullName}");
    }
  }
  private static FiggleFont GetFont(string? fontArg)
  {
    FiggleFont? font = null;

    if (!string.IsNullOrWhiteSpace(fontArg))
    {
      font = typeof(FiggleFonts)
        .GetProperty(fontArg, BindingFlags.Static | BindingFlags.Public)
        ?.GetValue(null) as FiggleFont;
      if (font == null)
      {
        Console.WriteLine($"Could not find font '{fontArg}'");
      }
    }
    font ??= FiggleFonts.Standard;

    return font;
  }
}
