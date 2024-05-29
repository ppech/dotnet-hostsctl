using Spectre.Console;
using Spectre.Console.Cli;
using System.Text.Json;

internal class ExistsCommand : Command<ExistsCommand.Settings>
{
    public class Settings : InSettingsBase
    {
		[CommandArgument(0, "[hosts]")]
		public required string Hosts { get; set; }

		[CommandOption("-a|--all")]
		public bool All { get; set; }

		[CommandOption("-j|--json")]
		public bool Json { get; set; }
	}

    public override int Execute(CommandContext context, Settings settings)
    {
        var inputFilePath = Utils.GetInputFilePath(settings);

        var entries = HostsFile.Parse(inputFilePath)
			.Where(p => p.Hosts.Contains(settings.Hosts, StringComparison.OrdinalIgnoreCase));

		if(!settings.All)
			entries = entries.Where(p => p.IsEnabled);

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

		return entries.Any() ? 0 : -1;
	}
}