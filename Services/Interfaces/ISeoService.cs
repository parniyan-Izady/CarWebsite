using CarWashWebsite.Data.Entities;

namespace CarWashWebsite.Services.Interfaces;

public interface ISeoService
{
    Task<string> GenerateSitemapXmlAsync(string baseUrl);
    string GenerateRobotsTxt(string baseUrl);
    string GenerateLocalBusinessSchema(string baseUrl, string culture, Dictionary<string, string> settings);
    string GenerateServiceSchema(string baseUrl, string culture, Service service);
}
