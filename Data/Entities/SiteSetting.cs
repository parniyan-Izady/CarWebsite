using System.ComponentModel.DataAnnotations;

namespace CarWashWebsite.Data.Entities;

public class SiteSetting
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Key { get; set; } = string.Empty;

    public string? ValueDe { get; set; }

    public string? ValueEn { get; set; }

    [MaxLength(50)]
    public string Group { get; set; } = "General"; // General, Contact, SEO, Social
}
