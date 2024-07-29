using CmdArgsTemplate;
using CommandLine;

var results = Parser.Default.ParseArguments<Options>(args)
  .WithParsed(options =>
    Console.WriteLine($"Argument passed: \"{options.Text}\"")
  );

results.WithNotParsed(_ =>
    Console.WriteLine(CommandLine.Text.HelpText.RenderUsageText(results))
  );
