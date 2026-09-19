using Microsoft.AspNetCore.Mvc;

namespace CarWashWebsite.Controllers;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        var culture = (string?)RouteData.Values["culture"] ?? "de";
        ViewData["CurrentCulture"] = culture;
        return View();
    }
}
