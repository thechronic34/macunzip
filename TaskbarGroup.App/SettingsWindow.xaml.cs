using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using TaskbarGroup.App.Models;
using TaskbarGroup.App.Services;

namespace TaskbarGroup.App;

public partial class SettingsWindow : Window
{
    private readonly ConfigService _configService = new();
    private readonly UwpDiscoveryService _uwpDiscoveryService = new();
    private readonly ObservableCollection<AppItem> _items;

    public SettingsWindow()
    {
        InitializeComponent();

        var config = _configService.Load();
        _items = new ObservableCollection<AppItem>(config.Items);
        ItemsGrid.ItemsSource = _items;
    }

    private void AddExe_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Applications (*.exe)|*.exe|All files (*.*)|*.*"
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        _items.Add(new AppItem
        {
            DisplayName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName),
            Type = "exe",
            Path = dialog.FileName,
            Args = string.Empty,
            AppUserModelId = string.Empty
        });
    }

    private void AddUwp_OnClick(object sender, RoutedEventArgs e)
    {
        _items.Add(new AppItem
        {
            DisplayName = "Yeni UWP",
            Type = "uwp",
            AppUserModelId = "",
            Path = string.Empty,
            Args = string.Empty
        });

        MessageBox.Show("UWP satırı eklendi. Lütfen AUMID sütununu doldurun.", "Taskbar Group", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void DiscoverUwp_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var discovered = _uwpDiscoveryService.DiscoverApps();
            var existing = _items.Where(i => i.Type.Equals("uwp", StringComparison.OrdinalIgnoreCase))
                .Select(i => i.AppUserModelId)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var addedCount = 0;
            foreach (var app in discovered)
            {
                if (existing.Contains(app.AppUserModelId))
                {
                    continue;
                }

                _items.Add(app);
                addedCount++;
            }

            MessageBox.Show($"{addedCount} UWP uygulaması eklendi.", "Taskbar Group", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"UWP listesi alınamadı: {ex.Message}", "Taskbar Group", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void MoveUp_OnClick(object sender, RoutedEventArgs e)
    {
        MoveSelected(-1);
    }

    private void MoveDown_OnClick(object sender, RoutedEventArgs e)
    {
        MoveSelected(1);
    }

    private void MoveSelected(int direction)
    {
        if (ItemsGrid.SelectedItem is not AppItem selected)
        {
            return;
        }

        var index = _items.IndexOf(selected);
        var newIndex = index + direction;

        if (index < 0 || newIndex < 0 || newIndex >= _items.Count)
        {
            return;
        }

        _items.Move(index, newIndex);
        ItemsGrid.SelectedItem = selected;
        ItemsGrid.ScrollIntoView(selected);
    }

    private void Remove_OnClick(object sender, RoutedEventArgs e)
    {
        if (ItemsGrid.SelectedItem is not AppItem item)
        {
            return;
        }

        _items.Remove(item);
    }

    private void Save_OnClick(object sender, RoutedEventArgs e)
    {
        _configService.Save(new AppConfig { Items = _items.ToList() });
        MessageBox.Show("Ayarlar kaydedildi.", "Taskbar Group", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
