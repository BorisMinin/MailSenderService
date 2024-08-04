using MailSenderService.Models;

namespace MailSenderService.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailMessage mailRequest, CancellationToken token);
    }
}