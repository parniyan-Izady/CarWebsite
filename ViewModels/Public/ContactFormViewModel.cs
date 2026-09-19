using System.ComponentModel.DataAnnotations;

namespace CarWashWebsite.ViewModels.Public;

public class ContactFormViewModel
{
    [Required(ErrorMessage = "Please enter your name / Bitte Namen eingeben")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a valid email / Bitte E-Mail eingeben")]
    [EmailAddress(ErrorMessage = "Invalid email format / Ungültige E-Mail")]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Please enter a subject / Bitte Betreff eingeben")]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your message / Bitte Nachricht eingeben")]
    [MaxLength(3000)]
    public string Message { get; set; } = string.Empty;

    public bool IsSuccess { get; set; } = false;
}
