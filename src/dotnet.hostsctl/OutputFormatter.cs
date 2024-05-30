using Spectre.Console;
using System.Text.Json;

public static class OutputFormatter
{
	private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
	{
		WriteIndented = true
	};

	public static void Entry(HostsFileEntry entry, bool json)
	{
		if (json)
		{
			PrintJson([entry]);
		}
		else
		{
			PrintRow(entry);
		}
	}

	public static void Entries(IEnumerable<HostsFileEntry> entries, bool json)
	{
		if (json)
		{
			PrintJson(entries);
		}
		else
		{
			foreach (var entry in entries)
			{
				PrintRow(entry);
			}
		}
	}

	private static void PrintRow(HostsFileEntry entry)
	{
		if (entry.IsEnabled)
			AnsiConsole.MarkupLine($"  [blue]{entry.IP}[/] {entry.Hosts} [green]{entry.Comment}[/]");
		else
			AnsiConsole.MarkupLine($"[red]#[/] [grey]{entry.IP} {entry.Hosts}[/] [green]{entry.Comment}[/]");
	}

	private static void PrintJson(IEnumerable<HostsFileEntry> entries)
	{
		var json = JsonSerializer.Serialize(entries, jsonOptions);
		Console.WriteLine(json);
	}
}
