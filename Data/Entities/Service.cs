using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWashWebsite.Data.Entities;

public class Service
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string TitleDe { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string TitleEn { get; set; } = string.Empty;

    [Required]
    public string DescriptionDe { get; set; } = string.Empty;

    [Required]
    public string DescriptionEn { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Price { get; set; }

    [Required]
    public PriceType PriceType { get; set; } = PriceType.StartingFrom;

    [MaxLength(500)]
    public string? ImagePath { get; set; }

    [MaxLength(250)]
    public string? ImageAltDe { get; set; }

    [MaxLength(250)]
    public string? ImageAltEn { get; set; }

    [Required]
    [MaxLength(200)]
    public string SlugDe { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string SlugEn { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? SeoTitleDe { get; set; }

    [MaxLength(250)]
    public string? SeoTitleEn { get; set; }

    [MaxLength(500)]
    public string? MetaDescriptionDe { get; set; }

    [MaxLength(500)]
    public string? MetaDescriptionEn { get; set; }

    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
