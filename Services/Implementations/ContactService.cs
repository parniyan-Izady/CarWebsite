using CarWashWebsite.Data;
using CarWashWebsite.Data.Entities;
using CarWashWebsite.Services.Interfaces;

namespace CarWashWebsite.Services.Implementations;

public class ContactService : IContactService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ContactService> _logger;

    public ContactService(ApplicationDbContext context, ILogger<ContactService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> SubmitMessageAsync(ContactMessage message)
    {
        try
        {
            message.CreatedAt = DateTime.UtcNow;
            message.IsRead = false;
            message.IsArchived = false;

            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New contact message received from {Email}", message.Email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist contact message from {Email}", message.Email);
            return false;
        }
    }
}
