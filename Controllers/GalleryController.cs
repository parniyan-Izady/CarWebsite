using CarWashWebsite.Data.Entities;
using CarWashWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarWashWebsite.Controllers;

public class GalleryController : Controller
{
    private readonly IContentService _contentService;

    public GalleryController(IContentService contentService)
    {
        _contentService = contentService;
    }

    public async Task<IActionResult> Index(GalleryCategory? category)
    {
        var culture = (string?)RouteData.Values["culture"] ?? "de";
        var items = await _contentService.GetGalleryItemsAsync(category);
        ViewData["CurrentCulture"] = culture;
        ViewData["SelectedCategory"] = category;
        return View(items);
    }
}
