using CarWashWebsite.Data;
using CarWashWebsite.Extensions;
using CarWashWebsite.Infrastructure;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Clean Architecture: Modular service registration via Extensions
builder.Services.AddCustomDatabase(builder.Configuration);
builder.Services.AddCustomIdentity();
builder.Services.AddCustomBusinessServices();
builder.Services.AddCustomLocalization();
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("culture", typeof(CultureRouteConstraint));
    options.LowercaseUrls = true;
});
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.AreaViewLocationFormats.Add("/Views/{2}/{1}/{0}.cshtml");
        options.AreaViewLocationFormats.Add("/Views/{2}/Shared/{0}.cshtml");
    });

var app = builder.Build();

// Seed Database automatically on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbInitializer.SeedAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Localization Middleware
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

app.UseAuthentication();
app.UseAuthorization();

// 1. Admin Area Route
app.MapAreaControllerRoute(
    name: "admin_area",
    areaName: "Admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

// 2. Multilingual Service Detail Route (/de/services/slug or /en/services/slug)
app.MapControllerRoute(
    name: "localized_service_details",
    pattern: "{culture:culture}/services/{slug}",
    defaults: new { controller = "Services", action = "Details" });

// 3. Multilingual Public Route (/de/... or /en/...)
app.MapControllerRoute(
    name: "localized",
    pattern: "{culture:culture}/{controller=Home}/{action=Index}/{id?}");

// 3. Fallback Route: Redirects root "/" to default language "/de"
app.MapGet("/", context =>
{
    context.Response.Redirect("/de", permanent: false);
    return Task.CompletedTask;
});

// 4. Default unlocalized fallback
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
