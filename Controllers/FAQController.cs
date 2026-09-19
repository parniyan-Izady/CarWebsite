using CarWashWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarWashWebsite.Controllers;

public class FAQController : Controller
{
    private readonly IContentService _contentService;

    public FAQController(IContentService contentService)
    {
        _contentService = contentService;
    }

    public async Task<IActionResult> Index()
    {
        var culture = (string?)RouteData.Values["culture"] ?? "de";
        var faqs = await _contentService.GetFaqsAsync();
        ViewData["CurrentCulture"] = culture;
        return View(faqs);
    }
}
