using CarWashWebsite.Data.Entities;

namespace CarWashWebsite.Services.Interfaces;

public interface IContentService
{
    Task<IEnumerable<FaqItem>> GetFaqsAsync();
    Task<IEnumerable<GalleryItem>> GetGalleryItemsAsync(GalleryCategory? category = null);
    Task<Dictionary<string, string>> GetSiteSettingsAsync(string culture);
}
