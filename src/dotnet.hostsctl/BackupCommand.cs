using Spectre.Console;
using Spectre.Console.Cli;
using System.IO.Abstractions;

/// <summary>
/// Backups the hosts file
/// </summary>
public class BackupCommand : Command<BackupCommand.Settings>
{
	private readonly IFileSystem fileSystem;

	public class Settings : HostsSettingsBase
	{

    }

    public BackupCommand(IFileSystem fileSystem)
    {
		this.fileSystem = fileSystem;
	}

    public override int Execute(CommandContext context, Settings settings)
    {
        var inputFilePath = Utils.GetInputFilePath(settings);
        var outputFilePath = Utils.GetOutputFilePath(settings, ".bak");

        if (fileSystem.File.Exists(outputFilePath))
        {
            AnsiConsole.MarkupLine($"[red]Backup file already exists at {outputFilePath}[/]");

            var response = AnsiConsole.Prompt(new ConfirmationPrompt("Do you want to override the existing backup file?"));

            if (!response)
            {
                return -1;
            }
        }

        fileSystem.File.Copy(inputFilePath, outputFilePath, true);

        AnsiConsole.MarkupLine($"[green]Backup created at {outputFilePath}[/]");

        return 0;
    }
}