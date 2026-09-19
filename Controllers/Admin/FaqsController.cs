using CarWashWebsite.Data;
using CarWashWebsite.Data.Entities;
using CarWashWebsite.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashWebsite.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("Admin/Faqs")]
public class FaqsController : Controller
{
    private readonly ApplicationDbContext _context;

    public FaqsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var faqs = await _context.FaqItems
            .OrderBy(f => f.DisplayOrder)
            .ThenByDescending(f => f.CreatedAt)
            .ToListAsync();
        return View(faqs);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new FaqFormViewModel { DisplayOrder = 0, IsActive = true });
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FaqFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var faq = new FaqItem
        {
            QuestionDe = model.QuestionDe,
            QuestionEn = model.QuestionEn,
            AnswerDe = model.AnswerDe,
            AnswerEn = model.AnswerEn,
            DisplayOrder = model.DisplayOrder,
            IsActive = model.IsActive
        };

        _context.FaqItems.Add(faq);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "FAQ item created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var faq = await _context.FaqItems.FindAsync(id);
        if (faq == null)
        {
            TempData["ErrorMessage"] = "FAQ item not found.";
            return RedirectToAction(nameof(Index));
        }

        var model = new FaqFormViewModel
        {
            Id = faq.Id,
            QuestionDe = faq.QuestionDe,
            QuestionEn = faq.QuestionEn,
            AnswerDe = faq.AnswerDe,
            AnswerEn = faq.AnswerEn,
            DisplayOrder = faq.DisplayOrder,
            IsActive = faq.IsActive
        };

        return View(model);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FaqFormViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var faq = await _context.FaqItems.FindAsync(id);
        if (faq == null)
        {
            TempData["ErrorMessage"] = "FAQ item not found.";
            return RedirectToAction(nameof(Index));
        }

        faq.QuestionDe = model.QuestionDe;
        faq.QuestionEn = model.QuestionEn;
        faq.AnswerDe = model.AnswerDe;
        faq.AnswerEn = model.AnswerEn;
        faq.DisplayOrder = model.DisplayOrder;
        faq.IsActive = model.IsActive;

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "FAQ item updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("ToggleStatus/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var faq = await _context.FaqItems.FindAsync(id);
        if (faq != null)
        {
            faq.IsActive = !faq.IsActive;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"FAQ status changed to {(faq.IsActive ? "Active" : "Inactive")}.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var faq = await _context.FaqItems.FindAsync(id);
        if (faq != null)
        {
            _context.FaqItems.Remove(faq);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "FAQ item deleted successfully.";
        }
        return RedirectToAction(nameof(Index));
    }
}
