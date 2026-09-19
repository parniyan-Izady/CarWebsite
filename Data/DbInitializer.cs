using CarWashWebsite.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarWashWebsite.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();

        // 1. Seed Roles & Admin User
        const string adminRole = "Admin";
        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        const string adminEmail = "admin@berlincarcare.de";
        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Administrator Berlin Car Care",
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(adminUser, "Admin@Secure2026!");
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }

        // 2. Seed All 14 Services if empty
        if (!await context.Services.AnyAsync())
        {
            var services = new List<Service>
            {
                new()
                {
                    TitleDe = "Autowäsche & Fahrzeugreinigung",
                    TitleEn = "Car Wash & Cleaning",
                    DescriptionDe = "Professionelle Hand- und Vorwäsche für schonende und rückstandslose Sauberkeit Ihres Fahrzeugs.",
                    DescriptionEn = "Professional hand wash and pre-cleaning ensuring gentle, streak-free cleanliness for your vehicle.",
                    Price = 35.00m,
                    PriceType = PriceType.StartingFrom,
                    SlugDe = "autowaesche-fahrzeugreinigung",
                    SlugEn = "car-wash-cleaning",
                    SeoTitleDe = "Autowäsche & Fahrzeugreinigung in Berlin | Schonende Handwäsche",
                    SeoTitleEn = "Car Wash & Cleaning in Berlin | Professional Hand Wash",
                    MetaDescriptionDe = "Gründliche und schonende Fahrzeugreinigung & Autowäsche in Berlin. Perfekte Pflege für Lack und Felgen.",
                    MetaDescriptionEn = "Thorough and gentle car washing and exterior cleaning in Berlin. Perfect paint and wheel care.",
                    DisplayOrder = 1,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Innenraumreinigung",
                    TitleEn = "Interior Cleaning",
                    DescriptionDe = "Komplette Reinigung des Innenraums inklusive Armaturen, Scheiben, Türverkleidungen und Fußmatten.",
                    DescriptionEn = "Complete interior cleaning including dashboard, windows, door panels, and floor mats.",
                    Price = 79.00m,
                    PriceType = PriceType.StartingFrom,
                    SlugDe = "innenraumreinigung",
                    SlugEn = "interior-cleaning",
                    SeoTitleDe = "Professionelle Innenraumreinigung Berlin | Frische & Hygiene",
                    SeoTitleEn = "Professional Interior Car Cleaning Berlin | Hygiene & Freshness",
                    MetaDescriptionDe = "Erstklassige Innenraumreinigung für Ihr Auto in Berlin. Beseitigung von Schmutz, Staub und unangenehmen Gerüchen.",
                    MetaDescriptionEn = "First-class car interior cleaning in Berlin. Removal of dirt, dust, and stubborn odors.",
                    DisplayOrder = 2,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Außenreinigung",
                    TitleEn = "Exterior Cleaning",
                    DescriptionDe = "Schonende Außenwäsche mit Aktivschaum, Felgenreinigung, Radkästen und Scheibenpolitur.",
                    DescriptionEn = "Gentle exterior cleaning with active foam, rim detailing, wheel arch wash, and window treatment.",
                    Price = 49.00m,
                    PriceType = PriceType.StartingFrom,
                    SlugDe = "aussenreinigung",
                    SlugEn = "exterior-cleaning",
                    SeoTitleDe = "Gründliche Außenreinigung Berlin | Glanz & Lackschutz",
                    SeoTitleEn = "Thorough Exterior Car Cleaning Berlin | Gloss & Care",
                    MetaDescriptionDe = "Exzellente Außenreinigung in Berlin für alle Fahrzeugtypen. Inklusive intensiver Felgenreinigung.",
                    MetaDescriptionEn = "Excellent exterior detailing and car wash in Berlin for all vehicle types. Includes deep rim cleaning.",
                    DisplayOrder = 3,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Dachreinigung",
                    TitleEn = "Roof Cleaning",
                    DescriptionDe = "Spezielle Pflege und Tiefenreinigung für Autodächer und Cabrio-Verdecke inklusive Imprägnierung.",
                    DescriptionEn = "Specialized deep cleaning and care for vehicle roofs and convertible soft-tops including sealing.",
                    Price = 65.00m,
                    PriceType = PriceType.StartingFrom,
                    SlugDe = "dachreinigung",
                    SlugEn = "roof-cleaning",
                    SeoTitleDe = "Dachreinigung & Cabrioverdeck-Pflege Berlin",
                    SeoTitleEn = "Vehicle Roof & Convertible Cleaning Berlin",
                    MetaDescriptionDe = "Schonende Reinigung von Fahrzeugdächern und Cabrioverdecken in Berlin mit Langzeitschutz.",
                    MetaDescriptionEn = "Gentle cleaning of vehicle roofs and convertible soft-tops in Berlin with long-term protection.",
                    DisplayOrder = 4,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Autopolitur",
                    TitleEn = "Car Polishing",
                    DescriptionDe = "Mehrstufige Lackpolitur zur Beseitigung von Mattigkeit und leichten Schlieren für brillanten Tiefenglanz.",
                    DescriptionEn = "Multi-stage paint polishing to eliminate haziness and swirls for an intense mirror gloss.",
                    Price = 149.00m,
                    PriceType = PriceType.StartingFrom,
                    SlugDe = "autopolitur",
                    SlugEn = "car-polishing",
                    SeoTitleDe = "Professionelle Autopolitur Berlin | Brillanter Tiefenglanz",
                    SeoTitleEn = "Professional Car Polishing Berlin | Brilliant Deep Gloss",
                    MetaDescriptionDe = "Hochwertige Autopolitur in Berlin. Wir bringen den Originallack Ihres Fahrzeugs wieder zum Strahlen.",
                    MetaDescriptionEn = "High-end car polishing in Berlin. We restore the original radiance and depth of your car's paintwork.",
                    DisplayOrder = 5,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Kratzerentfernung & Politur",
                    TitleEn = "Scratch Removal & Polishing",
                    DescriptionDe = "Gezielte Schleifpolitur zur sicheren Beseitigung oberflächlicher Lackkratzer und Waschstraßenkratzer.",
                    DescriptionEn = "Targeted compound polishing to safely remove light surface scratches and swirl marks.",
                    Price = 89.00m,
                    PriceType = PriceType.StartingFrom,
                    SlugDe = "kratzerentfernung-politur",
                    SlugEn = "scratch-removal-polishing",
                    SeoTitleDe = "Kratzerentfernung & Lackkorrektur Berlin",
                    SeoTitleEn = "Car Scratch Removal & Paint Correction Berlin",
                    MetaDescriptionDe = "Effektive Beseitigung von Lackkratzern und Schrammen in Berlin ohne teures Nachlackieren.",
                    MetaDescriptionEn = "Effective scratch removal and paint correction in Berlin without costly repainting.",
                    DisplayOrder = 6,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Polster- und Sitzreinigung",
                    TitleEn = "Seat & Upholstery Cleaning",
                    DescriptionDe = "Sprühextraktionsreinigung für Stoffsitze und schonende Tiefenpflege für edles Leder.",
                    DescriptionEn = "Spray-extraction deep cleaning for fabric seats and specialized treatment for fine leather.",
                    Price = 80.00m,
                    PriceType = PriceType.StartingFrom,
                    SlugDe = "polster-sitzreinigung",
                    SlugEn = "seat-upholstery-cleaning",
                    SeoTitleDe = "Polster- und Sitzreinigung Berlin | Tiefenhygiene für Autositze",
                    SeoTitleEn = "Car Seat & Upholstery Cleaning Berlin | Deep Hygiene",
                    MetaDescriptionDe = "Fleckenfreie und hygienische Sitzreinigung für Autoleder und Stoffpolster in Berlin.",
                    MetaDescriptionEn = "Stain-free and hygienic seat cleaning for leather and fabric upholstery in Berlin.",
                    DisplayOrder = 7,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Innenraumreinigung mit Spezialstaubsauger",
                    TitleEn = "Interior Vacuum & Cleaning",
                    DescriptionDe = "Intensives Aussaugen aller Nischen, Sitze und Teppiche mit hochleistungsfähigen Profi-Saugern.",
                    DescriptionEn = "Intensive vacuuming of all niches, seats, and floor carpets using high-power industrial vacuums.",
                    Price = 45.00m,
                    PriceType = PriceType.Fixed,
                    SlugDe = "innenraumreinigung-spezialstaubsauger",
                    SlugEn = "interior-vacuum-cleaning",
                    SeoTitleDe = "Intensive Autosauger-Reinigung Berlin",
                    SeoTitleEn = "Intensive Interior Vacuuming Berlin",
                    MetaDescriptionDe = "Gründliches Aussaugen des Fahrzeuginnenraums mit Spezialgeräten bis in die kleinsten Ecken.",
                    MetaDescriptionEn = "Thorough interior vacuuming with specialized equipment down into every hard-to-reach corner.",
                    DisplayOrder = 8,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Tiefenreinigung & Trocknung",
                    TitleEn = "Deep Interior Cleaning & Drying",
                    DescriptionDe = "Nassreinigung mit modernsten Extraktionsgeräten und anschließender professioneller Schnelltrocknung.",
                    DescriptionEn = "Wet interior extraction cleaning combined with professional rapid drying technology.",
                    Price = 120.00m,
                    PriceType = PriceType.StartingFrom,
                    SlugDe = "tiefenreinigung-trocknung",
                    SlugEn = "deep-interior-cleaning-drying",
                    SeoTitleDe = "Tiefenreinigung & Fahrzeugtrocknung Berlin",
                    SeoTitleEn = "Deep Interior Cleaning & Fast Drying Berlin",
                    MetaDescriptionDe = "Beseitigung tiefsitzender Verschmutzungen mit direkter Trocknung ohne Feuchtigkeitsrisiko.",
                    MetaDescriptionEn = "Eliminate deep-seated stains and grime with immediate controlled drying.",
                    DisplayOrder = 9,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Fahrzeugaufbereitung / Detailing",
                    TitleEn = "Car Detailing",
                    DescriptionDe = "Die Königsklasse der Fahrzeugpflege: Umfassende Aufbereitung von Lack, Felgen, Glas und Interieur.",
                    DescriptionEn = "The premier standard of car care: complete transformation of paint, wheels, glass, and interior.",
                    Price = 249.00m,
                    PriceType = PriceType.StartingFrom,
                    SlugDe = "fahrzeugaufbereitung-detailing",
                    SlugEn = "car-detailing",
                    SeoTitleDe = "Professionelles Car Detailing Berlin | Premium Fahrzeugaufbereitung",
                    SeoTitleEn = "Professional Car Detailing Berlin | Premium Automotive Care",
                    MetaDescriptionDe = "Exklusives Detailing in Berlin für höchste Ansprüche. Wertsteigernd, detailverliebt und langanhaltend.",
                    MetaDescriptionEn = "Exclusive car detailing in Berlin for discerning car enthusiasts. Value-boosting and meticulous.",
                    DisplayOrder = 10,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Wachs & Glanz",
                    TitleEn = "Wax & Shine",
                    DescriptionDe = "Hochwertiges Carnauba- oder synthetisches Schutzwachs für exzellenten Abperleffekt und Glanz.",
                    DescriptionEn = "High-grade Carnauba or synthetic protective wax delivering brilliant beading and radiant gloss.",
                    Price = 69.00m,
                    PriceType = PriceType.StartingFrom,
                    SlugDe = "wachs-glanz",
                    SlugEn = "wax-shine",
                    SeoTitleDe = "Lackversiegelung & Wachs Berlin | Wachs & Glanz",
                    SeoTitleEn = "Car Waxing & Protective Shine Berlin",
                    MetaDescriptionDe = "Schützen Sie Ihren Autolack vor Witterungseinflüssen mit unserer Premium-Wachspflege in Berlin.",
                    MetaDescriptionEn = "Protect your paintwork against harsh weather elements with premium wax treatments in Berlin.",
                    DisplayOrder = 11,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Dampfreinigung",
                    TitleEn = "Steam Cleaning",
                    DescriptionDe = "Hygienische Dampfreinigung bei hohen Temperaturen: Bakterien, Keime und Gerüche werden ohne Chemie gelöst.",
                    DescriptionEn = "Hygienic high-temperature steam cleaning: removes bacteria, germs, and odors completely chemical-free.",
                    Price = 95.00m,
                    PriceType = PriceType.StartingFrom,
                    SlugDe = "dampfreinigung",
                    SlugEn = "steam-cleaning",
                    SeoTitleDe = "Ökologische Dampfreinigung fürs Auto Berlin",
                    SeoTitleEn = "Eco-Friendly Car Steam Cleaning Berlin",
                    MetaDescriptionDe = "Porentiefe Desinfektion und Reinigung mit Heißdampf für Sitze, Lüftungskanäle und Motorraum in Berlin.",
                    MetaDescriptionEn = "Deep thermal disinfection and cleaning using steam for upholstery, air vents, and engine bays.",
                    DisplayOrder = 12,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Schnellwäsche mit Maschine",
                    TitleEn = "Quick Machine Wash",
                    DescriptionDe = "Schnelle, unkomplizierte maschinelle Wäsche für zwischendurch bei vollem Glanzergebnis.",
                    DescriptionEn = "Fast, efficient machine-assisted wash for a quick turnaround with gleaming results.",
                    Price = 25.00m,
                    PriceType = PriceType.Fixed,
                    SlugDe = "schnellwaesche-mit-maschine",
                    SlugEn = "quick-machine-wash",
                    SeoTitleDe = "Schnelle Autowäsche Berlin | Express Pflege",
                    SeoTitleEn = "Quick Machine Car Wash Berlin | Express Wash",
                    MetaDescriptionDe = "Die schnelle Lösung für ein sauberes Auto: Schnelle maschinelle Wäsche in Berlin.",
                    MetaDescriptionEn = "The fast solution for a clean vehicle: quick machine wash in Berlin.",
                    DisplayOrder = 13,
                    IsActive = true
                },
                new()
                {
                    TitleDe = "Fahrzeugaufbereitung vor dem Verkauf",
                    TitleEn = "Pre-Sale Car Detailing",
                    DescriptionDe = "Maximieren Sie den Verkaufswert Ihres Autos mit unserer umfassenden Rundum-Verkaufsvorbereitung.",
                    DescriptionEn = "Maximize your car's market resale value with our comprehensive, detailed pre-sale makeover.",
                    Price = 299.00m,
                    PriceType = PriceType.StartingFrom,
                    SlugDe = "fahrzeugaufbereitung-vor-dem-verkauf",
                    SlugEn = "pre-sale-car-detailing",
                    SeoTitleDe = "Auto-Aufbereitung vor dem Verkauf Berlin | Maximaler Verkaufspreis",
                    SeoTitleEn = "Pre-Sale Car Detailing Berlin | Maximize Your Resale Value",
                    MetaDescriptionDe = "Erhöhen Sie den Verkaufswert Ihres Fahrzeugs nachweislich mit unserer spezialisierten Verkaufsvorbereitung in Berlin.",
                    MetaDescriptionEn = "Proven increase in your car's resale appeal and price with our dedicated pre-sale detailing in Berlin.",
                    DisplayOrder = 14,
                    IsActive = true
                }
            };

            await context.Services.AddRangeAsync(services);
            await context.SaveChangesAsync();
        }

        // 3. Seed Site Settings (Berlin Contact & Social)
        if (!await context.SiteSettings.AnyAsync())
        {
            var settings = new List<SiteSetting>
            {
                new() { Key = "BusinessName", ValueDe = "Berlin Car Care & Detailing", ValueEn = "Berlin Car Care & Detailing", Group = "General" },
                new() { Key = "Phone", ValueDe = "+49 30 12345678", ValueEn = "+49 30 12345678", Group = "Contact" },
                new() { Key = "Email", ValueDe = "info@berlincarcare.de", ValueEn = "info@berlincarcare.de", Group = "Contact" },
                new() { Key = "Address", ValueDe = "Kurfürstendamm 120, 10711 Berlin", ValueEn = "Kurfürstendamm 120, 10711 Berlin, Germany", Group = "Contact" },
                new() { Key = "OpeningHours", ValueDe = "Mo - Sa: 08:00 - 18:00 Uhr", ValueEn = "Mon - Sat: 08:00 AM - 06:00 PM", Group = "Contact" },
                new() { Key = "Instagram", ValueDe = "https://instagram.com/berlincarcare", ValueEn = "https://instagram.com/berlincarcare", Group = "Social" }
            };

            await context.SiteSettings.AddRangeAsync(settings);
            await context.SaveChangesAsync();
        }

        // 4. Seed FAQs
        if (!await context.FaqItems.AnyAsync())
        {
            var faqs = new List<FaqItem>
            {
                new()
                {
                    QuestionDe = "Wie lange dauert eine professionelle Innenraumreinigung?",
                    QuestionEn = "How long does a professional interior cleaning take?",
                    AnswerDe = "In der Regel dauert eine umfassende Innenreinigung zwischen 2 und 4 Stunden, abhängig vom Verschmutzungsgrad des Fahrzeugs.",
                    AnswerEn = "Generally, a comprehensive interior cleaning takes between 2 and 4 hours depending on the vehicle's initial condition.",
                    DisplayOrder = 1,
                    IsActive = true
                },
                new()
                {
                    QuestionDe = "Muss ich im Voraus einen Termin vereinbaren?",
                    QuestionEn = "Do I need to book an appointment in advance?",
                    AnswerDe = "Ja, wir empfehlen eine vorherige Terminabsprache telefonisch oder über unser Online-Kontaktformular, um Wartezeiten zu vermeiden.",
                    AnswerEn = "Yes, we recommend booking in advance via phone or our online contact form to avoid wait times.",
                    DisplayOrder = 2,
                    IsActive = true
                },
                new()
                {
                    QuestionDe = "Lohnt sich die Fahrzeugaufbereitung vor dem Verkauf wirklich?",
                    QuestionEn = "Is pre-sale car detailing really worth the investment?",
                    AnswerDe = "Absolut! Durch gepflegten Lack, hygienische Polster und glänzende Details erzielen Sie erfahrungsgemäß einen deutlich höheren Verkaufserlös.",
                    AnswerEn = "Absolutely! Clean paintwork, fresh upholstery, and spotless details have been proven to significantly increase vehicle resale value.",
                    DisplayOrder = 3,
                    IsActive = true
                }
            };

            await context.FaqItems.AddRangeAsync(faqs);
            await context.SaveChangesAsync();
        }

        // 5. Ensure Module Toggles exist in SiteSettings
        var moduleKeys = new[] { ("EnableBlog", "true"), ("EnableGallery", "true"), ("EnableFaq", "true") };
        foreach (var (k, v) in moduleKeys)
        {
            if (!await context.SiteSettings.AnyAsync(s => s.Key == k))
            {
                await context.SiteSettings.AddAsync(new SiteSetting
                {
                    Key = k,
                    ValueDe = v,
                    ValueEn = v,
                    Group = "Modules"
                });
            }
        }
        await context.SaveChangesAsync();

        // 6. Seed SEO Blog Posts if empty
        if (!await context.BlogPosts.AnyAsync())
        {
            var posts = new List<BlogPost>
            {
                new()
                {
                    TitleDe = "Autopolitur in Perfektion: Swirls & Hologramme sicher entfernen",
                    TitleEn = "Car Polishing Mastery: Removing Swirls and Holograms Safely",
                    SlugDe = "autopolitur-swirls-hologramme-entfernen",
                    SlugEn = "car-polishing-swirl-marks-removal",
                    SummaryDe = "Erfahren Sie, wie professionelle Lackaufbereitung feine Kratzer und Hologramme beseitigt und Ihrem Fahrzeug perfekten Tiefenglanz verleiht.",
                    SummaryEn = "Discover how professional paint correction removes scratches and buffer swirls, restoring flawless mirror reflection to your vehicle.",
                    ContentDe = "<p>Waschanlagenbürsten und unsachgemäße Handwäschen hinterlassen auf jedem Autolack mit der Zeit feine, kreisrunde Mikrokratzer – sogenannte <strong>Swirl Marks</strong>. Im direkten Sonnenlicht wirken diese wie ein grauer Schleier, der dem Lack Tiefe und Brillanz raubt.</p><h3>Die Ursachen für Lackdefekte</h3><p>Schmutzpartikel wirken bei Berührung wie Schmirgelpapier. Bei einer professionellen mehrstufigen Lackkorrektur messen wir zunächst die Klarlackstärke mit Präzisionsmessgeräten. Anschließend wird in mehreren Polierschritten die oberste, beschädigte Schicht im Mikrometerbereich egalisiert.</p><h3>Vorteile der professionellen Maschinenpolitur</h3><ul><li>Vollständige Beseitigung von Waschkratzern und Hologrammen</li><li>Wiederherstellung des originalen Tiefenglanzes</li><li>Optimale Vorbereitung für langanhaltende Schutzversiegelungen</li></ul><p>Wir bei Berlin Car Care setzen modernste Exzenter- und Rotationspolierer sowie silikonfreie Schleifpasten namhafter deutscher Hersteller ein.</p>",
                    ContentEn = "<p>Automatic car washes and improper washing techniques inevitably inflict microscopic circular scratches – known as <strong>swirl marks</strong>. Under direct sunlight, they create an unattractive haze that robs the paintwork of its natural depth and clarity.</p><h3>Understanding Paint Defects</h3><p>Dirt particles act like fine sandpaper when wiped across paint. During our multi-stage paint correction, we first measure the clear coat thickness with digital depth gauges. Next, we systematically level the damaged surface by removing only a minute fraction of the clear coat.</p><h3>Key Benefits of Professional Polishing</h3><ul><li>Total eradication of wash swirls and machine holograms</li><li>Restoration of authentic deep mirror gloss</li><li>Essential foundation for ceramic or wax coatings</li></ul><p>At Berlin Car Care, we utilize dual-action rotary polishers paired with silicone-free German compounds for enduring results.</p>",
                    CoverImagePath = "/uploads/services/polishing.webp",
                    Category = "PaintCare",
                    ReadingTimeMinutes = 4,
                    SeoTitleDe = "Autopolitur Berlin | Swirls & Kratzer entfernen | Berlin Car Care",
                    SeoTitleEn = "Car Polishing in Berlin | Swirl Removal | Berlin Car Care",
                    MetaDescriptionDe = "Expertenratgeber zur professionellen Autopolitur in Berlin. Erfahren Sie, wie wir Hologramme und Swirls schonend entfernen.",
                    MetaDescriptionEn = "Expert guide to professional paint correction in Berlin. Learn how we safely eliminate swirl marks and scratches.",
                    IsPublished = true,
                    PublishedAt = DateTime.UtcNow.AddDays(-5),
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new()
                {
                    TitleDe = "Keramikversiegelung vs. Wachs: Welcher Schutz lohnt sich wirklich?",
                    TitleEn = "Ceramic Coating vs. Car Wax: Which Protection Truly Pays Off?",
                    SlugDe = "keramikversiegelung-vs-wachs-vergleich",
                    SlugEn = "ceramic-coating-vs-wax-comparison",
                    SummaryDe = "Vergleich zwischen traditionellem Carnaubawachs und moderner Keramikversiegelung in Bezug auf Standzeit, Glanz und Pflegeaufwand.",
                    SummaryEn = "A detailed comparison between traditional carnauba wax and modern ceramic coatings regarding longevity, gloss, and maintenance.",
                    ContentDe = "<p>Fahrzeugbesitzer stehen oft vor der Wahl: Traditionelles Naturwachs oder hochmoderne Keramikbeschichtung (SiO2)? Beide Methoden haben klare Vorzüge.</p><h3>Carnaubawachs – Warmer Glanz für Liebhaber</h3><p>Hochwertiges Carnaubawachs erzeugt einen besonders warmen, tiefen Nassglanz. Es ist ideal für Oldtimer oder Schönwetterfahrzeuge, muss jedoch alle 3 bis 6 Monate erneuert werden.</p><h3>Keramikversiegelung – Maximaler Langzeitschutz</h3><p>Eine Keramikversiegelung verbindet sich chemisch mit dem Klarlack. Sie bietet extremen Schutz vor UV-Strahlung, Streusalz, Vogelkot und saurem Regen. Mit einer Standzeit von 2 bis 5 Jahren und extrem hydrophobem Abperleffekt ist sie die wirtschaftlichste Lösung für Alltagsfahrzeuge.</p>",
                    ContentEn = "<p>Car owners frequently wonder whether traditional carnauba wax or cutting-edge ceramic coating (SiO2) is the best choice for their vehicle. Both offer unique qualities.</p><h3>Carnauba Wax – Warm Radiance for Enthusiasts</h3><p>Natural carnauba wax produces an incomparable warm, deep wet-look shine. It is superb for classic and weekend cars, though requires reapplication every 3 to 6 months.</p><h3>Ceramic Coating – Superior Long-Term Shield</h3><p>Ceramic coatings form a molecular chemical bond with your factory clear coat. They offer exceptional resistance against harsh road salt, tree sap, acid rain, and UV damage, lasting 2 to 5 years.</p>",
                    CoverImagePath = "/uploads/services/wax.webp",
                    Category = "Detailing",
                    ReadingTimeMinutes = 5,
                    SeoTitleDe = "Keramikversiegelung vs. Wachs im Vergleich | Berlin Car Care",
                    SeoTitleEn = "Ceramic Coating vs Car Wax Comparison | Berlin Car Care",
                    MetaDescriptionDe = "Was ist besser für Ihr Auto: Keramikversiegelung oder Wachs? Wir vergleichen Schutzwirkung, Glanz und Kosten.",
                    MetaDescriptionEn = "Which is better for your car: ceramic coating or wax? We compare longevity, gloss, and overall value.",
                    IsPublished = true,
                    PublishedAt = DateTime.UtcNow.AddDays(-2),
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new()
                {
                    TitleDe = "Lederpflege im Auto: Risse vermeiden und Wert erhalten",
                    TitleEn = "Car Leather Detailing: Preventing Cracks and Maintaining Value",
                    SlugDe = "lederpflege-auto-tipps-werterhalt",
                    SlugEn = "car-leather-care-tips",
                    SummaryDe = "Richtige Reinigung und Imprägnierung von Autoleder: So bleibt hochwertiges Leder geschmeidig und frei von Flecken.",
                    SummaryEn = "Proper cleaning and conditioning for automotive leather: keep your upholstery supple, matte, and stain-resistant.",
                    ContentDe = "<p>Echtes Leder im Fahrzeuginnenraum vermittelt Luxus und Komfort. Doch Sonneneinstrahlung und Abrieb entziehen dem Leder über die Jahre Feuchtigkeit und Weichmacher. Die Folge: Glänzende, speckige Oberflächen und unansehnliche Bruchstellen.</p><h3>Schritt 1: Tiefenreinigung mit pH-neutralen Schaumreinigern</h3><p>Herkömmliche Haushaltsreiniger greifen die Farbschicht des Leders an. Wir nutzen milde Reiniger mit speziellen Reinigungsbürsten, um Schmutz und Schweiß aus den Poren zu heben.</p><h3>Schritt 2: Rückfettung und UV-Schutz</h3><p>Nach der Trocknung wird eine nährende Lederversiegelung aufgetragen, die das Material geschmeidig hält und das ursprüngliche matte Werksfinish wiederherstellt.</p>",
                    ContentEn = "<p>Genuine leather seats elevate automotive interiors to genuine luxury. Over time, however, heat, sunlight, and everyday friction deplete essential natural oils. The result is an unsightly greasy shine followed by cracking.</p><h3>Step 1: Deep Pore Cleaning</h3><p>Ordinary household cleaners strip the top protective coat. We employ pH-neutral foaming cleansers and soft horsehair brushes to safely dislodge body oils and embedded grime.</p><h3>Step 2: Conditioning and UV Sealing</h3><p>Once dried, we apply specialized conditioners that rehydrate the hide and impart a durable barrier against dye transfer and UV degradation while restoring that desirable matte factory finish.</p>",
                    CoverImagePath = "/uploads/services/interior.webp",
                    Category = "Interior",
                    ReadingTimeMinutes = 3,
                    SeoTitleDe = "Lederpflege im Auto | Risse & Flecken vermeiden | Berlin Car Care",
                    SeoTitleEn = "Car Leather Care & Detailing | Berlin Car Care",
                    MetaDescriptionDe = "Tipps zur professionellen Lederpflege im Auto. Erfahren Sie, wie Sie Leder geschmeidig halten und Risse verhindern.",
                    MetaDescriptionEn = "Expert tips on automotive leather care. Learn how to maintain matte, supple leather and prevent cracking.",
                    IsPublished = true,
                    PublishedAt = DateTime.UtcNow.AddDays(-1),
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            await context.BlogPosts.AddRangeAsync(posts);
            await context.SaveChangesAsync();
        }
    }
}

