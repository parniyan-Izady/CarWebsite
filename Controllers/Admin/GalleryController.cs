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
[Route("Admin/Gallery")]
public class GalleryController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMediaService _mediaService;

    public GalleryController(ApplicationDbContext context, IMediaService mediaService)
    {
        _context = context;
        _mediaService = mediaService;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(GalleryCategory? category)
    {
        var query = _context.GalleryItems.AsQueryable();

        if (category.HasValue)
        {
            query = query.Where(g => g.Category == category.Value);
        }

        var items = await query
            .OrderBy(g => g.DisplayOrder)
            .ThenByDescending(g => g.CreatedAt)
            .ToListAsync();

        ViewBag.SelectedCategory = category;
        return View(items);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new GalleryFormViewModel
        {
            Category = GalleryCategory.Detailing,
            DisplayOrder = 0,
            IsActive = true
        });
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GalleryFormViewModel model)
    {
        if (model.BeforeImageFile == null || model.BeforeImageFile.Length == 0)
        {
            ModelState.AddModelError(nameof(model.BeforeImageFile), "Before image is required.");
        }

        if (model.AfterImageFile == null || model.AfterImageFile.Length == 0)
        {
            ModelState.AddModelError(nameof(model.AfterImageFile), "After image is required.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string beforeImagePath = await _mediaService.UploadImageAsync(model.BeforeImageFile!, "gallery");
        string afterImagePath = await _mediaService.UploadImageAsync(model.AfterImageFile!, "gallery");

        var galleryItem = new GalleryItem
        {
            TitleDe = model.TitleDe,
            TitleEn = model.TitleEn,
            DescriptionDe = model.DescriptionDe,
            DescriptionEn = model.DescriptionEn,
            BeforeImagePath = beforeImagePath,
            AfterImagePath = afterImagePath,
            Category = model.Category,
            DisplayOrder = model.DisplayOrder,
            IsActive = model.IsActive
        };

        _context.GalleryItems.Add(galleryItem);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Gallery item created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.GalleryItems.FindAsync(id);
        if (item == null)
        {
            TempData["ErrorMessage"] = "Gallery item not found.";
            return RedirectToAction(nameof(Index));
        }

        var model = new GalleryFormViewModel
        {
            Id = item.Id,
            TitleDe = item.TitleDe,
            TitleEn = item.TitleEn,
            DescriptionDe = item.DescriptionDe,
            DescriptionEn = item.DescriptionEn,
            ExistingBeforeImagePath = item.BeforeImagePath,
            ExistingAfterImagePath = item.AfterImagePath,
            Category = item.Category,
            DisplayOrder = item.DisplayOrder,
            IsActive = item.IsActive
        };

        return View(model);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, GalleryFormViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var item = await _context.GalleryItems.FindAsync(id);
        if (item == null)
        {
            TempData["ErrorMessage"] = "Gallery item not found.";
            return RedirectToAction(nameof(Index));
        }

        if (model.BeforeImageFile != null && model.BeforeImageFile.Length > 0)
        {
            await _mediaService.DeleteImageAsync(item.BeforeImagePath);
            item.BeforeImagePath = await _mediaService.UploadImageAsync(model.BeforeImageFile, "gallery");
        }

        if (model.AfterImageFile != null && model.AfterImageFile.Length > 0)
        {
            await _mediaService.DeleteImageAsync(item.AfterImagePath);
            item.AfterImagePath = await _mediaService.UploadImageAsync(model.AfterImageFile, "gallery");
        }

        item.TitleDe = model.TitleDe;
        item.TitleEn = model.TitleEn;
        item.DescriptionDe = model.DescriptionDe;
        item.DescriptionEn = model.DescriptionEn;
        item.Category = model.Category;
        item.DisplayOrder = model.DisplayOrder;
        item.IsActive = model.IsActive;

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Gallery item updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("ToggleStatus/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var item = await _context.GalleryItems.FindAsync(id);
        if (item != null)
        {
            item.IsActive = !item.IsActive;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Gallery item status changed to {(item.IsActive ? "Active" : "Inactive")}.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.GalleryItems.FindAsync(id);
        if (item != null)
        {
            await _mediaService.DeleteImageAsync(item.BeforeImagePath);
            await _mediaService.DeleteImageAsync(item.AfterImagePath);

            _context.GalleryItems.Remove(item);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Gallery item deleted successfully.";
        }
        return RedirectToAction(nameof(Index));
    }
}
