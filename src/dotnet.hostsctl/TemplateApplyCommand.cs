using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Generic;
using System.ComponentModel;

public class TemplateApplyCommand : Command<TemplateApplyCommand.Settings>
{
	public class Settings : TemplateSettings
	{
		[CommandOption("-o|--output <OUTPUT>")]
		[Description("Output file")]
		public string? OutputFile { get; set; }

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

		var newEntries = HostsFile.Parse(filename);

		var outputFile = Utils.GetSystemFilePath();

		if(!string.IsNullOrWhiteSpace(settings.OutputFile))
		{
			outputFile = settings.OutputFile;
		}

		var currentEntries = HostsFile.Parse(outputFile);

		var addedEntries = new List<HostsFileEntry>();

		foreach (var entry in newEntries)
		{
			if (!currentEntries.Any(p => p.Hosts.Equals(entry.Hosts, StringComparison.OrdinalIgnoreCase) && p.IP.Equals(entry.IP)))
			{
				addedEntries.Add(entry);
				HostsFile.Append(outputFile, entry);
			}
		}

		OutputFormatter.Entries(addedEntries, settings.Json);

		return 0;
	}
}