using Spectre.Console;
using Spectre.Console.Cli;

internal class AddCommand : Command<AddCommand.Settings>
{
	public class Settings : CommandSettings
	{
		[CommandArgument(0, "<hosts>")]
		public required string Hosts { get; set; }

		[CommandArgument(1, "[ip]")]
		public string? IP { get; set; }

		[CommandArgument(2, "[comment]")]
		public string? Comment { get; set; }
	}
	public override int Execute(CommandContext context, Settings settings)
	{
		var entry = new HostsFileEntry
		(
			IsEnabled: true,
			IP: settings.IP ?? "127.0.0.1",
			Hosts: settings.Hosts,
			Comment: settings.Comment
		);

		var path = Utils.GetHostsFilePath();

		var entries = HostsFile.Parse(path);

		// check if the entry with hosts and ip already exists
		if (entries.Any(p => p.Hosts.Equals(entry.Hosts, StringComparison.OrdinalIgnoreCase) && p.IP.Equals(entry.IP)))
		{
			AnsiConsole.MarkupLine($"[red]Entry with hosts '{entry.Hosts}' and IP '{entry.IP}' already exists[/]");
			return -1;
		}

		HostsFile.Append(path, entry);

		return 0;
	}
}