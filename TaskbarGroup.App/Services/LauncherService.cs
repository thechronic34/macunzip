using System.Diagnostics;
using TaskbarGroup.App.Models;

namespace TaskbarGroup.App.Services;

public class LauncherService
{
    public void Launch(AppItem item)
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
}
