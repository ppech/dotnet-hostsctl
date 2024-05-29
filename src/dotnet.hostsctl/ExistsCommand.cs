using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Text.Json;

internal class ExistsCommand : Command<ExistsCommand.Settings>
{
    public class Settings : InSettingsBase
    {
		[CommandArgument(0, "<hostname>")]
		[Description("Host name, ex. app.mydomain.local")]
		public required string HostName { get; set; }

		[CommandArgument(1, "[ip]")]
		[Description("IP address, default is 127.0.0.1")]
		public string? IP { get; set; }

		[CommandOption("-a|--all")]
		public bool All { get; set; }

		[CommandOption("-j|--json")]
		public bool Json { get; set; }
	}

    public override int Execute(CommandContext context, Settings settings)
    {
        var inputFilePath = Utils.GetInputFilePath(settings);

        var entries = HostsFile.Parse(inputFilePath)
			.Where(p => p.Hosts.Contains(settings.HostName, StringComparison.OrdinalIgnoreCase));

		if (settings.IP is not null)
			entries = entries.Where(p => p.IP.Equals(settings.IP));

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