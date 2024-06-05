using System.Security.Principal;

internal static class Utils
{
    public static bool IsRunningWithElevatedPrivileges()
    {
        if (OperatingSystem.IsWindows())
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        if (OperatingSystem.IsLinux())
        {
            return Environment.GetEnvironmentVariable("SUDO_USER") != null;
        }

        throw new PlatformNotSupportedException();
    }

    public static string GetInputFilePath(IInputFileSettings settings, string suffix = "")
	{
		if (settings.InputFile != null)
		{
			return settings.InputFile;
		}

		return GetSystemFilePath(suffix);
	}

	public static string GetOutputFilePath(HostsSettingsBase settings, string suffix = "")
    {
        if (settings.OutputFile != null)
            return settings.OutputFile;

        return GetInputFilePath(settings) + suffix;
    }

	public static string GetSystemFilePath(string suffix = "")
	{
		if (OperatingSystem.IsWindows())
		{
			var windir = Environment.GetEnvironmentVariable("windir");
			return windir + @"\System32\drivers\etc\hosts" + suffix;
		}

		if (OperatingSystem.IsLinux())
		{
			return "/etc/hosts" + suffix;
		}

		throw new PlatformNotSupportedException();
	}
}
