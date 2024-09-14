using Spectre.Console;
using Spectre.Console.Cli;
using System.IO.Abstractions;

/// <summary>
/// Removes entry from hosts file
/// </summary>
public class RemoveCommand : Command<RemoveCommand.Settings>
{
	private readonly IFileSystem fileSystem;
	private readonly IHostsFile hostsFile;
	private readonly IOutputFormatter outputFormatter;

	public class Settings : HostsEntrySettingsBase
	{

	}

	public RemoveCommand(IFileSystem fileSystem, IHostsFile hostsFile, IOutputFormatter outputFormatter)
	{
		this.fileSystem = fileSystem;
		this.hostsFile = hostsFile;
		this.outputFormatter = outputFormatter;
	}

	public override int Execute(CommandContext context, Settings settings)
	{
        var inputFilePath = Utils.GetInputFilePath(settings);
        var outputFilePath = Utils.GetOutputFilePath(settings);

		var list = new List<HostsFileEntry>();

		var inputFile = fileSystem.FileInfo.New(inputFilePath);
		var outputFile = fileSystem.FileInfo.New(outputFilePath);

        hostsFile.Process(inputFile, outputFile, entry =>
		{
			if (entry.Hosts.Equals(settings.HostName, StringComparison.OrdinalIgnoreCase))
			{
				if (settings.IP is null || entry.IP.Equals(settings.IP))
				{
					list.Add(entry);
					return null;
				}
			}

			return entry;
		});

		if (list.Count == 0)
		{
			AnsiConsole.MarkupLine($"[red]Entry with HostName '{settings.HostName}' and IP '{settings.IP}' not found[/]");
			return -1;
		}
		else
		{
			outputFormatter.Entries(list, settings.Json);
		}

		return 0;
	}
}