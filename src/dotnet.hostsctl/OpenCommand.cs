using Spectre.Console.Cli;
using System.Diagnostics;

internal class OpenCommand : Command
{
    public override int Execute(CommandContext context)
    {
        // Start the process here
        var process = new Process();
        process.StartInfo.FileName = Utils.GetHostsFilePath();
        process.StartInfo.UseShellExecute = true;
        process.Start();

        return 0;
    }
}
