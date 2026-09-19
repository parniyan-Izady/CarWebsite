using CarWashWebsite.Services.Interfaces;
using CarWashWebsite.ViewModels.Admin;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace CarWashWebsite.Services.Implementations;

public class MediaService : IMediaService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<MediaService> _logger;

    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".avif"];
    private static readonly string[] AllowedMimeTypes = ["image/jpeg", "image/png", "image/webp", "image/avif"];
    private const long MaxFileSize = 10 * 1024 * 1024; // 10MB limit

    public MediaService(IWebHostEnvironment env, ILogger<MediaService> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async Task<string> UploadImageAsync(IFormFile file, string subFolder)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No file provided.");
        }

        if (file.Length > MaxFileSize)
        {
            throw new InvalidOperationException("File size exceeds 10MB limit.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension) || !AllowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            throw new InvalidOperationException("Invalid image format. Allowed formats: JPG, PNG, WebP, AVIF.");
        }

        var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", subFolder);
        Directory.CreateDirectory(uploadsRoot);

        var uniqueFileName = $"{Guid.NewGuid():N}.webp";
        var targetFilePath = Path.Combine(uploadsRoot, uniqueFileName);

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        using var image = await Image.LoadAsync(memoryStream);

        // Resize if exceeding max 1920 width, keeping aspect ratio
        if (image.Width > 1920)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(1920, 0),
                Mode = ResizeMode.Max
            }));
        }

        var encoder = new WebpEncoder
        {
            Quality = 80
        };

        await image.SaveAsync(targetFilePath, encoder);

        // Generate thumbnail
        var thumbDir = Path.Combine(uploadsRoot, "thumbs");
        Directory.CreateDirectory(thumbDir);
        var thumbPath = Path.Combine(thumbDir, uniqueFileName);

        using var thumbImage = image.Clone(x => x.Resize(new ResizeOptions
        {
            Size = new Size(400, 300),
            Mode = ResizeMode.Crop
        }));

        await thumbImage.SaveAsync(thumbPath, encoder);

        _logger.LogInformation("Image successfully processed and saved to {Path}", targetFilePath);

        return $"/uploads/{subFolder}/{uniqueFileName}";
    }

    public Task<bool> DeleteImageAsync(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return Task.FromResult(false);
        }

        try
        {
            var cleanedPath = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_env.WebRootPath, cleanedPath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);

                // Also attempt deleting matching thumbnail if present
                var dir = Path.GetDirectoryName(fullPath);
                var fileName = Path.GetFileName(fullPath);
                if (dir != null)
                {
                    var thumbFile = Path.Combine(dir, "thumbs", fileName);
                    if (File.Exists(thumbFile))
                    {
                        File.Delete(thumbFile);
                    }
                }
                return Task.FromResult(true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete image at {Path}", relativePath);
        }

        return Task.FromResult(false);
    }

    public Task<List<MediaFileViewModel>> GetMediaFilesAsync(string? folder = null)
    {
        var result = new List<MediaFileViewModel>();
        var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads");

        if (!Directory.Exists(uploadsRoot))
        {
            return Task.FromResult(result);
        }

        var searchDirs = string.IsNullOrWhiteSpace(folder) || folder.Equals("all", StringComparison.OrdinalIgnoreCase)
            ? Directory.GetDirectories(uploadsRoot)
            : new[] { Path.Combine(uploadsRoot, folder) };

        foreach (var dir in searchDirs)
        {
            if (!Directory.Exists(dir)) continue;

            var folderName = Path.GetFileName(dir);
            var files = Directory.GetFiles(dir);

            foreach (var filePath in files)
            {
                var ext = Path.GetExtension(filePath).ToLowerInvariant();
                if (!AllowedExtensions.Contains(ext)) continue;

                var fileInfo = new FileInfo(filePath);
                var fileName = fileInfo.Name;
                var relPath = $"/uploads/{folderName}/{fileName}";
                var thumbRelPath = File.Exists(Path.Combine(dir, "thumbs", fileName))
                    ? $"/uploads/{folderName}/thumbs/{fileName}"
                    : relPath;

                result.Add(new MediaFileViewModel
                {
                    FileName = fileName,
                    RelativePath = relPath,
                    ThumbnailPath = thumbRelPath,
                    FileSizeBytes = fileInfo.Length,
                    FileSizeFormatted = FormatBytes(fileInfo.Length),
                    CreatedAt = fileInfo.CreationTimeUtc,
                    Folder = folderName
                });
            }
        }

        return Task.FromResult(result.OrderByDescending(f => f.CreatedAt).ToList());
    }

    private static string FormatBytes(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        int order = 0;
        double len = bytes;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:0.#} {sizes[order]}";
    }
}

