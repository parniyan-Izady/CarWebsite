using CarWashWebsite.Data;
using CarWashWebsite.Data.Entities;
using CarWashWebsite.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashWebsite.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("Admin/Seo")]
public class SeoController : Controller
{
    private readonly ApplicationDbContext _context;

    private static readonly List<(string Key, string Name, string PathDe, string PathEn)> DefinedPages = new()
    {
        ("home", "Home Page", "/de", "/en"),
        ("services", "Services Overview", "/de/services", "/en/services"),
        ("gallery", "Before / After Gallery", "/de/gallery", "/en/gallery"),
        ("about", "About Us", "/de/about", "/en/about"),
        ("faq", "FAQ", "/de/faq", "/en/faq"),
        ("contact", "Contact & Booking", "/de/contact", "/en/contact")
    };

    public SeoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(string page = "home")
    {
        var normalizedPage = page.ToLowerInvariant();
        if (!DefinedPages.Any(p => p.Key == normalizedPage))
        {
            normalizedPage = "home";
        }

        var seoItems = await _context.PageContents
            .Where(p => p.PageKey.StartsWith("SEO_"))
            .ToListAsync();

        var dict = seoItems.ToDictionary(p => p.PageKey, p => p);

        var pageList = DefinedPages.Select(p =>
        {
            var dbKey = $"SEO_{p.Key}";
            dict.TryGetValue(dbKey, out var item);
            return new PageSeoItemViewModel
            {
                PageKey = p.Key,
                PageName = p.Name,
                PathDe = p.PathDe,
                PathEn = p.PathEn,
                SeoTitleDe = item?.TitleDe ?? $"{p.Name} | Berlin Car Care",
                SeoTitleEn = item?.TitleEn ?? $"{p.Name} | Berlin Car Care",
                MetaDescriptionDe = item?.ContentDe ?? $"Professionelle Autopflege & Detailing in Berlin - {p.Name}.",
                MetaDescriptionEn = item?.ContentEn ?? $"Expert automotive detailing and car care in Berlin - {p.Name}."
            };
        }).ToList();

        var current = pageList.First(p => p.PageKey == normalizedPage);

        var model = new SeoManagementViewModel
        {
            Pages = pageList,
            SelectedPageKey = normalizedPage,
            CurrentPage = current,
            SitemapUrl = "/sitemap.xml",
            RobotsUrl = "/robots.txt"
        };

        return View(model);
    }

    [HttpPost("Save")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(PageSeoItemViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.PageKey))
        {
            return BadRequest();
        }

        var dbKey = $"SEO_{model.PageKey.ToLowerInvariant()}";
        var existing = await _context.PageContents.FirstOrDefaultAsync(p => p.PageKey == dbKey && p.SectionKey == "Metadata");

        if (existing != null)
        {
            existing.TitleDe = model.SeoTitleDe;
            existing.TitleEn = model.SeoTitleEn;
            existing.ContentDe = model.MetaDescriptionDe;
            existing.ContentEn = model.MetaDescriptionEn;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var newItem = new PageContent
            {
                PageKey = dbKey,
                SectionKey = "Metadata",
                TitleDe = model.SeoTitleDe,
                TitleEn = model.SeoTitleEn,
                ContentDe = model.MetaDescriptionDe,
                ContentEn = model.MetaDescriptionEn,
                UpdatedAt = DateTime.UtcNow
            };
            await _context.PageContents.AddAsync(newItem);
        }

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"SEO settings for {model.PageKey.ToUpper()} saved successfully.";
        return RedirectToAction(nameof(Index), new { page = model.PageKey });
    }
}
