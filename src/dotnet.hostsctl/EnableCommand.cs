using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

internal class EnableCommand : Command<EnableCommand.Settings>
{
	public class Settings : InOutSettingsBase
	{
		[CommandArgument(0, "<hostname>")]
		[Description("Host name, ex. app.mydomain.local")]
		public required string HostName { get; set; }

		[CommandArgument(1, "[ip]")]
		[Description("IP address, default is 127.0.0.1")]
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
				if (entry.Hosts.Equals(settings.HostName, StringComparison.OrdinalIgnoreCase))
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