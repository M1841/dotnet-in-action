using AsciiArtService;
using Figgle;
using BarcodeStandard;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/fonts", (
    int? limit,
    int? offset,
    FiggleTextDirection? direction,
    string? name,
    string? order
) =>
{
  var query =
    from font
    in AsciiArt.Fonts.Value
    where (
      name == null
      || font.Name.Contains(
        name,
        StringComparison.OrdinalIgnoreCase
      )
    ) && (
      direction == null
      || font.Font.Direction == direction
    )
    select font.Name;

  if ("descending".Contains(
    order ?? "asc",
    StringComparison.OrdinalIgnoreCase
  ))
  {
    query = query.OrderDescending();
  }
  else { query = query.Order(); }

  return query
    .Skip(offset ?? 0)
    .Take(limit.HasValue ? Math.Min(limit.Value, 50) : 50);
});

app.MapGet("/{text}", (
  string text,
  string? font
) => AsciiArt.Write(
      text,
      out var asciiText,
      font
    ) ? Results.Content(asciiText)
      : Results.NotFound()
);

app.MapGet("/barcode/{text}", (string text) =>
{
  var barcode = new Barcode
  {
    ImageFormat = SkiaSharp.SKEncodedImageFormat.Jpeg
  };

  barcode.Encode(
    BarcodeStandard.Type.Pharmacode,
    text,
    SkiaSharp.SKColors.Black,
    SkiaSharp.SKColors.White
  );

  return Results.File(
    barcode.EncodedImageBytes,
    "image/jpeg"
  );
});

app.Run();
