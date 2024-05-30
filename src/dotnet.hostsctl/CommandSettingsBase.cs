using Spectre.Console.Cli;
using System.ComponentModel;

public class SettingsBase : CommandSettings
{
    [CommandOption("-i|--input <file>")]
    [Description("Path of input file, default value depends on operating system")]

    public string? InputFile { get; set; }

	[CommandOption("-j|--json")]
	[Description("Output as JSON")]
	public bool Json { get; set; }
}

public class InOutSettingsBase : SettingsBase
{
    [CommandOption("-o|--output <file>")]
    [Description("Path of output file, default value is same as input file")]
    public string? OutputFile { get; set; }
}