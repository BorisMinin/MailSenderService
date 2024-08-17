using MailSenderService.Models;

namespace MailSenderService.Services
{
    public interface IMailingService
    {
        Task SendEmailAsync(MailRequest mailRequest, CancellationToken token);
    }
}