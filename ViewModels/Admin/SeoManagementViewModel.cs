using System.ComponentModel.DataAnnotations;

namespace CarWashWebsite.ViewModels.Admin;

public class PageSeoItemViewModel
{
    public string PageKey { get; set; } = string.Empty;
    public string PageName { get; set; } = string.Empty;
    public string PathDe { get; set; } = string.Empty;
    public string PathEn { get; set; } = string.Empty;

    [Display(Name = "SEO Title (German)")]
    [MaxLength(100)]
    public string? SeoTitleDe { get; set; }

    [Display(Name = "SEO Title (English)")]
    [MaxLength(100)]
    public string? SeoTitleEn { get; set; }

    [Display(Name = "Meta Description (German)")]
    [MaxLength(320)]
    public string? MetaDescriptionDe { get; set; }

    [Display(Name = "Meta Description (English)")]
    [MaxLength(320)]
    public string? MetaDescriptionEn { get; set; }

    [Display(Name = "Canonical URL")]
    public string? CanonicalUrl { get; set; }

    [Display(Name = "OG Image URL")]
    public string? OgImageUrl { get; set; }
}

public class SeoManagementViewModel
{
    public List<PageSeoItemViewModel> Pages { get; set; } = new();
    public string SelectedPageKey { get; set; } = "home";
    public PageSeoItemViewModel CurrentPage { get; set; } = new();
    public string SitemapUrl { get; set; } = "/sitemap.xml";
    public string RobotsUrl { get; set; } = "/robots.txt";
}
