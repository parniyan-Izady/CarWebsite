using System.ComponentModel.DataAnnotations;

namespace CarWashWebsite.Data.Entities;

public class BlogPost
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(250)]
    public string TitleDe { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string TitleEn { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string SlugDe { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string SlugEn { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string SummaryDe { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string SummaryEn { get; set; } = string.Empty;

    [Required]
    public string ContentDe { get; set; } = string.Empty;

    [Required]
    public string ContentEn { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? CoverImagePath { get; set; }

    [MaxLength(100)]
    public string Category { get; set; } = "Detailing"; // Detailing, PaintCare, Interior, Guides

    public int ReadingTimeMinutes { get; set; } = 5;

    [MaxLength(250)]
    public string? SeoTitleDe { get; set; }

    [MaxLength(250)]
    public string? SeoTitleEn { get; set; }

    [MaxLength(500)]
    public string? MetaDescriptionDe { get; set; }

    [MaxLength(500)]
    public string? MetaDescriptionEn { get; set; }

    public bool IsPublished { get; set; } = true;

    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public int ViewCount { get; set; } = 0;
}
