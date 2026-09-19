using CarWashWebsite.Data;
using CarWashWebsite.Data.Entities;
using CarWashWebsite.Infrastructure;
using CarWashWebsite.Services.Implementations;
using CarWashWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CarWashWebsite.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Admin/Account/Login";
            options.LogoutPath = "/Admin/Account/Logout";
            options.AccessDeniedPath = "/Admin/Account/AccessDenied";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
            options.SlidingExpiration = true;
        });

        return services;
    }

    public static IServiceCollection AddCustomBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IContentService, ContentService>();
        services.AddScoped<ISeoService, SeoService>();
        return services;
    }

    public static IServiceCollection AddCustomLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        var supportedCultures = new[]
        {
            new CultureInfo("de"),
            new CultureInfo("en")
        };

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("de");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider { Options = options });
        });

        return services;
    }
}
