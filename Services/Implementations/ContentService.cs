using CarWashWebsite.Data;
using CarWashWebsite.Data.Entities;
using CarWashWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarWashWebsite.Services.Implementations;

public class ContentService : IContentService
{
    private readonly ApplicationDbContext _context;

    public ContentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FaqItem>> GetFaqsAsync()
    {
        return await _context.FaqItems
            .AsNoTracking()
            .Where(f => f.IsActive)
            .OrderBy(f => f.DisplayOrder)
            .ThenBy(f => f.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<GalleryItem>> GetGalleryItemsAsync(GalleryCategory? category = null)
    {
        var query = _context.GalleryItems
            .AsNoTracking()
            .Where(g => g.IsActive);

        if (category.HasValue)
        {
            query = query.Where(g => g.Category == category.Value);
        }

        return await query
            .OrderBy(g => g.DisplayOrder)
            .ThenByDescending(g => g.CreatedAt)
            .ToListAsync();
    }

    public async Task<Dictionary<string, string>> GetSiteSettingsAsync(string culture)
    {
        var isEn = culture.StartsWith("en", StringComparison.OrdinalIgnoreCase);
        var settings = await _context.SiteSettings.AsNoTracking().ToListAsync();

        return settings.ToDictionary(
            s => s.Key,
            s => (isEn ? s.ValueEn : s.ValueDe) ?? s.ValueDe ?? string.Empty
        );
    }
}
