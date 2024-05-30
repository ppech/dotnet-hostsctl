using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Text.Json;

/// <summary>
/// Checks if entry exists in hosts file
/// </summary>
public class ExistsCommand : Command<ExistsCommand.Settings>
{
    public class Settings : SettingsBase
    {
		[CommandArgument(0, "<hostname>")]
		[Description("Host name, ex. app.mydomain.local")]
		public required string HostName { get; set; }

		[CommandArgument(1, "[ip]")]
		[Description("IP address, default is 127.0.0.1")]
		public string? IP { get; set; }

		[CommandOption("-a|--all")]
		public bool All { get; set; }
	}

    public override int Execute(CommandContext context, Settings settings)
    {
        var inputFilePath = Utils.GetInputFilePath(settings);

        var entries = HostsFile.Parse(inputFilePath)
			.Where(p => p.Hosts.Contains(settings.HostName, StringComparison.OrdinalIgnoreCase));

		if (settings.IP is not null)
			entries = entries.Where(p => p.IP.Equals(settings.IP));

		if(!settings.All)
			entries = entries.Where(p => p.IsEnabled);

		OutputFormatter.Entries(entries, settings.Json);

		return entries.Any() ? 0 : -1;
	}
}