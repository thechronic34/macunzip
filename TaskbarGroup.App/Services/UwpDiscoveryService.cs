using System.Diagnostics;
using TaskbarGroup.App.Models;

namespace TaskbarGroup.App.Services;

public class UwpDiscoveryService
{
    public List<AppItem> DiscoverApps()
    {
        var command = "Get-StartApps | Sort-Object Name | ForEach-Object { \"$($_.Name)|$($_.AppID)\" }";
        var startInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException("PowerShell başlatılamadı.");
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"UWP uygulamaları alınamadı: {error}");
        }

        var result = new List<AppItem>();
        var lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var parts = line.Split('|', 2, StringSplitOptions.TrimEntries);
            if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[1]))
            {
                continue;
            }

            result.Add(new AppItem
            {
                DisplayName = parts[0],
                Type = "uwp",
                AppUserModelId = parts[1],
                Path = string.Empty,
                Args = string.Empty
            });
        }

        return result;
    }
}
