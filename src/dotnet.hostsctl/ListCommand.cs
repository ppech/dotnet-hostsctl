using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Text.Json;

/// <summary>
/// List the entries in the hosts file.
/// </summary>
public class ListCommand : Command<ListCommand.Settings>
{
	public class Settings : SettingsBase
	{
		
	}

	public override int Execute(CommandContext context, Settings settings)
	{
        var inputFilePath = Utils.GetInputFilePath(settings);

        var entries = HostsFile.Parse(inputFilePath);

		OutputFormatter.Entries(entries, settings.Json);

		//if (settings.Json)
		//{
		//	var json = JsonSerializer.Serialize(entries);
		//	Console.WriteLine(json);
		//}
		//else
		//{
		//	foreach (var entry in entries)
		//	{
		//		if (entry.IsEnabled)
		//			AnsiConsole.MarkupLine($"  [blue]{entry.IP}[/] {entry.Hosts} [green]{entry.Comment}[/]");
		//		else
		//			AnsiConsole.MarkupLine($"[red]#[/] [grey]{entry.IP} {entry.Hosts}[/] [green]{entry.Comment}[/]");
		//	}
		//}

		return 0;
	}
}
