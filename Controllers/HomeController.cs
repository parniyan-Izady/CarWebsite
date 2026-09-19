using CarWashWebsite.Services.Interfaces;
using CarWashWebsite.ViewModels.Public;
using Microsoft.AspNetCore.Mvc;

namespace CarWashWebsite.Controllers;

public class HomeController : Controller
{
    private readonly IServiceService _serviceService;
    private readonly IContentService _contentService;
    private readonly ISeoService _seoService;

    public HomeController(IServiceService serviceService, IContentService contentService, ISeoService seoService)
    {
        _serviceService = serviceService;
        _contentService = contentService;
        _seoService = seoService;
    }

    public async Task<IActionResult> Index()
    {
        var culture = (string?)RouteData.Values["culture"] ?? "de";
        var services = await _serviceService.GetAllServicesAsync(onlyActive: true);
        var faqs = await _contentService.GetFaqsAsync();
        var settings = await _contentService.GetSiteSettingsAsync(culture);

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        ViewData["SchemaJsonLd"] = _seoService.GenerateLocalBusinessSchema(baseUrl, culture, settings);

        var model = new HomeViewModel
        {
            FeaturedServices = services,
            TopFaqs = faqs.Take(4),
            Settings = settings,
            CurrentCulture = culture
        };

        return View(model);
    }

    public IActionResult Impressum()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
