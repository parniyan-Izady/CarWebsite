using CarWashWebsite.Data;
using CarWashWebsite.Data.Entities;
using CarWashWebsite.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashWebsite.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("Admin/Pages")]
public class PagesController : Controller
{
    private readonly ApplicationDbContext _context;

    public PagesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        return View();
    }

    // --- 1. HOME PAGE ---
    [HttpGet("Home")]
    public async Task<IActionResult> Home()
    {
        var items = await _context.PageContents.Where(p => p.PageKey == "Home").ToListAsync();
        var dict = items.ToDictionary(p => p.SectionKey, p => p);

        var model = new HomePageContentViewModel
        {
            HeroTitleDe = GetTitleDe(dict, "Hero", "Exzellente Autopflege & Detailing in Berlin"),
            HeroTitleEn = GetTitleEn(dict, "Hero", "Masterful Car Care & Detailing in Berlin"),
            HeroSubtitleDe = GetContentDe(dict, "Hero", "Ihre Spezialisten für professionelle Fahrzeugaufbereitung, Tiefenreinigung und langanhaltenden Lackschutz."),
            HeroSubtitleEn = GetContentEn(dict, "Hero", "Certified automotive detailing, high-gloss correction, and ceramic protection in the center of Berlin."),
            HeroCtaTextDe = GetTitleDe(dict, "HeroCta", "Termin anfragen"),
            HeroCtaTextEn = GetTitleEn(dict, "HeroCta", "Book Appointment"),
            HeroCtaLink = GetContentDe(dict, "HeroCta", "/contact"),

            WhyChooseUsTitleDe = GetTitleDe(dict, "WhyUs", "Warum Berlin Car Care?"),
            WhyChooseUsTitleEn = GetTitleEn(dict, "WhyUs", "Why Choose Berlin Car Care?"),
            WhyChooseUsDescDe = GetContentDe(dict, "WhyUs", "Höchste Präzision, schonende Verfahren und kompromisslose Materialqualität für den Werterhalt Ihres Fahrzeugs."),
            WhyChooseUsDescEn = GetContentEn(dict, "WhyUs", "Maximum precision, gentle methodologies, and uncompromising quality for lasting vehicle value."),

            BottomCtaTitleDe = GetTitleDe(dict, "BottomCta", "Bereit für den perfekten Glanz?"),
            BottomCtaTitleEn = GetTitleEn(dict, "BottomCta", "Ready for Showroom Perfection?"),
            BottomCtaSubtitleDe = GetContentDe(dict, "BottomCta", "Kontaktieren Sie uns jetzt für ein persönliches Beratungsgespräch oder buchen Sie Ihren Wunschtermin."),
            BottomCtaSubtitleEn = GetContentEn(dict, "BottomCta", "Contact our detailing team today or schedule a tailored session for your automobile."),
            BottomCtaButtonDe = GetTitleDe(dict, "BottomCtaBtn", "Jetzt Termin vereinbaren"),
            BottomCtaButtonEn = GetTitleEn(dict, "BottomCtaBtn", "Schedule Your Session")
        };

        return View(model);
    }

    [HttpPost("Home")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Home(HomePageContentViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var items = await _context.PageContents.Where(p => p.PageKey == "Home").ToListAsync();
        var dict = items.ToDictionary(p => p.SectionKey, p => p);

        await SaveSectionAsync(dict, "Home", "Hero", model.HeroTitleDe, model.HeroTitleEn, model.HeroSubtitleDe, model.HeroSubtitleEn);
        await SaveSectionAsync(dict, "Home", "HeroCta", model.HeroCtaTextDe, model.HeroCtaTextEn, model.HeroCtaLink, model.HeroCtaLink);
        await SaveSectionAsync(dict, "Home", "WhyUs", model.WhyChooseUsTitleDe, model.WhyChooseUsTitleEn, model.WhyChooseUsDescDe, model.WhyChooseUsDescEn);
        await SaveSectionAsync(dict, "Home", "BottomCta", model.BottomCtaTitleDe, model.BottomCtaTitleEn, model.BottomCtaSubtitleDe, model.BottomCtaSubtitleEn);
        await SaveSectionAsync(dict, "Home", "BottomCtaBtn", model.BottomCtaButtonDe, model.BottomCtaButtonEn, "", "");

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Home page content updated successfully.";
        return RedirectToAction(nameof(Home));
    }

    // --- 2. ABOUT PAGE ---
    [HttpGet("About")]
    public async Task<IActionResult> About()
    {
        var items = await _context.PageContents.Where(p => p.PageKey == "About").ToListAsync();
        var dict = items.ToDictionary(p => p.SectionKey, p => p);

        var model = new AboutPageContentViewModel
        {
            PageTitleDe = GetTitleDe(dict, "Header", "Über Berlin Car Care"),
            PageTitleEn = GetTitleEn(dict, "Header", "About Berlin Car Care"),
            SubtitleDe = GetContentDe(dict, "Header", "Ihre verlässlichen Partner für meisterhafte Fahrzeugpflege und Werterhalt."),
            SubtitleEn = GetContentEn(dict, "Header", "Committed to delivering peerless automotive cosmetics and detailing in Berlin."),
            CommitmentHeadingDe = GetTitleDe(dict, "Commitment", "Unser Qualitätsversprechen"),
            CommitmentHeadingEn = GetTitleEn(dict, "Commitment", "Our Commitment to Quality"),
            BodyDe = GetContentDe(dict, "Commitment", "<p>Berlin Car Care steht für kompromisslose Qualität bei der Fahrzeugreinigung und Lackveredelung. Unser Betrieb in Berlin bietet privaten Fahrzeughaltern sowie anspruchsvollen Sammlern ein Aufbereitungsniveau, das weit über herkömmliche Autowaschanlagen hinausgeht.</p>"),
            BodyEn = GetContentEn(dict, "Commitment", "<p>Located in Berlin, Berlin Car Care was founded with a singular mission: to provide vehicle owners with a standard of cleanliness, paint correction, and protection that goes far beyond ordinary car washes.</p>"),

            Feature1TitleDe = GetTitleDe(dict, "Feature1", "Modernste Technologie"),
            Feature1TitleEn = GetTitleEn(dict, "Feature1", "Modern Equipment"),
            Feature1DescDe = GetContentDe(dict, "Feature1", "Wir setzen auf industrielle Sprühextraktionsgeräte, Heißdampf und präzise Lackschichtenmessung."),
            Feature1DescEn = GetContentEn(dict, "Feature1", "We utilize commercial extraction cleaners, temperature-controlled steam machines, and high-precision paint depth gauges."),

            Feature2TitleDe = GetTitleDe(dict, "Feature2", "Zertifizierte Pflegeprodukte"),
            Feature2TitleEn = GetTitleEn(dict, "Feature2", "Certified Materials"),
            Feature2DescDe = GetContentDe(dict, "Feature2", "Ausschließlich pH-neutrale Reiniger und Spitzenwachse schonen Materialien und Umwelt nachhaltig."),
            Feature2DescEn = GetContentEn(dict, "Feature2", "Only pH-neutral, environmentally conscious shampoos and premier German/Swiss waxes are applied to your paint.")
        };

        return View(model);
    }

    [HttpPost("About")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> About(AboutPageContentViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var items = await _context.PageContents.Where(p => p.PageKey == "About").ToListAsync();
        var dict = items.ToDictionary(p => p.SectionKey, p => p);

        await SaveSectionAsync(dict, "About", "Header", model.PageTitleDe, model.PageTitleEn, model.SubtitleDe, model.SubtitleEn);
        await SaveSectionAsync(dict, "About", "Commitment", model.CommitmentHeadingDe, model.CommitmentHeadingEn, model.BodyDe, model.BodyEn);
        await SaveSectionAsync(dict, "About", "Feature1", model.Feature1TitleDe, model.Feature1TitleEn, model.Feature1DescDe, model.Feature1DescEn);
        await SaveSectionAsync(dict, "About", "Feature2", model.Feature2TitleDe, model.Feature2TitleEn, model.Feature2DescDe, model.Feature2DescEn);

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "About page content updated successfully.";
        return RedirectToAction(nameof(About));
    }

    // --- 3. LEGAL PAGES (Impressum & Privacy) ---
    [HttpGet("Legal")]
    public async Task<IActionResult> Legal()
    {
        var items = await _context.PageContents.Where(p => p.PageKey == "Legal").ToListAsync();
        var dict = items.ToDictionary(p => p.SectionKey, p => p);

        var model = new LegalPagesContentViewModel
        {
            ImpressumDe = GetContentDe(dict, "Impressum", "<h1>Impressum</h1><p>Angaben gemäß § 5 TMG: Berlin Car Care & Detailing GmbH, Kurfürstendamm 120, 10711 Berlin.</p>"),
            ImpressumEn = GetContentEn(dict, "Impressum", "<h1>Legal Notice / Impressum</h1><p>Information according to German Telemedia Act: Berlin Car Care & Detailing GmbH, Kurfürstendamm 120, 10711 Berlin, Germany.</p>"),
            PrivacyDe = GetContentDe(dict, "Privacy", "<h1>Datenschutzerklärung</h1><p>Wir nehmen den Schutz Ihrer persönlichen Daten sehr ernst...</p>"),
            PrivacyEn = GetContentEn(dict, "Privacy", "<h1>Privacy Policy</h1><p>We treat your personal data with utmost confidentiality in accordance with GDPR...</p>")
        };

        return View(model);
    }

    [HttpPost("Legal")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Legal(LegalPagesContentViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var items = await _context.PageContents.Where(p => p.PageKey == "Legal").ToListAsync();
        var dict = items.ToDictionary(p => p.SectionKey, p => p);

        await SaveSectionAsync(dict, "Legal", "Impressum", "Impressum", "Legal Notice", model.ImpressumDe, model.ImpressumEn);
        await SaveSectionAsync(dict, "Legal", "Privacy", "Datenschutz", "Privacy Policy", model.PrivacyDe, model.PrivacyEn);

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Legal pages content updated successfully.";
        return RedirectToAction(nameof(Legal));
    }

    // --- 4. FOOTER CONTENT ---
    [HttpGet("Footer")]
    public async Task<IActionResult> Footer()
    {
        var items = await _context.PageContents.Where(p => p.PageKey == "Footer").ToListAsync();
        var dict = items.ToDictionary(p => p.SectionKey, p => p);

        var model = new FooterContentViewModel
        {
            TaglineDe = GetContentDe(dict, "Tagline", "Ihre Experten für professionelle Fahrzeugaufbereitung, Tiefenreinigung, Lackpolitur und Detailing im Herzen von Berlin. Höchste Qualität und Liebe zum Detail."),
            TaglineEn = GetContentEn(dict, "Tagline", "Your certified specialists for professional automotive detailing, interior deep cleaning, high-gloss paint polishing, and pre-sale preparation in the center of Berlin."),
            CopyrightDe = GetContentDe(dict, "Copyright", "Alle Rechte vorbehalten."),
            CopyrightEn = GetContentEn(dict, "Copyright", "All rights reserved.")
        };

        return View(model);
    }

    [HttpPost("Footer")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Footer(FooterContentViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var items = await _context.PageContents.Where(p => p.PageKey == "Footer").ToListAsync();
        var dict = items.ToDictionary(p => p.SectionKey, p => p);

        await SaveSectionAsync(dict, "Footer", "Tagline", "Tagline", "Tagline", model.TaglineDe, model.TaglineEn);
        await SaveSectionAsync(dict, "Footer", "Copyright", "Copyright", "Copyright", model.CopyrightDe, model.CopyrightEn);

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Footer content updated successfully.";
        return RedirectToAction(nameof(Footer));
    }

    // --- Helper Methods ---
    private static string GetTitleDe(Dictionary<string, PageContent> dict, string section, string def)
        => dict.TryGetValue(section, out var p) && !string.IsNullOrWhiteSpace(p.TitleDe) ? p.TitleDe : def;

    private static string GetTitleEn(Dictionary<string, PageContent> dict, string section, string def)
        => dict.TryGetValue(section, out var p) && !string.IsNullOrWhiteSpace(p.TitleEn) ? p.TitleEn : def;

    private static string GetContentDe(Dictionary<string, PageContent> dict, string section, string def)
        => dict.TryGetValue(section, out var p) && !string.IsNullOrWhiteSpace(p.ContentDe) ? p.ContentDe : def;

    private static string GetContentEn(Dictionary<string, PageContent> dict, string section, string def)
        => dict.TryGetValue(section, out var p) && !string.IsNullOrWhiteSpace(p.ContentEn) ? p.ContentEn : def;

    private async Task SaveSectionAsync(Dictionary<string, PageContent> dict, string pageKey, string sectionKey, string titleDe, string titleEn, string contentDe, string contentEn)
    {
        if (dict.TryGetValue(sectionKey, out var item))
        {
            item.TitleDe = titleDe;
            item.TitleEn = titleEn;
            item.ContentDe = contentDe;
            item.ContentEn = contentEn;
            item.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var newItem = new PageContent
            {
                PageKey = pageKey,
                SectionKey = sectionKey,
                TitleDe = titleDe,
                TitleEn = titleEn,
                ContentDe = contentDe,
                ContentEn = contentEn,
                UpdatedAt = DateTime.UtcNow
            };
            await _context.PageContents.AddAsync(newItem);
            dict[sectionKey] = newItem;
        }
    }
}
