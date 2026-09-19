using CarWashWebsite.Data;
using CarWashWebsite.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashWebsite.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("Admin/Dashboard")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel
        {
            TotalServices = await _context.Services.CountAsync(),
            ActiveServices = await _context.Services.CountAsync(s => s.IsActive),
            TotalGalleryItems = await _context.GalleryItems.CountAsync(),
            UnreadMessages = await _context.ContactMessages.CountAsync(m => !m.IsRead),
            TotalFaqs = await _context.FaqItems.CountAsync(),
            RecentMessages = await _context.ContactMessages
                .OrderByDescending(m => m.CreatedAt)
                .Take(5)
                .ToListAsync()
        };

        return View(model);
    }
}
