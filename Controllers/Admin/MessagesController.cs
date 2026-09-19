using CarWashWebsite.Data;
using CarWashWebsite.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashWebsite.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("Admin/Messages")]
public class MessagesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MessagesController> _logger;

    public MessagesController(ApplicationDbContext context, ILogger<MessagesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(string tab = "new")
    {
        var normalizedTab = tab.ToLowerInvariant();
        if (normalizedTab != "read" && normalizedTab != "archived")
        {
            normalizedTab = "new";
        }

        var query = _context.ContactMessages.AsQueryable();

        var newCount = await _context.ContactMessages.CountAsync(m => !m.IsRead && !m.IsArchived);
        var readCount = await _context.ContactMessages.CountAsync(m => m.IsRead && !m.IsArchived);
        var archivedCount = await _context.ContactMessages.CountAsync(m => m.IsArchived);

        var messages = normalizedTab switch
        {
            "read" => await query.Where(m => m.IsRead && !m.IsArchived).OrderByDescending(m => m.CreatedAt).ToListAsync(),
            "archived" => await query.Where(m => m.IsArchived).OrderByDescending(m => m.CreatedAt).ToListAsync(),
            _ => await query.Where(m => !m.IsRead && !m.IsArchived).OrderByDescending(m => m.CreatedAt).ToListAsync()
        };

        var model = new MessagesIndexViewModel
        {
            Messages = messages,
            CurrentTab = normalizedTab,
            NewCount = newCount,
            ReadCount = readCount,
            ArchivedCount = archivedCount
        };

        return View(model);
    }

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message == null)
        {
            TempData["ErrorMessage"] = "Message not found.";
            return RedirectToAction(nameof(Index));
        }

        if (!message.IsRead)
        {
            message.IsRead = true;
            await _context.SaveChangesAsync();
        }

        return View(message);
    }

    [HttpPost("MarkAsRead/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsRead(int id, string returnTab = "new")
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message != null)
        {
            message.IsRead = true;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Message marked as read.";
        }
        return RedirectToAction(nameof(Index), new { tab = returnTab });
    }

    [HttpPost("ToggleArchive/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleArchive(int id, string returnTab = "new")
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message != null)
        {
            message.IsArchived = !message.IsArchived;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = message.IsArchived ? "Message moved to archive." : "Message restored from archive.";
        }
        return RedirectToAction(nameof(Index), new { tab = returnTab });
    }

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, string returnTab = "new")
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message != null)
        {
            _context.ContactMessages.Remove(message);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Message deleted successfully.";
        }
        return RedirectToAction(nameof(Index), new { tab = returnTab });
    }
}
