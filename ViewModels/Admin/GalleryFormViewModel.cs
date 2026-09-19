using System.ComponentModel.DataAnnotations;
using CarWashWebsite.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace CarWashWebsite.ViewModels.Admin;

public class GalleryFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title (German) is required.")]
    [MaxLength(200)]
    [Display(Name = "Title (German)")]
    public string TitleDe { get; set; } = string.Empty;

    [Required(ErrorMessage = "Title (English) is required.")]
    [MaxLength(200)]
    [Display(Name = "Title (English)")]
    public string TitleEn { get; set; } = string.Empty;

    [MaxLength(500)]
    [Display(Name = "Description (German)")]
    public string? DescriptionDe { get; set; }

    [MaxLength(500)]
    [Display(Name = "Description (English)")]
    public string? DescriptionEn { get; set; }

    public string? ExistingBeforeImagePath { get; set; }
    public string? ExistingAfterImagePath { get; set; }

    [Display(Name = "Before Image")]
    public IFormFile? BeforeImageFile { get; set; }

    [Display(Name = "After Image")]
    public IFormFile? AfterImageFile { get; set; }

    [Required]
    [Display(Name = "Category")]
    public GalleryCategory Category { get; set; } = GalleryCategory.Detailing;

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; } = 0;

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}
