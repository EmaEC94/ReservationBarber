using MimeKit;

namespace CRM.Infrastructure.Persistences.Interfaces
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(MimeMessage message);
    }
}
