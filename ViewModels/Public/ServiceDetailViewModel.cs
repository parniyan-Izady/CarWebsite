using CarWashWebsite.Data.Entities;

namespace CarWashWebsite.ViewModels.Public;

public class ServiceDetailViewModel
{
    public Service Service { get; set; } = null!;
    public IEnumerable<Service> OtherServices { get; set; } = [];
    public string CurrentCulture { get; set; } = "de";
    public string GermanSlug { get; set; } = string.Empty;
    public string EnglishSlug { get; set; } = string.Empty;
}
