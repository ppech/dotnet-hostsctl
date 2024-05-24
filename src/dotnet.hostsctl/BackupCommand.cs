using Spectre.Console;
using Spectre.Console.Cli;

internal class BackupCommand : Command
{
    public override int Execute(CommandContext context)
    {
        var hostsFilePath = Utils.GetHostsFilePath();
        var backupFilePath = hostsFilePath + ".bak";

        if (File.Exists(backupFilePath))
        {
            AnsiConsole.MarkupLine($"[red]Backup file already exists at {backupFilePath}[/]");

            var response = AnsiConsole.Prompt(new ConfirmationPrompt("Do you want to override the existing backup file?"));

            if (!response)
            {
                return -1;
            }
        }

        File.Copy(hostsFilePath, backupFilePath, true);

        AnsiConsole.MarkupLine($"[green]Backup created at {backupFilePath}[/]");

        return 0;
    }
}