using MailKit.Security;
using MailSenderService.Models;
using MailSenderService.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace MailSenderService.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IConfiguration _configuration;

        public MailService(IOptions<MailSettings> mailSettings, IConfiguration configuration)
        {
            _mailSettings = mailSettings.Value;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(MailMessage mailRequest, CancellationToken token)
        {
            Console.WriteLine(_mailSettings.Host);
            Console.WriteLine(_configuration.GetSection("MailSettings:Port"));

            // Create email message.
            var email = CreateEmailMessage(mailRequest);

            // Add attachments.
            AddAttachmentsToEmail(email, mailRequest);

            // Send email.
            await SendEmail(email, token);
        }

        private MimeMessage CreateEmailMessage(MailMessage mailRequest)
        {
            var email = new MimeMessage();

            Console.WriteLine(_mailSettings.Host);

            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            email.Body = new BodyBuilder { HtmlBody = mailRequest.Body }.ToMessageBody();

            return email;
        }

        private void AddAttachmentsToEmail(MimeMessage email, MailMessage mailRequest)
        {
            if (mailRequest.Attachments != null)
            {
                var builder = new BodyBuilder();
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
                email.Body = builder.ToMessageBody();
            }
        }

        private async Task SendEmail(MimeMessage email, CancellationToken token)
        {
            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls, token);
            await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password, token);
            await smtp.SendAsync(email, token);
            await smtp.DisconnectAsync(true, token);
        }
    }
}