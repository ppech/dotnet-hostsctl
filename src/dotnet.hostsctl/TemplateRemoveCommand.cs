using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

public class TemplateRemoveCommand : Command<TemplateRemoveCommand.Settings>
{
	public class Settings : TemplateSettings
	{
		[CommandArgument(0, "<hostname>")]
		[Description("Host name, ex. app.mydomain.local")]
		public required string HostName { get; set; }

		[CommandArgument(1, "[ip]")]
		[Description("IP address, default is 127.0.0.1")]
		public string? IP { get; set; }

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

		var list = new List<HostsFileEntry>();

		HostsFile.Process(filename, filename, entry =>
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