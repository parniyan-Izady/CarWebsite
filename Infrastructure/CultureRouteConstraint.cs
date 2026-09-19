using Microsoft.AspNetCore.Routing;

namespace CarWashWebsite.Infrastructure;

public class CultureRouteConstraint : IRouteConstraint
{
    private static readonly HashSet<string> AllowedCultures = new(StringComparer.OrdinalIgnoreCase)
    {
        "de", "en"
    };

    public bool Match(
        HttpContext? httpContext,
        IRouter? route,
        string routeKey,
        RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var val) && val is string culture)
        {
            return AllowedCultures.Contains(culture);
        }
        return false;
    }
}
