namespace TaskbarGroup.App.Models;

public class AppItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string DisplayName { get; set; } = string.Empty;
    public string Type { get; set; } = "exe"; // exe | uwp
    public string Path { get; set; } = string.Empty; // exe path
    public string Args { get; set; } = string.Empty;
    public string AppUserModelId { get; set; } = string.Empty; // uwp aumid
}
