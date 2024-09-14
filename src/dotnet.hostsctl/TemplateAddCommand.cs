using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.IO.Abstractions;

public class TemplateAddCommand : Command<TemplateAddCommand.Settings>
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

		[CommandArgument(2, "[comment]")]
		[Description("Comment for the entry")]
		public string? Comment { get; set; }

		[CommandOption("-j|--json")]
		[Description("Output as JSON")]
		public bool Json { get; set; }
	}

	public TemplateAddCommand(IFileSystem fileSystem, IHostsFile hostsFile, IOutputFormatter outputFormatter)
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

		var entry = new HostsFileEntry
		(
			IsEnabled: true,
			IP: settings.IP ?? "127.0.0.1",
			Hosts: settings.HostName,
			Comment: settings.Comment
		);

		var f = fileSystem.FileInfo.New(filename);

		var entries = hostsFile.Parse(f);

		// check if the entry with hosts and ip already exists
		if (entries.Any(p => p.Hosts.Equals(entry.Hosts, StringComparison.OrdinalIgnoreCase) && p.IP.Equals(entry.IP)))
		{
			AnsiConsole.MarkupLine($"[red]Entry with HostName '{entry.Hosts}' and IP '{entry.IP}' already exists[/]");
			return -2;
		}

		hostsFile.Append(f, entry);

		outputFormatter.Entry(entry, settings.Json);

		return 0;
	}
}