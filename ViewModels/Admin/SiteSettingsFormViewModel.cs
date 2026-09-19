using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CarWashWebsite.ViewModels.Admin;

public class SiteSettingsFormViewModel
{
    // Contact Info
    [Display(Name = "Business Name (DE)")]
    public string BusinessNameDe { get; set; } = "Berlin Car Care & Detailing";

    [Display(Name = "Business Name (EN)")]
    public string BusinessNameEn { get; set; } = "Berlin Car Care & Detailing";

    [Display(Name = "Telephone Number")]
    public string Phone { get; set; } = "+49 30 12345678";

    [Display(Name = "Email Address")]
    [EmailAddress]
    public string Email { get; set; } = "info@berlincarcare.de";

    [Display(Name = "Address (DE)")]
    public string AddressDe { get; set; } = "Kurfürstendamm 120, 10711 Berlin";

    [Display(Name = "Address (EN)")]
    public string AddressEn { get; set; } = "Kurfürstendamm 120, 10711 Berlin, Germany";

    [Display(Name = "Opening Hours (DE)")]
    public string OpeningHoursDe { get; set; } = "Mo - Sa: 08:00 - 18:00 Uhr";

    [Display(Name = "Opening Hours (EN)")]
    public string OpeningHoursEn { get; set; } = "Mon - Sat: 08:00 AM - 06:00 PM";

    [Display(Name = "Google Maps Embed URL")]
    public string GoogleMapsEmbed { get; set; } = string.Empty;

    // Social Links
    [Display(Name = "Instagram URL")]
    public string InstagramUrl { get; set; } = string.Empty;

    [Display(Name = "Facebook URL")]
    public string FacebookUrl { get; set; } = string.Empty;

    [Display(Name = "WhatsApp Number or Link")]
    public string WhatsAppUrl { get; set; } = string.Empty;

    // Branding & Localization
    public string? LogoPath { get; set; }
    public IFormFile? LogoFile { get; set; }

    public string? FaviconPath { get; set; }
    public IFormFile? FaviconFile { get; set; }

    [Display(Name = "Default Language")]
    public string DefaultCulture { get; set; } = "de";

    [Display(Name = "Enable English Language")]
    public bool EnableEnglish { get; set; } = true;

    // Module Visibility Toggles
    [Display(Name = "Enable Blog / SEO Articles Module")]
    public bool EnableBlog { get; set; } = true;

    [Display(Name = "Enable Before / After Gallery Module")]
    public bool EnableGallery { get; set; } = true;

    [Display(Name = "Enable FAQ Section Module")]
    public bool EnableFaq { get; set; } = true;
}
