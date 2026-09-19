namespace CarWashWebsite.ViewModels.Admin;

public class MediaFileViewModel
{
    public string FileName { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string FileSizeFormatted { get; set; } = string.Empty;
    public int? Width { get; set; }
    public int? Height { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Folder { get; set; } = string.Empty;
}

public class MediaLibraryViewModel
{
    public List<MediaFileViewModel> Files { get; set; } = new();
    public string CurrentFolder { get; set; } = "all";
    public List<string> Folders { get; set; } = new();
    public long TotalStorageBytes { get; set; }
}
