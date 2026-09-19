using CarWashWebsite.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace CarWashWebsite.ViewModels.Admin;

public class ServiceFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "German title is required.")]
    [Display(Name = "Title (German)")]
    public string TitleDe { get; set; } = string.Empty;

    [Required(ErrorMessage = "English title is required.")]
    [Display(Name = "Title (English)")]
    public string TitleEn { get; set; } = string.Empty;

    [Required(ErrorMessage = "German description is required.")]
    [Display(Name = "Description (German)")]
    public string DescriptionDe { get; set; } = string.Empty;

    [Required(ErrorMessage = "English description is required.")]
    [Display(Name = "Description (English)")]
    public string DescriptionEn { get; set; } = string.Empty;

    [Display(Name = "Price (€)")]
    public decimal? Price { get; set; }

    [Required]
    [Display(Name = "Pricing Type")]
    public PriceType PriceType { get; set; } = PriceType.StartingFrom;

    public string? ExistingImagePath { get; set; }

    [Display(Name = "Service Image")]
    public IFormFile? ImageFile { get; set; }

    [Display(Name = "Image Alt Text (German)")]
    public string? ImageAltDe { get; set; }

    [Display(Name = "Image Alt Text (English)")]
    public string? ImageAltEn { get; set; }

    [Display(Name = "URL Slug (German)")]
    public string? SlugDe { get; set; }

    [Display(Name = "URL Slug (English)")]
    public string? SlugEn { get; set; }

    [Display(Name = "SEO Meta Title (German)")]
    public string? SeoTitleDe { get; set; }

    [Display(Name = "SEO Meta Title (English)")]
    public string? SeoTitleEn { get; set; }

    [Display(Name = "SEO Meta Description (German)")]
    public string? MetaDescriptionDe { get; set; }

    [Display(Name = "SEO Meta Description (English)")]
    public string? MetaDescriptionEn { get; set; }

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; } = 0;

    [Display(Name = "Is Visible on Website")]
    public bool IsActive { get; set; } = true;
}
