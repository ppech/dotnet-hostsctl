using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.IO.Abstractions;

/// <summary>
/// List the entries in the hosts file.
/// </summary>
public class ListCommand : Command<ListCommand.Settings>
{
	private readonly IFileSystem fileSystem;
	private readonly IHostsFile hostsFile;
	private readonly IOutputFormatter outputFormatter;

	public ListCommand(
		IFileSystem fileSystem,
		IHostsFile hostsFile,
		IOutputFormatter outputFormatter)
	{
		this.fileSystem = fileSystem;
		this.hostsFile = hostsFile;
		this.outputFormatter = outputFormatter;
	}
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
		var cd = fileSystem.Directory.GetCurrentDirectory();
		AnsiConsole.MarkupLine($"[yellow]Current directory:[/] {cd}");

		var inputFilePath = Utils.GetInputFilePath(settings);

		var f = fileSystem.FileInfo.New(inputFilePath);

		if (!f.Exists)
		{
			AnsiConsole.MarkupLine($"[red]File not found:[/] {inputFilePath}");
			return 1;
		}

		var entries = hostsFile.Parse(f);

		outputFormatter.Entries(entries, settings.Json);

		return 0;
	}
}
