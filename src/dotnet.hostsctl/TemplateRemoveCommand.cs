using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.IO.Abstractions;

public class TemplateRemoveCommand : Command<TemplateRemoveCommand.Settings>
{
	private readonly IFileSystem fileSystem;
	private readonly IHostsFile hostsFile;
	private readonly IOutputFormatter outputFormatter;

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

	public TemplateRemoveCommand(IFileSystem fileSystem, IHostsFile hostsFile, IOutputFormatter outputFormatter)
	{
		this.fileSystem = fileSystem;
		this.hostsFile = hostsFile;
		this.outputFormatter = outputFormatter;
	}

	public override int Execute(CommandContext context, Settings settings)
	{
		var filename = "hosts.ht";

		if (!string.IsNullOrWhiteSpace(settings.TemplatePath))
			filename = settings.TemplatePath;

		filename = fileSystem.Path.GetFullPath(filename);

		if (!fileSystem.File.Exists(filename))
		{
			AnsiConsole.MarkupLine($"[red]Template file not found at {filename}[/]");
			return -1;
		}

		var list = new List<HostsFileEntry>();

		var f = fileSystem.FileInfo.New(filename);

		hostsFile.Process(f, f, entry =>
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
			outputFormatter.Entries(list, settings.Json);
		}

		return 0;
	}
}