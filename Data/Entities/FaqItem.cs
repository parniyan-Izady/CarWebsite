using System.ComponentModel.DataAnnotations;

namespace CarWashWebsite.Data.Entities;

public class FaqItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(300)]
    public string QuestionDe { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string QuestionEn { get; set; } = string.Empty;

    [Required]
    public string AnswerDe { get; set; } = string.Empty;

    [Required]
    public string AnswerEn { get; set; } = string.Empty;

    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
