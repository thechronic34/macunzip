using System.Windows;
using TaskbarGroup.App.Models;
using TaskbarGroup.App.Services;

namespace TaskbarGroup.App;

public partial class MainWindow : Window
{
    private readonly ConfigService _configService = new();
    private readonly LauncherService _launcherService = new();
    private readonly TaskbarPositionService _taskbarPositionService = new();
    private AppConfig _config = new();

    public MainWindow()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        PositionNearTaskbar();
        LoadItems();
    }

    private void PositionNearTaskbar()
    {
        var position = _taskbarPositionService.CalculatePopupTopLeft(new Size(Width, Height));
        Left = position.X;
        Top = position.Y;
    }

    private void LoadItems()
    {
        _config = _configService.Load();
        AppListBox.ItemsSource = _config.Items;
    }

    private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
    {
        var settingsWindow = new SettingsWindow
        {
            Owner = this
        };

        settingsWindow.ShowDialog();
        LoadItems();
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

    private void Window_OnDeactivated(object? sender, EventArgs e)
    {
        Close();
    }
}
