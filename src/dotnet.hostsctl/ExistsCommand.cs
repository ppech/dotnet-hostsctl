using Spectre.Console;
using Spectre.Console.Cli;

internal class ExistsCommand : Command<ExistsCommand.Settings>
{
    public class Settings : CommandSettings
    {
        public required string Hosts { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var path = Utils.GetHostsFilePath();

        HostsFile.Read(path, line =>
        {
            var m = HostsFile.HostsFileEntryPattern.Match(line);

            if (m.Success)
            {
                var isEnabled = !m.Groups[1].Success;
                var ip = m.Groups[2].Value;
                var hosts = m.Groups[3].Value;
                var comment = m.Groups[4].Success ? m.Groups[4].Value[1..] : string.Empty;

                if (isEnabled)
                {
                    AnsiConsole.MarkupLine($"[green]{ip}[/] [blue]{hosts}[/] {comment}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]{ip}[/] [blue]{hosts}[/] {comment}");
                }
            }
        });

        return 0;

    }
}