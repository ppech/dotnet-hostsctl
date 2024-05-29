using Spectre.Console;
using System.Text.Json;

public static class OutputFormatter
{
	public static void Entries(IEnumerable<HostsFileEntry> entries, bool json)
	{
		if (json)
		{
			Json(entries);
		}
		else
		{
			foreach (var entry in entries)
			{
				Entry(entry);
			}
		}
	}

	public static void Json(IEnumerable<HostsFileEntry> entries)
	{
		var json = JsonSerializer.Serialize(entries);
		Console.WriteLine(json);
	}

	public static void Entry(HostsFileEntry entry)
	{
		if (entry.IsEnabled)
			AnsiConsole.MarkupLine($"  [blue]{entry.IP}[/] {entry.Hosts} [green]{entry.Comment}[/]");
		else
			AnsiConsole.MarkupLine($"[red]#[/] [grey]{entry.IP} {entry.Hosts}[/] [green]{entry.Comment}[/]");
	}

	public static void Removed(HostsFileEntry entry)
	{
		//AnsiConsole.MarkupLine($"[red]-  {ip}[/] {host} [green]{comment}[/]");
	}
}
