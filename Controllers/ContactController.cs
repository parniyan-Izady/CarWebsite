using CarWashWebsite.Data.Entities;
using CarWashWebsite.Services.Interfaces;
using CarWashWebsite.ViewModels.Public;
using Microsoft.AspNetCore.Mvc;

namespace CarWashWebsite.Controllers;

public class ContactController : Controller
{
    private readonly IContactService _contactService;
    private readonly IContentService _contentService;

    public ContactController(IContactService contactService, IContentService contentService)
    {
        _contactService = contactService;
        _contentService = contentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var culture = (string?)RouteData.Values["culture"] ?? "de";
        ViewData["CurrentCulture"] = culture;
        ViewData["Settings"] = await _contentService.GetSiteSettingsAsync(culture);
        return View(new ContactFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactFormViewModel model)
    {
        var culture = (string?)RouteData.Values["culture"] ?? "de";
        ViewData["CurrentCulture"] = culture;
        ViewData["Settings"] = await _contentService.GetSiteSettingsAsync(culture);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var message = new ContactMessage
        {
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            Subject = model.Subject,
            Message = model.Message
        };

        var success = await _contactService.SubmitMessageAsync(message);
        if (success)
        {
            model.IsSuccess = true;
            ModelState.Clear();
            return View(new ContactFormViewModel { IsSuccess = true });
        }

        ModelState.AddModelError(string.Empty, culture == "en"
            ? "An error occurred while sending your message. Please try again."
            : "Beim Senden Ihrer Nachricht ist ein Fehler aufgetreten. Bitte versuchen Sie es erneut.");

        return View(model);
    }
}
