using System.Text.RegularExpressions;

internal partial class HostsFile
{
    public static readonly Regex HostsFileEntryPattern = HostsFileEntryRegex();

    public static void Read(string path, Action<string> action)
    {
        var lines = File.ReadAllLines(path);
        foreach (var line in lines)
        {
            action(line);
        }
    }

    public static void Process(string path, Func<string, string> action)
    {
        var lines = File.ReadAllLines(path);
        using var sw = new StreamWriter(path, false);
        foreach (var line in lines)
        {
            var result = action(line);
            sw.WriteLine(result);
        }
    }


    [GeneratedRegex(@"^([#]{2})?(\d{1,4}\.\d{1,4}\.\d{1,4}\.\d{1,4})\s+([\w\. ]+)([#]\w+)?$", RegexOptions.Compiled)]
    private static partial Regex HostsFileEntryRegex();
}
