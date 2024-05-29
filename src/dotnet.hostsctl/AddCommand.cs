using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

internal class AddCommand : Command<AddCommand.Settings>
{
	public class Settings : InOutSettingsBase
	{
		[CommandArgument(0, "<hostname>")]
		[Description("Host name, ex. app.mydomain.local")]
		public required string HostName { get; set; }

		[CommandArgument(1, "[ip]")]
		[Description("IP address, default is 127.0.0.1")]
		public string? IP { get; set; }

		[CommandArgument(2, "[comment]")]
		[Description("Comment for the entry")]
		public string? Comment { get; set; }
	}

	public override int Execute(CommandContext context, Settings settings)
	{
		var entry = new HostsFileEntry
		(
			IsEnabled: true,
			IP: settings.IP ?? "127.0.0.1",
			Hosts: settings.HostName,
			Comment: settings.Comment
		);

        var inputFilePath = Utils.GetInputFilePath(settings);
        var outputFilePath = Utils.GetOutputFilePath(settings);

        var entries = HostsFile.Parse(inputFilePath);
		
		// check if the entry with hosts and ip already exists
		if (entries.Any(p => p.Hosts.Equals(entry.Hosts, StringComparison.OrdinalIgnoreCase) && p.IP.Equals(entry.IP)))
		{
			AnsiConsole.MarkupLine($"[red]Entry with hosts '{entry.Hosts}' and IP '{entry.IP}' already exists[/]");
			return -1;
		}

		HostsFile.Append(outputFilePath, entry);

		return 0;
	}
}