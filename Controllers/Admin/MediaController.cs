using CarWashWebsite.Services.Interfaces;
using CarWashWebsite.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWashWebsite.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("Admin/Media")]
public class MediaController : Controller
{
    private readonly IMediaService _mediaService;
    private readonly IWebHostEnvironment _env;

    public MediaController(IMediaService mediaService, IWebHostEnvironment env)
    {
        _mediaService = mediaService;
        _env = env;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(string folder = "all")
    {
        var files = await _mediaService.GetMediaFilesAsync(folder);

        var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads");
        var folders = new List<string>();
        if (Directory.Exists(uploadsRoot))
        {
            folders = Directory.GetDirectories(uploadsRoot)
                .Select(Path.GetFileName)
                .Where(f => !string.IsNullOrEmpty(f) && !f.Equals("thumbs", StringComparison.OrdinalIgnoreCase))
                .ToList()!;
        }

        long totalStorage = files.Sum(f => f.FileSizeBytes);

        var model = new MediaLibraryViewModel
        {
            Files = files,
            CurrentFolder = folder,
            Folders = folders,
            TotalStorageBytes = totalStorage
        };

        return View(model);
    }

    [HttpPost("Upload")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IFormFile file, string folder = "general")
    {
        if (file == null || file.Length == 0)
        {
            TempData["ErrorMessage"] = "Please select an image file to upload.";
            return RedirectToAction(nameof(Index), new { folder });
        }

        try
        {
            var savedPath = await _mediaService.UploadImageAsync(file, folder);
            TempData["SuccessMessage"] = $"Image uploaded successfully: {savedPath}";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index), new { folder });
    }

    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string relativePath, string folder = "all")
    {
        if (!string.IsNullOrWhiteSpace(relativePath))
        {
            var deleted = await _mediaService.DeleteImageAsync(relativePath);
            if (deleted)
            {
                TempData["SuccessMessage"] = "Asset deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Could not delete asset from storage.";
            }
        }

        return RedirectToAction(nameof(Index), new { folder });
    }
}
