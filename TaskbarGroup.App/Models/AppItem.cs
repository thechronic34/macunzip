namespace TaskbarGroup.App.Models;

public class AppItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string DisplayName { get; set; } = string.Empty;
    public string Type { get; set; } = "exe";
    public string Path { get; set; } = string.Empty;
    public string Args { get; set; } = string.Empty;
}
