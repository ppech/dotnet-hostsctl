using Spectre.Console.Cli;
using System.Diagnostics;

internal class OpenCommand : Command<OpenCommand.Settings>
{
    public class Settings : SettingsBase
    {
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        // Start the process here
        var process = new Process();
        process.StartInfo.FileName = Utils.GetInputFilePath(settings);
        process.StartInfo.UseShellExecute = true;
        process.Start();

        return 0;
    }
}
