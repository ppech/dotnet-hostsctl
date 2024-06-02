using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

/// <summary>
/// Adds entry to the hosts file
/// </summary>
public class AddCommand : Command<AddCommand.Settings>
{
	public class Settings : HostsEntrySettingsBase
	{
		[CommandArgument(2, "[comment]")]
		[Description("Comment for the entry")]
		public string? Comment { get; set; }
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

        var entries = HostsFile.Parse(inputFilePath);

		// check if the entry with hosts and ip already exists
		if (entries.Any(p => p.Hosts.Equals(entry.Hosts, StringComparison.OrdinalIgnoreCase) && p.IP.Equals(entry.IP)))
		{
			AnsiConsole.MarkupLine($"[red]Entry with HostName '{entry.Hosts}' and IP '{entry.IP}' already exists[/]");
			return -1;
		}

		HostsFile.Append(outputFilePath, entry);

		OutputFormatter.Entry(entry, settings.Json);

		return 0;
	}
}