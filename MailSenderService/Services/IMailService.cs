using MailSenderService.Models;

namespace MailSenderService.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest, CancellationToken token);
    }
}