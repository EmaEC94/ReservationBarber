namespace CRM.Application.Interfaces
{
    public interface IEmailApplication
    {
        Task SendEmailAsync(string toEmail, string subject, string body, string attachmentPath = null);

    }
}
