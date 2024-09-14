using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.IO.Abstractions;

/// <summary>
/// Checks if entry exists in hosts file
/// </summary>
public class ExistsCommand : Command<ExistsCommand.Settings>
{
	private readonly IFileSystem fileSystem;
	private readonly IHostsFile hostsFile;
	private readonly IOutputFormatter outputFormatter;

	public class Settings : HostsEntrySettingsBase
	{
		[CommandOption("-a|--all")]
		[Description("Include disabled entries")]
		public bool All { get; set; }
	}

	public ExistsCommand(IFileSystem fileSystem, IHostsFile hostsFile, IOutputFormatter outputFormatter)
	{
		this.fileSystem = fileSystem;
		this.hostsFile = hostsFile;
		this.outputFormatter = outputFormatter;
	}

    public override int Execute(CommandContext context, Settings settings)
    {
        var inputFilePath = Utils.GetInputFilePath(settings);

		var inputFile = fileSystem.FileInfo.New(inputFilePath);

        var entries = hostsFile.Parse(inputFile)
			.Where(p => p.Hosts.Contains(settings.HostName, StringComparison.OrdinalIgnoreCase));

		if (settings.IP is not null)
			entries = entries.Where(p => p.IP.Equals(settings.IP));

		if(!settings.All)
			entries = entries.Where(p => p.IsEnabled);

		if (!entries.Any())
		{
			AnsiConsole.MarkupLine($"[red]Entry with HostName '{settings.HostName}' and IP '{settings.IP}' not found[/]");
			return -1;
		}

		outputFormatter.Entries(entries, settings.Json);

		return 0;
	}
}