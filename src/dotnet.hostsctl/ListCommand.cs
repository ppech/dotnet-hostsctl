using Spectre.Console.Cli;
using System.ComponentModel;

/// <summary>
/// List the entries in the hosts file.
/// </summary>
public class ListCommand : Command<ListCommand.Settings>
{
	public class Settings : CommandSettings, IInputFileSettings
	{
		[CommandOption("-i|--input <file>")]
		[Description("Path of input file, default value depends on operating system")]

		public string? InputFile { get; set; }

		[CommandOption("-j|--json")]
		[Description("Output as JSON")]
		public bool Json { get; set; }
	}

	public override int Execute(CommandContext context, Settings settings)
	{
        var inputFilePath = Utils.GetInputFilePath(settings);

        var entries = HostsFile.Parse(inputFilePath);

		OutputFormatter.Entries(entries, settings.Json);

		return 0;
	}
}
