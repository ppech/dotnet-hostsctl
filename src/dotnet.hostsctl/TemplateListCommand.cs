using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.IO.Abstractions;

public class TemplateListCommand : Command<TemplateListCommand.Settings>
{
	private readonly IFileSystem fileSystem;
	private readonly IHostsFile hostsFile;
	private readonly IOutputFormatter outputFormatter;

	public class Settings : TemplateSettings
	{
		[CommandOption("-j|--json")]
		[Description("Output as JSON")]
		public bool Json { get; set; }
	}

	public TemplateListCommand(IFileSystem fileSystem, IHostsFile hostsFile, IOutputFormatter outputFormatter)
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

		if(!fileSystem.File.Exists(filename))
		{
			AnsiConsole.MarkupLine($"[red]Template file not found at {filename}[/]");
			return -1;
		}

		var f = fileSystem.FileInfo.New(filename);

		var entries = hostsFile.Parse(f);

		outputFormatter.Entries(entries, settings.Json);

		return 0;
	}
}