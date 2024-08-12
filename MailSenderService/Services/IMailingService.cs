using MailSenderService.Models;

namespace MailSenderService.Services
{
    public interface IMailingService
    {
        Task SendEmailAsync(MailMessage mailRequest, CancellationToken token);
    }
}