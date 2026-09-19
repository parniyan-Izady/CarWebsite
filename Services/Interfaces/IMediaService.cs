using CarWashWebsite.ViewModels.Admin;

namespace CarWashWebsite.Services.Interfaces;

public interface IMediaService
{
    Task<string> UploadImageAsync(IFormFile file, string subFolder);
    Task<bool> DeleteImageAsync(string? relativePath);
    Task<List<MediaFileViewModel>> GetMediaFilesAsync(string? folder = null);
}
