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

    public static string GetHostsFilePath()
    {
        return "../samples/hosts";

        if (OperatingSystem.IsWindows())
        {
            var windir = Environment.GetEnvironmentVariable("windir");
            return windir + @"\System32\drivers\etc\hosts";
        }

        if (OperatingSystem.IsLinux())
        {
            return "/etc/hosts";
        }

        throw new PlatformNotSupportedException();
    }
}
