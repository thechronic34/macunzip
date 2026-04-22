using System.IO;
using System.Text.Json;
using TaskbarGroup.App.Models;

namespace TaskbarGroup.App.Services;

public class ConfigService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public string ConfigPath { get; }
    public string BackupPath { get; }

    public ConfigService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var configDir = Path.Combine(appData, "TaskbarGroup");
        Directory.CreateDirectory(configDir);

        ConfigPath = Path.Combine(configDir, "config.json");
        BackupPath = Path.Combine(configDir, "config.backup.json");
    }

    public AppConfig Load()
    {
        if (!File.Exists(ConfigPath))
        {
            var defaultConfig = CreateDefaultConfig();
            Save(defaultConfig);
            return defaultConfig;
        }

        try
        {
            var json = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize<AppConfig>(json, JsonOptions) ?? new AppConfig();
        }
        catch
        {
            TryRestoreFromBackup();
            return LoadBackupOrEmpty();
        }
    }

    public void Save(AppConfig config)
    {
        var json = JsonSerializer.Serialize(config, JsonOptions);

        if (File.Exists(ConfigPath))
        {
            File.Copy(ConfigPath, BackupPath, overwrite: true);
        }

        File.WriteAllText(ConfigPath, json);
    }

    private AppConfig LoadBackupOrEmpty()
    {
        try
        {
            if (!File.Exists(BackupPath))
            {
                return new AppConfig();
            }

            var json = File.ReadAllText(BackupPath);
            return JsonSerializer.Deserialize<AppConfig>(json, JsonOptions) ?? new AppConfig();
        }
        catch
        {
            return new AppConfig();
        }
    }

    private void TryRestoreFromBackup()
    {
        if (!File.Exists(BackupPath))
        {
            return;
        }

        try
        {
            File.Copy(BackupPath, ConfigPath, overwrite: true);
        }
        catch
        {
            // intentionally ignored for MVP
        }
    }

    private static AppConfig CreateDefaultConfig() => new()
    {
        Items =
        [
            new AppItem
            {
                Id = "notepad",
                DisplayName = "Notepad",
                Type = "exe",
                Path = @"C:\\Windows\\System32\\notepad.exe",
                Args = string.Empty
            }
        ]
    };
}
