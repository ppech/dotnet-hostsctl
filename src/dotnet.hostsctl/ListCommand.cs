using Spectre.Console.Cli;

/// <summary>
/// List the entries in the hosts file.
/// </summary>
public class ListCommand : Command<ListCommand.Settings>
{
	public class Settings : SettingsBase
	{

	}

	public override int Execute(CommandContext context, Settings settings)
	{
        var inputFilePath = Utils.GetInputFilePath(settings);

        var entries = HostsFile.Parse(inputFilePath);

		OutputFormatter.Entries(entries, settings.Json);

		return 0;
	}
}
