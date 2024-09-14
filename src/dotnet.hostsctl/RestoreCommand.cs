using Spectre.Console;
using Spectre.Console.Cli;
using System.IO.Abstractions;

/// <summary>
/// Restores the hosts file from a backup
/// </summary>
public class RestoreCommand : Command<RestoreCommand.Settings>
{
	private readonly IFileSystem fileSystem;

	public class Settings : HostsSettingsBase
	{

    }

    public RestoreCommand(IFileSystem fileSystem)
    {
		this.fileSystem = fileSystem;
	}

    public override int Execute(CommandContext context, Settings settings)
    {
        var inputFilePath = Utils.GetInputFilePath(settings, ".bak");
        var outputFilePath = Utils.GetOutputFilePath(settings);



        if (!fileSystem.File.Exists(inputFilePath))
        {
            AnsiConsole.MarkupLine($"[red]Backup file does not exist at {inputFilePath}[/]");
            return -1;
        }

        fileSystem.File.Copy(inputFilePath, outputFilePath, true);

        AnsiConsole.MarkupLine($"[green]Backup restored from {inputFilePath} to {outputFilePath}[/]");

        return 0;
    }
}