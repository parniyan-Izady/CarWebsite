using CarWashWebsite.Data.Entities;

namespace CarWashWebsite.ViewModels.Admin;

public class DashboardViewModel
{
    public int TotalServices { get; set; }
    public int ActiveServices { get; set; }
    public int TotalGalleryItems { get; set; }
    public int UnreadMessages { get; set; }
    public int TotalFaqs { get; set; }
    public List<ContactMessage> RecentMessages { get; set; } = [];
}
