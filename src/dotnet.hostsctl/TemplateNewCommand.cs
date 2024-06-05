using Spectre.Console;
using Spectre.Console.Cli;

public class TemplateNewCommand : Command<TemplateNewCommand.Settings>
{
	public class Settings : TemplateSettings
	{
	}

	public override int Execute(CommandContext context, Settings settings)
	{
		var lines = """
			#
			# Hosts template file
			#
			# You can add your hosts entries here manually the same way as in the hosts file
			#
			# Or use 'hostsctl template' commands to manipulate this file
			#

			""";

		var filename = "hosts.ht";

		if(!string.IsNullOrWhiteSpace(settings.TemplatePath))
			filename = settings.TemplatePath;

		filename = Path.GetFullPath(filename);

		if(System.IO.File.Exists(filename))
		{
			AnsiConsole.MarkupLine($"[red]Template file already exists at {filename}[/]");
			return -1;
		}

		System.IO.File.WriteAllText(filename, lines);

		AnsiConsole.MarkupLine($"[green]Template file created at {filename}[/]");

		return 0;
	}
}