using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics;

internal class OpenCommand : Command<OpenCommand.Settings>
{
    public class Settings : CommandSettings, IInputFileSettings
	{
		[CommandOption("-i|--input <file>")]
		[Description("Path of input file, default value depends on operating system")]
		public string? InputFile { get; set; }
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
