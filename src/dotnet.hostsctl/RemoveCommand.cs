using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

/// <summary>
/// Removes entry from hosts file
/// </summary>
public class RemoveCommand : Command<RemoveCommand.Settings>
{
	public class Settings : HostsEntrySettingsBase
	{

	}

	public override int Execute(CommandContext context, Settings settings)
	{
        var inputFilePath = Utils.GetInputFilePath(settings);
        var outputFilePath = Utils.GetOutputFilePath(settings);

		var list = new List<HostsFileEntry>();

        HostsFile.Process(inputFilePath, outputFilePath, entry =>
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
			OutputFormatter.Entries(list, settings.Json);
		}

		return 0;
	}
}