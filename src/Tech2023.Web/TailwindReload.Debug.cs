using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Tech2023.Web;

internal static class TailwindReload
{
    internal static void Run()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = GetShellName(),
            Arguments = "npm run css:dev",
            CreateNoWindow = true
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string GetShellName()
    {
        if (OperatingSystem.IsWindows())
        {
            return "powershell";
        }
        else if (OperatingSystem.IsLinux())
        {
            return "bash";
        }

        throw new InvalidOperationException("Development with tailwind reload is not supported on your platform");
    }
}