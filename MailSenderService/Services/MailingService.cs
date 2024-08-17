using MailKit.Net.Smtp;
using MailKit.Security;
using MailSenderService.Models;
using MailSenderService.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text.RegularExpressions;

namespace MailSenderService.Services
{
    /// <summary>
    /// Service for handling email operations.
    /// </summary>
    public class MailingService : IMailingService
    {
        private readonly MailingSettings _mailingSettings;

        public MailingService(IOptions<MailingSettings> mailingSettings) => _mailingSettings = mailingSettings.Value;

        /// <summary>
        /// Send email address.
        /// </summary>
        /// <param name="MailRequest">The request containing details for the email to be sent, including recipient, subject, and body.</param>
        /// <param name="token">A CancellationToken to observe while sending the email, allowing for task cancellation.</param>
        /// <returns></returns>
        public async Task SendEmailAsync(MailRequest MailRequest, CancellationToken token)
        {
            // Create email message.
            var email = CreateEMailRequest(MailRequest);

            // Add attachments.
            AddAttachmentsToEmail(email, MailRequest);

            // Send email.
            await SendEmail(email, token);
        }

        /// <summary>
        /// Create a MimeMessage from the provided MailRequest.
        /// </summary>
        /// <param name="MailRequest">The request containing details for the email to be sent, including recipient, subject, and body.</param>
        /// <returns></returns>
        private MimeMessage CreateEMailRequest(MailRequest MailRequest)
        {
            var email = new MimeMessage();

            email.Sender = MailboxAddress.Parse(_mailingSettings.Mail);
            email.To.Add(MailboxAddress.Parse(MailRequest.ToEmail));
            email.Subject = MailRequest.Subject;
            email.Body = new BodyBuilder { HtmlBody = MailRequest.Body }.ToMessageBody();

            return email;
        }

        /// <summary>
        /// Add attachments to the email based on the provided MailRequest.
        /// </summary>
        /// <param name="email">The MimeMessage to which attachments will be added.</param>
        /// <param name="MailRequest">The request containing the file attachments.</param>
        private void AddAttachmentsToEmail(MimeMessage email, MailRequest MailRequest)
        {
            if (MailRequest.Attachments != null)
            {
                var builder = new BodyBuilder();
                foreach (var file in MailRequest.Attachments)
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

        /// <summary>
        /// Connect to the SMTP server, authenticate, and send the email.
        /// </summary>
        /// <param name="email">The MimeMessage to be sent via the SMTP client.</param>
        /// <param name="token">A CancellationToken to observe while sending the email, allowing for task cancellation.</param>
        /// <returns></returns>
        private async Task SendEmail(MimeMessage email, CancellationToken token)
        {
            using var smtp = new SmtpClient();

            IsValid(_mailingSettings.Mail);

            await smtp.ConnectAsync(_mailingSettings.Host, _mailingSettings.Port, SecureSocketOptions.StartTls, token);
            await smtp.AuthenticateAsync(_mailingSettings.Mail, _mailingSettings.Password, token);
            await smtp.SendAsync(email, token);
            await smtp.DisconnectAsync(true, token);
        }

        /// <summary>
        /// Validates the format of an email address using a regular expression.
        /// </summary>
        /// <param name="emailAddress">The email address to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the provided email address does not match the expected format.</exception>
        private void IsValid(string emailAddress)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (!emailRegex.IsMatch(emailAddress))
                throw new ArgumentException("Invalid email address format", nameof(emailAddress));
        }
    }
}