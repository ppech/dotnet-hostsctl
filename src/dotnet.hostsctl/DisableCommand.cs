using Spectre.Console;
using Spectre.Console.Cli;

internal class DisableCommand : Command<DisableCommand.Settings>
{
	public class Settings : CommandSettings
	{
		[CommandArgument(0, "[hosts]")]
		public required string Hosts { get; set; }

		[CommandArgument(1, "[ip]")]
		public string? IP { get; set; }
	}

	public override int Execute(CommandContext context, Settings settings)
    {
		var path = Utils.GetHostsFilePath();

		HostsFile.Process(path, entry =>
		{
			var newEntry = entry;

			if (entry.IsEnabled)
			{
				if (entry.Hosts.Equals(settings.Hosts, StringComparison.OrdinalIgnoreCase))
				{
					if (settings.IP is null || entry.IP.Equals(settings.IP))
					{
						newEntry = entry with { IsEnabled = false };
						AnsiConsole.MarkupLine($"[red]#[/] [grey]{entry.IP} {entry.Hosts}[/] [green]{entry.Comment}[/]");
					}
				}
			}

			return newEntry;
		});

		return 0;
	}
}