using System.Threading;
using System.Windows;

namespace TaskbarGroup.App;

public partial class App : Application
{
    private static Mutex? _singleInstanceMutex;

    protected override void OnStartup(StartupEventArgs e)
    {
        const string mutexName = "TaskbarGroupAppSingletonMutex";
        _singleInstanceMutex = new Mutex(true, mutexName, out bool createdNew);

        if (!createdNew)
        {
            Shutdown();
            return;
        }

        base.OnStartup(e);

        var openSettings = e.Args.Any(a => a.Equals("--settings", StringComparison.OrdinalIgnoreCase));

        if (openSettings)
        {
            var settingsWindow = new SettingsWindow();
            MainWindow = settingsWindow;
            settingsWindow.Show();
            return;
        }

        var mainWindow = new MainWindow();
        MainWindow = mainWindow;
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _singleInstanceMutex?.ReleaseMutex();
        _singleInstanceMutex?.Dispose();
        base.OnExit(e);
    }
}
