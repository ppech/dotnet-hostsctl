using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

public class TemplateListCommand : Command<TemplateListCommand.Settings>
{
	public class Settings : TemplateSettings
	{
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

		if(!File.Exists(filename))
		{
			AnsiConsole.MarkupLine($"[red]Template file not found at {filename}[/]");
			return -1;
		}

		var entries = HostsFile.Parse(filename);

		OutputFormatter.Entries(entries, settings.Json);

		return 0;
	}
}