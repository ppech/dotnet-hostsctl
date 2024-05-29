using Spectre.Console.Cli;
using System.ComponentModel;

public class InSettingsBase : CommandSettings
{
    [CommandOption("-i|--input <file>")]
    [Description("Path of input file. For windows is default value '%windir%\\System32\\drivers\\etc\\hosts' and for linux is '/etc/hosts'")]

    public string? InputFile { get; set; }
}

public class InOutSettingsBase : InSettingsBase
{
    [CommandOption("-o|--output <file>")]
    [Description("Path of output file. Default value is same as --input option")]
    public string? OutputFile { get; set; }
}