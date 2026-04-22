using System.Diagnostics;
using TaskbarGroup.App.Models;

namespace TaskbarGroup.App.Services;

public class LauncherService
{
    public void Launch(AppItem item)
    {
        if (item.Type.Equals("uwp", StringComparison.OrdinalIgnoreCase))
        {
            LaunchUwp(item);
            return;
        }

        LaunchExe(item);
    }

    private static void LaunchExe(AppItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Path))
        {
            throw new InvalidOperationException("Uygulama yolu boş olamaz.");
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = item.Path,
            Arguments = item.Args,
            UseShellExecute = true
        };

        Process.Start(startInfo);
    }

    private static void LaunchUwp(AppItem item)
    {
        if (string.IsNullOrWhiteSpace(item.AppUserModelId))
        {
            throw new InvalidOperationException("UWP için AppUserModelId boş olamaz.");
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = "explorer.exe",
            Arguments = $"shell:AppsFolder\\{item.AppUserModelId}",
            UseShellExecute = true
        };

        Process.Start(startInfo);
    }
}
