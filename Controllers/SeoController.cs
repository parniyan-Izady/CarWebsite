using CarWashWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarWashWebsite.Controllers;

public class SeoController : Controller
{
    private readonly ISeoService _seoService;

    public SeoController(ISeoService seoService)
    {
        _seoService = seoService;
    }

    [HttpGet("sitemap.xml")]
    [ResponseCache(Duration = 3600)]
    public async Task<IActionResult> Sitemap()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var xml = await _seoService.GenerateSitemapXmlAsync(baseUrl);
        return Content(xml, "application/xml");
    }

    [HttpGet("robots.txt")]
    [ResponseCache(Duration = 86400)]
    public IActionResult Robots()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var robots = _seoService.GenerateRobotsTxt(baseUrl);
        return Content(robots, "text/plain");
    }
}
