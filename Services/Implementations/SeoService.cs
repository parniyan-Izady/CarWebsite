using CarWashWebsite.Data;
using CarWashWebsite.Data.Entities;
using CarWashWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace CarWashWebsite.Services.Implementations;

public class SeoService : ISeoService
{
    private readonly ApplicationDbContext _context;

    public SeoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateSitemapXmlAsync(string baseUrl)
    {
        var cleanBaseUrl = baseUrl.TrimEnd('/');
        var services = await _context.Services
            .AsNoTracking()
            .Where(s => s.IsActive)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\">");

        // Static core routes for DE and EN
        var coreRoutes = new[] { "", "services", "gallery", "about", "faq", "contact", "home/impressum", "home/privacy" };

        foreach (var route in coreRoutes)
        {
            var suffix = string.IsNullOrEmpty(route) ? "" : "/" + route;
            var deLoc = $"{cleanBaseUrl}/de{suffix}";
            var enLoc = $"{cleanBaseUrl}/en{suffix}";

            AddUrlElement(sb, deLoc, enLoc, "weekly", "0.9");
            AddUrlElement(sb, enLoc, deLoc, "weekly", "0.8");
        }

        // Dynamic Service Detail Pages
        foreach (var service in services)
        {
            var deLoc = $"{cleanBaseUrl}/de/services/{service.SlugDe}";
            var enLoc = $"{cleanBaseUrl}/en/services/{service.SlugEn}";

            AddUrlElement(sb, deLoc, enLoc, "monthly", "1.0");
            AddUrlElement(sb, enLoc, deLoc, "monthly", "0.9");
        }

        sb.AppendLine("</urlset>");
        return sb.ToString();
    }

    private static void AddUrlElement(StringBuilder sb, string loc, string alternateLoc, string changeFreq, string priority)
    {
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>{loc}</loc>");
        sb.AppendLine($"    <xhtml:link rel=\"alternate\" hreflang=\"de\" href=\"{(loc.Contains("/de/") ? loc : alternateLoc)}\" />");
        sb.AppendLine($"    <xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"{(loc.Contains("/en/") ? loc : alternateLoc)}\" />");
        sb.AppendLine($"    <changefreq>{changeFreq}</changefreq>");
        sb.AppendLine($"    <priority>{priority}</priority>");
        sb.AppendLine("  </url>");
    }

    public string GenerateRobotsTxt(string baseUrl)
    {
        var cleanBaseUrl = baseUrl.TrimEnd('/');
        var sb = new StringBuilder();
        sb.AppendLine("User-agent: *");
        sb.AppendLine("Disallow: /admin/");
        sb.AppendLine("Disallow: /Admin/");
        sb.AppendLine("Allow: /");
        sb.AppendLine();
        sb.AppendLine($"Sitemap: {cleanBaseUrl}/sitemap.xml");
        return sb.ToString();
    }

    public string GenerateLocalBusinessSchema(string baseUrl, string culture, Dictionary<string, string> settings)
    {
        var cleanBaseUrl = baseUrl.TrimEnd('/');
        var isEn = culture.StartsWith("en", StringComparison.OrdinalIgnoreCase);

        var schemaObj = new
        {
            context = "https://schema.org",
            type = "AutoRepair",
            name = settings.GetValueOrDefault("BusinessName", "Berlin Car Care"),
            description = isEn
                ? "Certified automotive detailing, paint polishing, interior cleaning, and pre-sale preparation in Berlin."
                : "Zertifizierte Fahrzeugaufbereitung, Lackpolitur, Tiefenreinigung und Verkaufsvorbereitung in Berlin.",
            url = $"{cleanBaseUrl}/{culture}",
            telephone = settings.GetValueOrDefault("Phone", "+49 30 12345678"),
            email = settings.GetValueOrDefault("Email", "info@berlincarcare.de"),
            address = new
            {
                type = "PostalAddress",
                streetAddress = "Kurfürstendamm 120",
                addressLocality = "Berlin",
                postalCode = "10711",
                addressCountry = "DE"
            },
            geo = new
            {
                type = "GeoCoordinates",
                latitude = 52.4986,
                longitude = 13.3039
            },
            openingHoursSpecification = new[]
            {
                new
                {
                    type = "OpeningHoursSpecification",
                    dayOfWeek = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" },
                    opens = "08:00",
                    closes = "18:00"
                }
            },
            priceRange = "€€"
        };

        return JsonSerializer.Serialize(schemaObj, new JsonSerializerOptions { WriteIndented = false });
    }

    public string GenerateServiceSchema(string baseUrl, string culture, Service service)
    {
        var cleanBaseUrl = baseUrl.TrimEnd('/');
        var isEn = culture.StartsWith("en", StringComparison.OrdinalIgnoreCase);
        var title = isEn ? service.TitleEn : service.TitleDe;
        var desc = isEn ? service.DescriptionEn : service.DescriptionDe;
        var slug = isEn ? service.SlugEn : service.SlugDe;

        var schemaObj = new
        {
            context = "https://schema.org",
            type = "Service",
            name = title,
            description = desc,
            provider = new
            {
                type = "AutoRepair",
                name = "Berlin Car Care",
                url = cleanBaseUrl
            },
            areaServed = new
            {
                type = "City",
                name = "Berlin"
            },
            offers = new
            {
                type = "Offer",
                price = service.Price ?? 0,
                priceCurrency = "EUR",
                url = $"{cleanBaseUrl}/{culture}/services/{slug}"
            }
        };

        return JsonSerializer.Serialize(schemaObj, new JsonSerializerOptions { WriteIndented = false });
    }
}
