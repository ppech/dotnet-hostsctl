using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

public class TemplateAddCommand : Command<TemplateAddCommand.Settings>
{
	public class Settings : TemplateSettings
	{
		[CommandArgument(0, "<hostname>")]
		[Description("Host name, ex. app.mydomain.local")]
		public required string HostName { get; set; }

		[CommandArgument(1, "[ip]")]
		[Description("IP address, default is 127.0.0.1")]
		public string? IP { get; set; }

		[CommandArgument(2, "[comment]")]
		[Description("Comment for the entry")]
		public string? Comment { get; set; }

		[CommandOption("-j|--json")]
		[Description("Output as JSON")]
		public bool Json { get; set; }
	}

	public override int Execute(CommandContext context, Settings settings)
	{
		var filename = "hosts.ht";

		if (!string.IsNullOrWhiteSpace(settings.TemplatePath))
			filename = settings.TemplatePath;

		filename = Path.GetFullPath(filename);

		if (!File.Exists(filename))
		{
			AnsiConsole.MarkupLine($"[red]Template file not found at {filename}[/]");
			return -1;
		}

		var entry = new HostsFileEntry
		(
			IsEnabled: true,
			IP: settings.IP ?? "127.0.0.1",
			Hosts: settings.HostName,
			Comment: settings.Comment
		);

		var entries = HostsFile.Parse(filename);

		// check if the entry with hosts and ip already exists
		if (entries.Any(p => p.Hosts.Equals(entry.Hosts, StringComparison.OrdinalIgnoreCase) && p.IP.Equals(entry.IP)))
		{
			AnsiConsole.MarkupLine($"[red]Entry with HostName '{entry.Hosts}' and IP '{entry.IP}' already exists[/]");
			return -2;
		}

		HostsFile.Append(filename, entry);

		OutputFormatter.Entry(entry, settings.Json);

		return 0;
	}
}