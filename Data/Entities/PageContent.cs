using System.ComponentModel.DataAnnotations;

namespace CarWashWebsite.Data.Entities;

public class PageContent
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string PageKey { get; set; } = string.Empty; // Home, About, Impressum, Privacy

    [Required]
    [MaxLength(50)]
    public string SectionKey { get; set; } = string.Empty; // Hero, Story, WhyUs, etc.

    [MaxLength(300)]
    public string? TitleDe { get; set; }

    [MaxLength(300)]
    public string? TitleEn { get; set; }

    public string? ContentDe { get; set; }

    public string? ContentEn { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
