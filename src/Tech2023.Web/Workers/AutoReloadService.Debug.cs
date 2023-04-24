using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Tech2023.Web.Workers;

/// <summary>
/// Launches processes during debug that handles reloading files at runtime for easier code development
/// </summary>
public class AutoReloadService : IHostedService
{
    internal readonly ILogger<AutoReloadService> _logger;
    internal readonly List<Process> _processes = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoReloadService"/> class
    /// </summary>
    /// <param name="logger">Logger that logs when a process launches and closes</param>
    public AutoReloadService(ILogger<AutoReloadService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Launches the processes used for hot reload
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (TryGetTailwindReloadArgs(out var file, out var args))
        {
            _logger.LogInformation("Starting tailwind reload");
            _processes.Add(Process.Start(GetTailwindStartInfo(file, args))!);
        }
        else
        {
            _logger.LogWarning("Tailwind reload is not supported on your platform: {os}", Environment.OSVersion);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Kills all the associated processes that were created at the start of the applications life
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var process in _processes)
        {
            _logger.LogInformation("Killing tailwind reload process");
            process.Kill();
        }

        return Task.CompletedTask;
    }

    internal static ProcessStartInfo GetTailwindStartInfo(string file, string args)
    {
        return new()
        {
            FileName = file,
            Arguments = args,
            CreateNoWindow = true,
            UseShellExecute = true,
        };
    }

    internal static bool TryGetTailwindReloadArgs([NotNullWhen(true)] out string? file, [NotNullWhen(true)] out string? args)
    {
        if (OperatingSystem.IsWindows())
        {
            file = "powershell";
            args = "npm run css:dev";
            return true;
        }
        else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
        {
            file = "npm";
            args = "run css:dev";
            return true;
        }
        else
        {
            file = null;
            args = null;
            return false;
        }
    }
}
