using Spectre.Console;
using Spectre.Console.Cli;

/// <summary>
/// Restores the hosts file from a backup
/// </summary>
public class RestoreCommand : Command<RestoreCommand.Settings>
{
    public class Settings : HostsSettingsBase
	{

    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var inputFilePath = Utils.GetInputFilePath(settings, ".bak");
        var outputFilePath = Utils.GetOutputFilePath(settings);

        if (!File.Exists(inputFilePath))
        {
            AnsiConsole.MarkupLine($"[red]Backup file does not exist at {inputFilePath}[/]");
            return -1;
        }

        File.Copy(inputFilePath, outputFilePath, true);

        AnsiConsole.MarkupLine($"[green]Backup restored from {inputFilePath} to {outputFilePath}[/]");

        return 0;
    }
}