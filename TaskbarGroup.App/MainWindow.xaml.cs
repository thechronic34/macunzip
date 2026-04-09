using System.Windows;
using TaskbarGroup.App.Models;
using TaskbarGroup.App.Services;

namespace TaskbarGroup.App;

public partial class MainWindow : Window
{
    private readonly ConfigService _configService = new();
    private readonly LauncherService _launcherService = new();
    private AppConfig _config = new();

    public MainWindow()
    {
        InitializeComponent();
        PositionNearTaskbar();
        LoadItems();
    }

    private void PositionNearTaskbar()
    {
        // Basit MVP yaklaşımı: ekranın sağ alt kısmına yakın konumlandır.
        var workArea = SystemParameters.WorkArea;
        Left = workArea.Right - Width - 16;
        Top = workArea.Bottom - Height - 16;
    }

    private void LoadItems()
    {
        _config = _configService.Load();
        AppListBox.ItemsSource = _config.Items;
    }

    private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
    {
        LoadItems();
    }

    private void LaunchButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (AppListBox.SelectedItem is not AppItem selected)
        {
            MessageBox.Show("Lütfen bir uygulama seçin.", "Taskbar Group", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        try
        {
            _launcherService.Launch(selected);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Uygulama başlatılamadı: {ex.Message}", "Taskbar Group", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
