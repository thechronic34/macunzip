using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using TaskbarGroup.App.Models;
using TaskbarGroup.App.Services;

namespace TaskbarGroup.App;

public partial class SettingsWindow : Window
{
    private readonly ConfigService _configService = new();
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
