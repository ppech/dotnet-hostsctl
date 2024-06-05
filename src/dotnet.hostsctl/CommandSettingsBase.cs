using Spectre.Console.Cli;
using System.ComponentModel;

public interface IInputFileSettings
{
	string? InputFile { get; set; }
}

public class HostsSettingsBase : CommandSettings, IInputFileSettings
{
	[CommandOption("-i|--input <file>")]
	[Description("Path of input file, default value depends on operating system")]
	public string? InputFile { get; set; }

	[CommandOption("-o|--output <file>")]
	[Description("Path of output file, default value is same as input file")]
	public string? OutputFile { get; set; }
}

public class HostsEntrySettingsBase : HostsSettingsBase
{
	[CommandArgument(0, "<hostname>")]
	[Description("Host name, ex. app.mydomain.local")]
	public required string HostName { get; set; }

	[CommandArgument(1, "[ip]")]
	[Description("IP address, default is 127.0.0.1")]
	public string? IP { get; set; }

	[CommandOption("-j|--json")]
	[Description("Output as JSON")]
	public bool Json { get; set; }
}

public class TemplateSettings : CommandSettings
{
	[CommandOption("-t|--template <template>")]
	[Description("Template filename, default is hosts.ht in current folder")]
	public string? TemplatePath { get; set; }
}