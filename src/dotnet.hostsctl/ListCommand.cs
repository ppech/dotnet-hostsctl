using Spectre.Console;
using Spectre.Console.Cli;

internal class ListCommand : Command
{
    public override int Execute(CommandContext context)
    {
        var path = Utils.GetHostsFilePath();

        var entries = HostsFile.Parse(path);

        foreach (var entry in entries)
        {
            if (entry.IsEnabled)
            {
                AnsiConsole.MarkupLine($"[green]{entry.IP}[/] [blue]{entry.Hosts}[/] {entry.Comment}");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]{entry.IP}[/] [blue]{entry.Hosts}[/] {entry.Comment}");
            }
        }

        return 0;
    }
}