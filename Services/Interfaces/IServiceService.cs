using CarWashWebsite.Data.Entities;

namespace CarWashWebsite.Services.Interfaces;

public interface IServiceService
{
    Task<IEnumerable<Service>> GetAllServicesAsync(bool onlyActive = false);
    Task<Service?> GetServiceByIdAsync(int id);
    Task<Service?> GetServiceBySlugAsync(string slug, string culture);
    Task<Service> CreateServiceAsync(Service service);
    Task<Service?> UpdateServiceAsync(Service service);
    Task<bool> DeleteServiceAsync(int id);
    Task<bool> ToggleServiceStatusAsync(int id);
}
