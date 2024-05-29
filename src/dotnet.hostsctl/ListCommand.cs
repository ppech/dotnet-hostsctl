using Spectre.Console;
using Spectre.Console.Cli;
using System.Text.Json;

internal class ListCommand : Command<ListCommand.Settings>
{
	public class Settings : InSettingsBase
	{
		[CommandOption("-j|--json")]
		public bool Json { get; set; }
	}

	public override int Execute(CommandContext context, Settings settings)
	{
        var inputFilePath = Utils.GetInputFilePath(settings);
        
        var entries = HostsFile.Parse(inputFilePath);

		if (settings.Json)
		{
			var json = JsonSerializer.Serialize(entries);
			Console.WriteLine(json);
		}
		else
		{
			foreach (var entry in entries)
			{
				if (entry.IsEnabled)
					AnsiConsole.MarkupLine($"  [blue]{entry.IP}[/] {entry.Hosts} [green]{entry.Comment}[/]");
				else
					AnsiConsole.MarkupLine($"[red]#[/] [grey]{entry.IP} {entry.Hosts}[/] [green]{entry.Comment}[/]");
			}
		}

		return 0;
	}
}
