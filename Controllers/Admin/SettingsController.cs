using CarWashWebsite.Data;
using CarWashWebsite.Data.Entities;
using CarWashWebsite.Services.Interfaces;
using CarWashWebsite.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashWebsite.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("Admin/Settings")]
public class SettingsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMediaService _mediaService;

    public SettingsController(ApplicationDbContext context, IMediaService mediaService)
    {
        _context = context;
        _mediaService = mediaService;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var settingsList = await _context.SiteSettings.ToListAsync();
        var dict = settingsList.ToDictionary(s => s.Key, s => s);

        var model = new SiteSettingsFormViewModel
        {
            BusinessNameDe = GetSettingDe(dict, "BusinessName", "Berlin Car Care & Detailing"),
            BusinessNameEn = GetSettingEn(dict, "BusinessName", "Berlin Car Care & Detailing"),
            Phone = GetSettingDe(dict, "Phone", "+49 30 12345678"),
            Email = GetSettingDe(dict, "Email", "info@berlincarcare.de"),
            AddressDe = GetSettingDe(dict, "Address", "Kurfürstendamm 120, 10711 Berlin"),
            AddressEn = GetSettingEn(dict, "Address", "Kurfürstendamm 120, 10711 Berlin, Germany"),
            OpeningHoursDe = GetSettingDe(dict, "OpeningHours", "Mo - Sa: 08:00 - 18:00 Uhr"),
            OpeningHoursEn = GetSettingEn(dict, "OpeningHours", "Mon - Sat: 08:00 AM - 06:00 PM"),
            GoogleMapsEmbed = GetSettingDe(dict, "GoogleMapsEmbed", ""),
            InstagramUrl = GetSettingDe(dict, "Instagram", "https://instagram.com/berlincarcare"),
            FacebookUrl = GetSettingDe(dict, "Facebook", ""),
            WhatsAppUrl = GetSettingDe(dict, "WhatsApp", ""),
            LogoPath = GetSettingDe(dict, "LogoPath", ""),
            FaviconPath = GetSettingDe(dict, "FaviconPath", ""),
            DefaultCulture = GetSettingDe(dict, "DefaultCulture", "de"),
            EnableEnglish = GetSettingDe(dict, "EnableEnglish", "true") == "true"
        };

        return View(model);
    }

    [HttpPost("")]
    [HttpPost("Index")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SiteSettingsFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var settingsList = await _context.SiteSettings.ToListAsync();
        var dict = settingsList.ToDictionary(s => s.Key, s => s);

        // Upload logo if provided
        if (model.LogoFile != null && model.LogoFile.Length > 0)
        {
            model.LogoPath = await _mediaService.UploadImageAsync(model.LogoFile, "branding");
        }

        // Upload favicon if provided
        if (model.FaviconFile != null && model.FaviconFile.Length > 0)
        {
            model.FaviconPath = await _mediaService.UploadImageAsync(model.FaviconFile, "branding");
        }

        await SaveSettingAsync(dict, "BusinessName", model.BusinessNameDe, model.BusinessNameEn, "General");
        await SaveSettingAsync(dict, "Phone", model.Phone, model.Phone, "Contact");
        await SaveSettingAsync(dict, "Email", model.Email, model.Email, "Contact");
        await SaveSettingAsync(dict, "Address", model.AddressDe, model.AddressEn, "Contact");
        await SaveSettingAsync(dict, "OpeningHours", model.OpeningHoursDe, model.OpeningHoursEn, "Contact");
        await SaveSettingAsync(dict, "GoogleMapsEmbed", model.GoogleMapsEmbed, model.GoogleMapsEmbed, "Contact");
        await SaveSettingAsync(dict, "Instagram", model.InstagramUrl, model.InstagramUrl, "Social");
        await SaveSettingAsync(dict, "Facebook", model.FacebookUrl, model.FacebookUrl, "Social");
        await SaveSettingAsync(dict, "WhatsApp", model.WhatsAppUrl, model.WhatsAppUrl, "Social");
        await SaveSettingAsync(dict, "LogoPath", model.LogoPath ?? "", model.LogoPath ?? "", "Branding");
        await SaveSettingAsync(dict, "FaviconPath", model.FaviconPath ?? "", model.FaviconPath ?? "", "Branding");
        await SaveSettingAsync(dict, "DefaultCulture", model.DefaultCulture, model.DefaultCulture, "Localization");
        await SaveSettingAsync(dict, "EnableEnglish", model.EnableEnglish ? "true" : "false", model.EnableEnglish ? "true" : "false", "Localization");

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Site settings have been saved successfully.";
        return RedirectToAction(nameof(Index));
    }

    private static string GetSettingDe(Dictionary<string, SiteSetting> dict, string key, string defaultValue)
    {
        return dict.TryGetValue(key, out var s) ? s.ValueDe : defaultValue;
    }

    private static string GetSettingEn(Dictionary<string, SiteSetting> dict, string key, string defaultValue)
    {
        return dict.TryGetValue(key, out var s) ? s.ValueEn : defaultValue;
    }

    private async Task SaveSettingAsync(Dictionary<string, SiteSetting> dict, string key, string valDe, string valEn, string group)
    {
        if (dict.TryGetValue(key, out var setting))
        {
            setting.ValueDe = valDe;
            setting.ValueEn = valEn;
            setting.Group = group;
        }
        else
        {
            var newSetting = new SiteSetting
            {
                Key = key,
                ValueDe = valDe,
                ValueEn = valEn,
                Group = group
            };
            await _context.SiteSettings.AddAsync(newSetting);
            dict[key] = newSetting;
        }
    }
}
