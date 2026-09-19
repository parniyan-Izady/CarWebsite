using CarWashWebsite.Data.Entities;

namespace CarWashWebsite.Services.Interfaces;

public interface IContactService
{
    Task<bool> SubmitMessageAsync(ContactMessage message);
}
