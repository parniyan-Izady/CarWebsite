using CarWashWebsite.Services.Interfaces;
using CarWashWebsite.ViewModels.Public;
using Microsoft.AspNetCore.Mvc;

namespace CarWashWebsite.Controllers;

public class ServicesController : Controller
{
    private readonly IServiceService _serviceService;
    private readonly ISeoService _seoService;

    public ServicesController(IServiceService serviceService, ISeoService seoService)
    {
        _serviceService = serviceService;
        _seoService = seoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var culture = (string?)RouteData.Values["culture"] ?? "de";
        var services = await _serviceService.GetAllServicesAsync(onlyActive: true);
        ViewData["CurrentCulture"] = culture;
        return View(services);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return NotFound();
        }

        var culture = (string?)RouteData.Values["culture"] ?? "de";
        var service = await _serviceService.GetServiceBySlugAsync(slug, culture);

        if (service == null)
        {
            return NotFound();
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        ViewData["SchemaJsonLd"] = _seoService.GenerateServiceSchema(baseUrl, culture, service);

        var allServices = await _serviceService.GetAllServicesAsync(onlyActive: true);
        var otherServices = allServices.Where(s => s.Id != service.Id).Take(3);

        var viewModel = new ServiceDetailViewModel
        {
            Service = service,
            OtherServices = otherServices,
            CurrentCulture = culture,
            GermanSlug = service.SlugDe,
            EnglishSlug = service.SlugEn
        };

        return View(viewModel);
    }
}
