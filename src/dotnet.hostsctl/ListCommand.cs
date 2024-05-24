﻿using Spectre.Console;
using Spectre.Console.Cli;

internal class ListCommand : Command
{
    public override int Execute(CommandContext context)
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