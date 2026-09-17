using System.ComponentModel.DataAnnotations;

namespace CarWashWebsite.Data.Entities;

public class GalleryItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string TitleDe { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string TitleEn { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? DescriptionDe { get; set; }

    [MaxLength(500)]
    public string? DescriptionEn { get; set; }

    [Required]
    [MaxLength(500)]
    public string BeforeImagePath { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string AfterImagePath { get; set; } = string.Empty;

    public GalleryCategory Category { get; set; } = GalleryCategory.Detailing;

    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
