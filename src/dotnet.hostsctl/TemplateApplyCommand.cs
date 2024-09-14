using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;

public class TemplateApplyCommand : Command<TemplateApplyCommand.Settings>
{
	private readonly IFileSystem fileSystem;
	private readonly IHostsFile hostsFile;
	private readonly IOutputFormatter outputFormatter;

	public class Settings : TemplateSettings
	{
		[CommandOption("-o|--output <OUTPUT>")]
		[Description("Output file")]
		public string? OutputFile { get; set; }

		[CommandOption("-j|--json")]
		[Description("Output as JSON")]
		public bool Json { get; set; }
	}

	public TemplateApplyCommand(IFileSystem fileSystem, IHostsFile hostsFile, IOutputFormatter outputFormatter)
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

		var f = fileSystem.FileInfo.New(filename);

		var newEntries = hostsFile.Parse(f);

		var outputFileName = Utils.GetSystemFilePath();

		if(!string.IsNullOrWhiteSpace(settings.OutputFile))
		{
			outputFileName = settings.OutputFile;
		}

		var outputFile = fileSystem.FileInfo.New(outputFileName);
		var currentEntries = hostsFile.Parse(outputFile);

		var addedEntries = new List<HostsFileEntry>();

		foreach (var entry in newEntries)
		{
			if (!currentEntries.Any(p => p.Hosts.Equals(entry.Hosts, StringComparison.OrdinalIgnoreCase) && p.IP.Equals(entry.IP)))
			{
				addedEntries.Add(entry);
				hostsFile.Append(outputFile, entry);
			}
		}

		outputFormatter.Entries(addedEntries, settings.Json);

		return 0;
	}
}