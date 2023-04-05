using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Tech2023.Web;

internal static class TailwindReload
{
    internal static void Run()
    {
        if (OperatingSystem.IsWindows())
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "npm run css:dev",
                CreateNoWindow = true
            });
        }
        else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "npm",
                Arguments = "run css:dev",
                CreateNoWindow = true
            });
        }
        else
        {
            throw new InvalidOperationException("Development with tailwind reload is not supported on your platform");
        }
    }
}