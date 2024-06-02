using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

/// <summary>
/// Disables entry in hosts file
/// </summary>
public class DisableCommand : Command<DisableCommand.Settings>
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
			var newEntry = entry;

			if (entry.IsEnabled)
			{
				if (entry.Hosts.Equals(settings.HostName, StringComparison.OrdinalIgnoreCase))
				{
					if (settings.IP is null || entry.IP.Equals(settings.IP))
					{
						newEntry = entry with { IsEnabled = false };
						list.Add(newEntry);
					}
				}
			}

			return newEntry;
		});

		if (list.Count == 0)
		{
			AnsiConsole.MarkupLine($"[red]Entry with HostName '{settings.HostName}' and IP '{settings.IP}' not found or is already disabled[/]");
			return -1;
		}
		else
		{
			OutputFormatter.Entries(list, settings.Json);
		}

		return 0;
	}
}