using CarWashWebsite.Data.Entities;

namespace CarWashWebsite.ViewModels.Public;

public class HomeViewModel
{
    public IEnumerable<Service> FeaturedServices { get; set; } = [];
    public IEnumerable<FaqItem> TopFaqs { get; set; } = [];
    public Dictionary<string, string> Settings { get; set; } = [];
    public string CurrentCulture { get; set; } = "de";
}
