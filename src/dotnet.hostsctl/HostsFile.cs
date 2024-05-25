﻿using System.Text.RegularExpressions;

internal partial class HostsFile
{
	public static readonly Regex HostsFileEntryPattern = HostsFileEntryRegex();

	public static List<HostsFileEntry> Parse(string path)
	{
		var regex = HostsFileEntryRegex();
		var hosts = new List<HostsFileEntry>();

		var lines = File.ReadAllLines(path);
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

	[GeneratedRegex(@"^([#]{2})?(\d{1,4}\.\d{1,4}\.\d{1,4}\.\d{1,4})\s+([\w\.\s]+)([#][\w\s]+)?$", RegexOptions.Compiled)]
	private static partial Regex HostsFileEntryRegex();
}

internal record HostsFileEntry(bool IsEnabled, string IP, string Hosts, string? Comment);
