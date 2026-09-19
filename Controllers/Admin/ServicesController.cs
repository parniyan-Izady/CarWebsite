using CarWashWebsite.Data.Entities;
using CarWashWebsite.Services.Interfaces;
using CarWashWebsite.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWashWebsite.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("Admin/Services")]
public class ServicesController : Controller
{
    private readonly IServiceService _serviceService;
    private readonly IMediaService _mediaService;

    public ServicesController(IServiceService serviceService, IMediaService mediaService)
    {
        _serviceService = serviceService;
        _mediaService = mediaService;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var services = await _serviceService.GetAllServicesAsync();
        return View(services);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new ServiceFormViewModel());
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string? imagePath = null;
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            imagePath = await _mediaService.UploadImageAsync(model.ImageFile, "services");
        }

        var service = new Service
        {
            TitleDe = model.TitleDe,
            TitleEn = model.TitleEn,
            DescriptionDe = model.DescriptionDe,
            DescriptionEn = model.DescriptionEn,
            Price = model.Price,
            PriceType = model.PriceType,
            ImagePath = imagePath,
            ImageAltDe = model.ImageAltDe,
            ImageAltEn = model.ImageAltEn,
            SlugDe = model.SlugDe ?? string.Empty,
            SlugEn = model.SlugEn ?? string.Empty,
            SeoTitleDe = model.SeoTitleDe,
            SeoTitleEn = model.SeoTitleEn,
            MetaDescriptionDe = model.MetaDescriptionDe,
            MetaDescriptionEn = model.MetaDescriptionEn,
            DisplayOrder = model.DisplayOrder,
            IsActive = model.IsActive
        };

        await _serviceService.CreateServiceAsync(service);
        TempData["SuccessMessage"] = $"Service '{service.TitleEn}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var service = await _serviceService.GetServiceByIdAsync(id);
        if (service == null)
        {
            return NotFound();
        }

        var model = new ServiceFormViewModel
        {
            Id = service.Id,
            TitleDe = service.TitleDe,
            TitleEn = service.TitleEn,
            DescriptionDe = service.DescriptionDe,
            DescriptionEn = service.DescriptionEn,
            Price = service.Price,
            PriceType = service.PriceType,
            ExistingImagePath = service.ImagePath,
            ImageAltDe = service.ImageAltDe,
            ImageAltEn = service.ImageAltEn,
            SlugDe = service.SlugDe,
            SlugEn = service.SlugEn,
            SeoTitleDe = service.SeoTitleDe,
            SeoTitleEn = service.SeoTitleEn,
            MetaDescriptionDe = service.MetaDescriptionDe,
            MetaDescriptionEn = service.MetaDescriptionEn,
            DisplayOrder = service.DisplayOrder,
            IsActive = service.IsActive
        };

        return View(model);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ServiceFormViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string? imagePath = model.ExistingImagePath;
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            imagePath = await _mediaService.UploadImageAsync(model.ImageFile, "services");
        }

        var service = new Service
        {
            Id = model.Id,
            TitleDe = model.TitleDe,
            TitleEn = model.TitleEn,
            DescriptionDe = model.DescriptionDe,
            DescriptionEn = model.DescriptionEn,
            Price = model.Price,
            PriceType = model.PriceType,
            ImagePath = imagePath,
            ImageAltDe = model.ImageAltDe,
            ImageAltEn = model.ImageAltEn,
            SlugDe = model.SlugDe ?? string.Empty,
            SlugEn = model.SlugEn ?? string.Empty,
            SeoTitleDe = model.SeoTitleDe,
            SeoTitleEn = model.SeoTitleEn,
            MetaDescriptionDe = model.MetaDescriptionDe,
            MetaDescriptionEn = model.MetaDescriptionEn,
            DisplayOrder = model.DisplayOrder,
            IsActive = model.IsActive
        };

        await _serviceService.UpdateServiceAsync(service);
        TempData["SuccessMessage"] = $"Service '{service.TitleEn}' updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("ToggleStatus/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var updatedStatus = await _serviceService.ToggleServiceStatusAsync(id);
        TempData["SuccessMessage"] = $"Service status changed to {(updatedStatus ? "Active" : "Inactive")}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _serviceService.DeleteServiceAsync(id);
        TempData["SuccessMessage"] = "Service deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
