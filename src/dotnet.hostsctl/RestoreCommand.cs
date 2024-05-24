using Spectre.Console;
using Spectre.Console.Cli;

internal class RestoreCommand : Command
{
    public override int Execute(CommandContext context)
    {
        var hostsFilePath = Utils.GetHostsFilePath();
        var backupFilePath = hostsFilePath + ".bak";

        if (!File.Exists(backupFilePath))
        {
            AnsiConsole.MarkupLine($"[red]Backup file does not exist at {backupFilePath}[/]");
            return -1;
        }

        File.Copy(backupFilePath, hostsFilePath, true);

        AnsiConsole.MarkupLine($"[green]Backup restored from {backupFilePath}[/]");

        return 0;
    }
}