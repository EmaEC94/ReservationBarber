using CRM.Infrastructure.Persistences.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net.Mail;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace CRM.Infrastructure.Persistences.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _configuration;
        public EmailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(MimeMessage message)
        {
            using var client = new SmtpClient();


            string smtpServer = _configuration["EmailSettings:SmtpServer"]!;
            int port = int.Parse(_configuration["EmailSettings:Port"]!);
            string username = _configuration["EmailSettings:Username"]!;
            string password = _configuration["EmailSettings:Password"]!;

            var secureSocketOption = port == 465 ? MailKit.Security.SecureSocketOptions.SslOnConnect : MailKit.Security.SecureSocketOptions.StartTls;

            await client.ConnectAsync(smtpServer, port, secureSocketOption);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
