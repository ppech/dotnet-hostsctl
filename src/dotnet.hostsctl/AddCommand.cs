using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.IO.Abstractions;

/// <summary>
/// Adds entry to the hosts file
/// </summary>
public class AddCommand : Command<AddCommand.Settings>
{
	private readonly IFileSystem fileSystem;
	private readonly IHostsFile hostsFile;
	private readonly IOutputFormatter outputFormatter;

	public class Settings : HostsEntrySettingsBase
	{
		[CommandArgument(2, "[comment]")]
		[Description("Comment for the entry")]
		public string? Comment { get; set; }
	}

	public AddCommand(IFileSystem fileSystem, IHostsFile hostsFile, IOutputFormatter outputFormatter)
	{
		this.fileSystem = fileSystem;
		this.hostsFile = hostsFile;
		this.outputFormatter = outputFormatter;
	}

	public override int Execute(CommandContext context, Settings settings)
	{
		var entry = new HostsFileEntry
		(
			IsEnabled: true,
			IP: settings.IP ?? "127.0.0.1",
			Hosts: settings.HostName,
			Comment: settings.Comment
		);

		var inputFilePath = Utils.GetInputFilePath(settings);
		var outputFilePath = Utils.GetOutputFilePath(settings);

		var inputFile = fileSystem.FileInfo.New(inputFilePath);

		if (!inputFile.Exists)
		{
			AnsiConsole.MarkupLine($"[red]File not found:[/] {inputFilePath}");
			return 1;
		}

		var entries = hostsFile.Parse(inputFile);

		// check if the entry with hosts and ip already exists
		if (entries.Any(p => p.Hosts.Equals(entry.Hosts, StringComparison.OrdinalIgnoreCase) && p.IP.Equals(entry.IP)))
		{
			AnsiConsole.MarkupLine($"[red]Entry with HostName '{entry.Hosts}' and IP '{entry.IP}' already exists[/]");
			return -1;
		}

		var outputFile = fileSystem.FileInfo.New(outputFilePath);
		hostsFile.Append(outputFile, entry);

		outputFormatter.Entry(entry, settings.Json);

		return 0;
	}
}