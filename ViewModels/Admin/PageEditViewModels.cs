using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CarWashWebsite.ViewModels.Admin;

public class HomePageContentViewModel
{
    // Hero Section
    [Display(Name = "Hero Title (DE)")]
    public string HeroTitleDe { get; set; } = string.Empty;

    [Display(Name = "Hero Title (EN)")]
    public string HeroTitleEn { get; set; } = string.Empty;

    [Display(Name = "Hero Subtitle (DE)")]
    public string HeroSubtitleDe { get; set; } = string.Empty;

    [Display(Name = "Hero Subtitle (EN)")]
    public string HeroSubtitleEn { get; set; } = string.Empty;

    [Display(Name = "CTA Button Text (DE)")]
    public string HeroCtaTextDe { get; set; } = string.Empty;

    [Display(Name = "CTA Button Text (EN)")]
    public string HeroCtaTextEn { get; set; } = string.Empty;

    [Display(Name = "CTA Button Link")]
    public string HeroCtaLink { get; set; } = "/contact";

    // Why Choose Us
    [Display(Name = "Why Choose Us Title (DE)")]
    public string WhyChooseUsTitleDe { get; set; } = string.Empty;

    [Display(Name = "Why Choose Us Title (EN)")]
    public string WhyChooseUsTitleEn { get; set; } = string.Empty;

    [Display(Name = "Why Choose Us Description (DE)")]
    public string WhyChooseUsDescDe { get; set; } = string.Empty;

    [Display(Name = "Why Choose Us Description (EN)")]
    public string WhyChooseUsDescEn { get; set; } = string.Empty;

    // Bottom CTA Band
    [Display(Name = "Bottom CTA Title (DE)")]
    public string BottomCtaTitleDe { get; set; } = string.Empty;

    [Display(Name = "Bottom CTA Title (EN)")]
    public string BottomCtaTitleEn { get; set; } = string.Empty;

    [Display(Name = "Bottom CTA Subtitle (DE)")]
    public string BottomCtaSubtitleDe { get; set; } = string.Empty;

    [Display(Name = "Bottom CTA Subtitle (EN)")]
    public string BottomCtaSubtitleEn { get; set; } = string.Empty;

    [Display(Name = "Bottom CTA Button (DE)")]
    public string BottomCtaButtonDe { get; set; } = string.Empty;

    [Display(Name = "Bottom CTA Button (EN)")]
    public string BottomCtaButtonEn { get; set; } = string.Empty;
}

public class AboutPageContentViewModel
{
    [Display(Name = "Page Title (DE)")]
    public string PageTitleDe { get; set; } = string.Empty;

    [Display(Name = "Page Title (EN)")]
    public string PageTitleEn { get; set; } = string.Empty;

    [Display(Name = "Intro Subheading (DE)")]
    public string SubtitleDe { get; set; } = string.Empty;

    [Display(Name = "Intro Subheading (EN)")]
    public string SubtitleEn { get; set; } = string.Empty;

    [Display(Name = "Main Body (German) - Rich Text")]
    public string BodyDe { get; set; } = string.Empty;

    [Display(Name = "Main Body (English) - Rich Text")]
    public string BodyEn { get; set; } = string.Empty;

    [Display(Name = "Commitment Section Heading (DE)")]
    public string CommitmentHeadingDe { get; set; } = string.Empty;

    [Display(Name = "Commitment Section Heading (EN)")]
    public string CommitmentHeadingEn { get; set; } = string.Empty;

    // Feature 1
    public string Feature1TitleDe { get; set; } = string.Empty;
    public string Feature1TitleEn { get; set; } = string.Empty;
    public string Feature1DescDe { get; set; } = string.Empty;
    public string Feature1DescEn { get; set; } = string.Empty;

    // Feature 2
    public string Feature2TitleDe { get; set; } = string.Empty;
    public string Feature2TitleEn { get; set; } = string.Empty;
    public string Feature2DescDe { get; set; } = string.Empty;
    public string Feature2DescEn { get; set; } = string.Empty;
}

public class LegalPagesContentViewModel
{
    [Display(Name = "Impressum Content (German) - Rich Text")]
    public string ImpressumDe { get; set; } = string.Empty;

    [Display(Name = "Impressum Content (English) - Rich Text")]
    public string ImpressumEn { get; set; } = string.Empty;

    [Display(Name = "Privacy Policy Content (German) - Rich Text")]
    public string PrivacyDe { get; set; } = string.Empty;

    [Display(Name = "Privacy Policy Content (English) - Rich Text")]
    public string PrivacyEn { get; set; } = string.Empty;
}

public class FooterContentViewModel
{
    [Display(Name = "Footer Tagline / Intro (German)")]
    public string TaglineDe { get; set; } = string.Empty;

    [Display(Name = "Footer Tagline / Intro (English)")]
    public string TaglineEn { get; set; } = string.Empty;

    [Display(Name = "Copyright Notice (DE)")]
    public string CopyrightDe { get; set; } = string.Empty;

    [Display(Name = "Copyright Notice (EN)")]
    public string CopyrightEn { get; set; } = string.Empty;
}
