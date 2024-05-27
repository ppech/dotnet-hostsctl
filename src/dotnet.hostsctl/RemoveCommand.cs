using Spectre.Console;
using Spectre.Console.Cli;

internal class RemoveCommand : Command<RemoveCommand.Settings>
{
	public class Settings : CommandSettings
	{
		[CommandArgument(0, "<hosts>")]
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

			if (entry.Hosts.Equals(settings.Hosts, StringComparison.OrdinalIgnoreCase))
			{
				if (settings.IP is null || entry.IP.Equals(settings.IP))
				{
					newEntry = null;
					AnsiConsole.MarkupLine($"[red]-  {entry.IP}[/] {entry.Hosts} [green]{entry.Comment}[/]");
				}
			}

			return newEntry;
		});

		return 0;
	}
}