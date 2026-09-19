using CarWashWebsite.Data.Entities;

namespace CarWashWebsite.ViewModels.Admin;

public class MessagesIndexViewModel
{
    public List<ContactMessage> Messages { get; set; } = new();
    public string CurrentTab { get; set; } = "new"; // "new", "read", "archived"
    public int NewCount { get; set; }
    public int ReadCount { get; set; }
    public int ArchivedCount { get; set; }
}
