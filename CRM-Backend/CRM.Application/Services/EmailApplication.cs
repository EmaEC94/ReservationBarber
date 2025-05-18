using CRM.Application.Interfaces;
using CRM.Infrastructure.Persistences.Interfaces;
using MimeKit;

namespace CRM.Application.Services
{
    public class EmailApplication : IEmailApplication
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmailApplication(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body, string attachmentPath = null)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Dsitribuidora R&J", "desarrollo@distribuidora-ryjcr.com"));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder { TextBody = body };

                if (attachmentPath != null)
                {
                    bodyBuilder.Attachments.Add(attachmentPath);
                }

                message.Body = bodyBuilder.ToMessageBody();

                await _unitOfWork.EmailRepository.SendEmailAsync(message);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
