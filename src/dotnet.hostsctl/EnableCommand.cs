using Spectre.Console;
using Spectre.Console.Cli;

internal class EnableCommand : Command<EnableCommand.Settings>
{
	public class Settings : InOutSettingsBase
	{
		[CommandArgument(0, "[hosts]")]
		public required string Hosts { get; set; }

		[CommandArgument(1, "[ip]")]
		public string? IP { get; set; }
	}

	public override int Execute(CommandContext context, Settings settings)
	{
        var inputFilePath = Utils.GetInputFilePath(settings);
        var outputFilePath = Utils.GetOutputFilePath(settings);

        HostsFile.Process(inputFilePath, outputFilePath, entry =>
		{
			var newEntry = entry;

			if (!entry.IsEnabled)
			{
				if (entry.Hosts.Equals(settings.Hosts, StringComparison.OrdinalIgnoreCase))
				{
					if (settings.IP is null || entry.IP.Equals(settings.IP))
					{
						newEntry = entry with { IsEnabled = true };
						AnsiConsole.MarkupLine($"  [blue]{entry.IP}[/] {entry.Hosts} [green]{entry.Comment}[/]");
					}
				}
			}

			return newEntry;
		});

		return 0;
	}
}