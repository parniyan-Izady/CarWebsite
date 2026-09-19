using System.ComponentModel.DataAnnotations;

namespace CarWashWebsite.ViewModels.Admin;

public class FaqFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Question (German) is required.")]
    [MaxLength(300)]
    [Display(Name = "Question (German)")]
    public string QuestionDe { get; set; } = string.Empty;

    [Required(ErrorMessage = "Question (English) is required.")]
    [MaxLength(300)]
    [Display(Name = "Question (English)")]
    public string QuestionEn { get; set; } = string.Empty;

    [Required(ErrorMessage = "Answer (German) is required.")]
    [Display(Name = "Answer (German)")]
    public string AnswerDe { get; set; } = string.Empty;

    [Required(ErrorMessage = "Answer (English) is required.")]
    [Display(Name = "Answer (English)")]
    public string AnswerEn { get; set; } = string.Empty;

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; } = 0;

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}
