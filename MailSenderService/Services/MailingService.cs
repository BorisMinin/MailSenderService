using MailKit.Net.Smtp;
using MailKit.Security;
using MailSenderService.Models;
using MailSenderService.Settings;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailSenderService.Services
{
    public class MailingService : IMailingService
    {
        private readonly MailingSettings _mailingSettings;

        public MailingService(IOptions<MailingSettings> mailingSettings) => _mailingSettings = mailingSettings.Value;

        public async Task SendEmailAsync(MailMessage mailMessage, CancellationToken token)
        {
            // Create email message.
            var email = CreateEmailMessage(mailMessage);

            // Add attachments.
            AddAttachmentsToEmail(email, mailMessage);

            // Send email.
            await SendEmail(email, token);
        }
        private MimeMessage CreateEmailMessage(MailMessage mailMessage)
        {
            var email = new MimeMessage();

            email.Sender = MailboxAddress.Parse(_mailingSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailMessage.ToEmail));
            email.Subject = mailMessage.Subject;
            email.Body = new BodyBuilder { HtmlBody = mailMessage.Body }.ToMessageBody();

            return email;
        }

        private void AddAttachmentsToEmail(MimeMessage email, MailMessage mailMessage)
        {
            if (mailMessage.Attachments != null)
            {
                var builder = new BodyBuilder();
                foreach (var file in mailMessage.Attachments)
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

            await smtp.ConnectAsync(_mailingSettings.Host, _mailingSettings.Port, SecureSocketOptions.StartTls, token);
            await smtp.AuthenticateAsync(_mailingSettings.Mail, _mailingSettings.Password, token);
            await smtp.SendAsync(email, token);
            await smtp.DisconnectAsync(true, token);
        }
    }
}