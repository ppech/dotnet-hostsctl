using Spectre.Console;
using Spectre.Console.Cli;
using System.IO.Abstractions;

public class TemplateNewCommand : Command<TemplateNewCommand.Settings>
{
	private readonly IFileSystem fileSystem;

	public class Settings : TemplateSettings
	{
	}

	public TemplateNewCommand(IFileSystem fileSystem)
	{
		this.fileSystem = fileSystem;
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

		filename = fileSystem.Path.GetFullPath(filename);

		if(fileSystem.File.Exists(filename))
		{
			AnsiConsole.MarkupLine($"[red]Template file already exists at {filename}[/]");
			return -1;
		}

		fileSystem.File.WriteAllText(filename, lines);

		AnsiConsole.MarkupLine($"[green]Template file created at {filename}[/]");

		return 0;
	}
}