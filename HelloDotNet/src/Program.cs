using CommandLine;
using Figgle;
using HelloDotNet;
using System.Reflection;

Parser.Default.ParseArguments<Options>(args)
  .WithParsed(AsciiArt.Write)
  .WithNotParsed(_ =>
  {
    Console.WriteLine("Available Fonts:");
    typeof(FiggleFonts)
      .GetProperties(BindingFlags.Static | BindingFlags.Public)
      .ToList().ForEach(font =>
        Console.WriteLine($"- {font.Name}")
      );
    Console.WriteLine("Usage: ./HelloDotNet <text> --font <font>");
  });
