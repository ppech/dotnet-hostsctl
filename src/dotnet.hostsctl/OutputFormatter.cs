using Spectre.Console;
using System.Text.Json;

public interface IOutputFormatter
{
	void Entry(HostsFileEntry entry, bool json);
	void Entries(IEnumerable<HostsFileEntry> entries, bool json);
}

public class ConsoleOutputFormatter : IOutputFormatter
{
	private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
	{
		WriteIndented = true
	};

	public void Entry(HostsFileEntry entry, bool json)
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

	public void Entries(IEnumerable<HostsFileEntry> entries, bool json)
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

	private void PrintRow(HostsFileEntry entry)
	{
		if (entry.IsEnabled)
			AnsiConsole.MarkupLine($"  [blue]{entry.IP}[/] {entry.Hosts} [green]{entry.Comment}[/]");
		else
			AnsiConsole.MarkupLine($"[red]#[/] [grey]{entry.IP} {entry.Hosts}[/] [green]{entry.Comment}[/]");
	}

	private void PrintJson(IEnumerable<HostsFileEntry> entries)
	{
		var json = JsonSerializer.Serialize(entries, jsonOptions);
		Console.WriteLine(json);
	}
}
