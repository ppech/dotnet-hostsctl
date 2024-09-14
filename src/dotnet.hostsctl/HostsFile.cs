using System.IO.Abstractions;
using System.Text.RegularExpressions;

public interface IHostsFile
{
	void Append(IFileInfo file, HostsFileEntry entry);
	List<HostsFileEntry> Parse(IFileInfo fileInfo);
	void Process(IFileInfo inputFile, IFileInfo outputFile, Func<HostsFileEntry, HostsFileEntry?> processor);
}

public partial class HostsFile : IHostsFile
{
	public static readonly Regex HostsFileEntryPattern = HostsFileEntryRegex();

	public void Append(IFileInfo file, HostsFileEntry entry)
	{
		// append the entry to the end of the file
		file.AppendAllLines(
			[
				$"{(entry.IsEnabled ? "" : "##")}{entry.IP} {entry.Hosts}{(string.IsNullOrWhiteSpace(entry.Comment) ? "" : $" #{entry.Comment}")}"
			]
		);
	}

	public List<HostsFileEntry> Parse(IFileInfo fileInfo)
	{
		var regex = HostsFileEntryRegex();
		var hosts = new List<HostsFileEntry>();

		var lines = fileInfo.ReadAllLines();
		foreach (var line in lines)
		{
			var m = regex.Match(line);

			if (!m.Success)
				continue;

			var entry = new HostsFileEntry(
				!m.Groups[1].Success,
				m.Groups[2].Value.Trim(),
				m.Groups[3].Value.Trim(),
				m.Groups[4].Success ? m.Groups[4].Value.Substring(1).Trim() : null);

			hosts.Add(entry);
		}

		return hosts;
	}

	public void Process(IFileInfo inputFile, IFileInfo outputFile, Func<HostsFileEntry, HostsFileEntry?> processor)
	{
		var regex = HostsFileEntryRegex();
		var lines = inputFile.ReadAllLines();
		var hosts = new List<string>();

		foreach (var line in lines)
		{
			var m = regex.Match(line);

			if (!m.Success)
			{
				hosts.Add(line);
				continue;
			}

			var entry = new HostsFileEntry(
				!m.Groups[1].Success,
				m.Groups[2].Value.Trim(),
				m.Groups[3].Value.Trim(),
				m.Groups[4].Success ? m.Groups[4].Value.Substring(1).Trim() : null);

			var processed = processor(entry);

			if (processed is not null)
				hosts.Add($"{(processed.IsEnabled ? "" : "##")}{processed.IP} {processed.Hosts}{(processed.Comment != null ? $" #{processed.Comment}" : "")}");
		}

		outputFile.WriteAllLines(hosts);
	}

	[GeneratedRegex(@"^([#]{2})?(\d{1,4}\.\d{1,4}\.\d{1,4}\.\d{1,4})\s+([\w\.\s]+)([#][\w\s]+)?$", RegexOptions.Compiled)]
	private static partial Regex HostsFileEntryRegex();
}

public record HostsFileEntry(bool IsEnabled, string IP, string Hosts, string? Comment);
