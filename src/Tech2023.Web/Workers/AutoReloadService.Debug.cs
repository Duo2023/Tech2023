﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Tech2023.Web.Workers;

/// <summary>
/// Launches processes during debug that handles reloading files at runtime for easier code development
/// </summary>
public class AutoReloadService : IHostedService
{
    internal readonly ILogger<AutoReloadService> _logger;
    internal readonly string[] _npmScripts = new string[] { "css:dev-reload", "build:dev-reload" }; // See: ../package.json
    internal readonly Process[] _processes;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoReloadService"/> class
    /// </summary>
    /// <param name="logger">Logger that logs when a process launches and closes</param>
    public AutoReloadService(ILogger<AutoReloadService> logger)
    {
        _logger = logger;
        _processes = new Process[_npmScripts.Length];
    }

    /// <summary>
    /// Launches the processes used for hot reload
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Debug.Assert(_npmScripts.Length == _processes.Length);

        if (TryGetNpmFileName(out var file))
        {
            _logger.LogInformation("Starting Npm reload scripts");

            for (int i = 0; i < _npmScripts.Length; i++)
            {
                _processes[i] = Process.Start(GetNpmRunStartInfo(file, "run " + _npmScripts[i]))!;
            }
        }
        else
        {
            _logger.LogWarning("npm reload scripts is not supported on your platform : {os}", Environment.OSVersion);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Kills all the associated processes that were created at the start of the applications life
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Closing npm reload script processes");
        foreach (var process in _processes)
        {
            process.CloseMainWindow();
            process.Kill();
        }

        return Task.CompletedTask;
    }

    internal static ProcessStartInfo GetNpmRunStartInfo(string file, string args)
    {
        return new()
        {
            FileName = file,
            Arguments = args,
            CreateNoWindow = true,
            UseShellExecute = true,
        };
    }

    internal static bool TryGetNpmFileName([NotNullWhen(true)] out string? file)
    {
        if (OperatingSystem.IsWindows())
        {
            file = "npm.cmd";
            return true;
        }
        else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
        {
            file = "npm";
            return true;
        }
        else
        {
            file = null;
            return false;
        }
    }
}
