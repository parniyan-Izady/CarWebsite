using CarWashWebsite.Data;
using CarWashWebsite.Data.Entities;
using CarWashWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarWashWebsite.Services.Implementations;

public class ServiceService : IServiceService
{
    private readonly ApplicationDbContext _context;
    private readonly IMediaService _mediaService;
    private readonly ILogger<ServiceService> _logger;

    public ServiceService(ApplicationDbContext context, IMediaService mediaService, ILogger<ServiceService> logger)
    {
        _context = context;
        _mediaService = mediaService;
        _logger = logger;
    }

    public async Task<IEnumerable<Service>> GetAllServicesAsync(bool onlyActive = false)
    {
        var query = _context.Services.AsNoTracking();

        if (onlyActive)
        {
            query = query.Where(s => s.IsActive);
        }

        return await query.OrderBy(s => s.DisplayOrder).ThenBy(s => s.Id).ToListAsync();
    }

    public async Task<Service?> GetServiceByIdAsync(int id)
    {
        return await _context.Services.FindAsync(id);
    }

    public async Task<Service?> GetServiceBySlugAsync(string slug, string culture)
    {
        var lowerSlug = slug.ToLowerInvariant().Trim();
        var query = _context.Services.AsNoTracking().Where(s => s.IsActive);

        if (culture.StartsWith("en", StringComparison.OrdinalIgnoreCase))
        {
            return await query.FirstOrDefaultAsync(s => s.SlugEn == lowerSlug);
        }

        return await query.FirstOrDefaultAsync(s => s.SlugDe == lowerSlug);
    }

    public async Task<Service> CreateServiceAsync(Service service)
    {
        service.CreatedAt = DateTime.UtcNow;
        service.SlugDe = GenerateSlug(string.IsNullOrWhiteSpace(service.SlugDe) ? service.TitleDe : service.SlugDe);
        service.SlugEn = GenerateSlug(string.IsNullOrWhiteSpace(service.SlugEn) ? service.TitleEn : service.SlugEn);

        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return service;
    }

    public async Task<Service?> UpdateServiceAsync(Service updatedService)
    {
        var existing = await _context.Services.FindAsync(updatedService.Id);
        if (existing == null)
        {
            return null;
        }

        existing.TitleDe = updatedService.TitleDe;
        existing.TitleEn = updatedService.TitleEn;
        existing.DescriptionDe = updatedService.DescriptionDe;
        existing.DescriptionEn = updatedService.DescriptionEn;
        existing.Price = updatedService.Price;
        existing.PriceType = updatedService.PriceType;
        existing.ImageAltDe = updatedService.ImageAltDe;
        existing.ImageAltEn = updatedService.ImageAltEn;
        existing.DisplayOrder = updatedService.DisplayOrder;
        existing.IsActive = updatedService.IsActive;
        existing.SeoTitleDe = updatedService.SeoTitleDe;
        existing.SeoTitleEn = updatedService.SeoTitleEn;
        existing.MetaDescriptionDe = updatedService.MetaDescriptionDe;
        existing.MetaDescriptionEn = updatedService.MetaDescriptionEn;
        existing.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(updatedService.ImagePath))
        {
            existing.ImagePath = updatedService.ImagePath;
        }

        if (!string.IsNullOrWhiteSpace(updatedService.SlugDe))
        {
            existing.SlugDe = GenerateSlug(updatedService.SlugDe);
        }

        if (!string.IsNullOrWhiteSpace(updatedService.SlugEn))
        {
            existing.SlugEn = GenerateSlug(updatedService.SlugEn);
        }

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteServiceAsync(int id)
    {
        var existing = await _context.Services.FindAsync(id);
        if (existing == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(existing.ImagePath))
        {
            await _mediaService.DeleteImageAsync(existing.ImagePath);
        }

        _context.Services.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleServiceStatusAsync(int id)
    {
        var existing = await _context.Services.FindAsync(id);
        if (existing == null)
        {
            return false;
        }

        existing.IsActive = !existing.IsActive;
        existing.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return existing.IsActive;
    }

    private static string GenerateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        var slug = text.ToLowerInvariant()
            .Replace("ä", "ae")
            .Replace("ö", "oe")
            .Replace("ü", "ue")
            .Replace("ß", "ss")
            .Replace("&", "and")
            .Replace("/", "-")
            .Replace(" ", "-");

        var chars = slug.Where(c => char.IsLetterOrDigit(c) || c == '-').ToArray();
        return new string(chars).Trim('-');
    }
}
