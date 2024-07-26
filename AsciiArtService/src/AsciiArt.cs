using Figgle;
using System.Reflection;

namespace AsciiArtService;

public static class AsciiArt
{
  public static Lazy<IEnumerable<(string Name, FiggleFont Font)>>
  Fonts = new(() =>
      from prop
      in typeof(FiggleFonts)
        .GetProperties(BindingFlags.Public | BindingFlags.Static)
      select (
        prop.Name,
        prop.GetValue(null) as FiggleFont
      )
  );

  public static bool
  Write(string text, out string? asciiText, string? fontName = null)
  {
    FiggleFont? font = null;
    if (!string.IsNullOrWhiteSpace(fontName))
    {
      font = typeof(FiggleFonts)
        .GetProperty(
          fontName,
          BindingFlags.Public | BindingFlags.Static
        )?.GetValue(null)
        as FiggleFont;
    }
    else { font = FiggleFonts.Standard; }

    if (font == null)
    {
      asciiText = null;
      return false;
    }
    else
    {
      asciiText = font.Render(text);
      return true;
    }
  }
}
