using MailSenderService.Models;
using MailSenderService.Services;
using MailSenderService.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace TestMailing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailingService _mailService;
        private readonly MailingSettings _mailingSettings;

        public MailController(IMailingService mailService, IOptions<MailingSettings> mailingSettings)
        {
            _mailService = mailService;
            _mailingSettings = mailingSettings.Value;
        }

        [HttpPost]  
        public async Task SendEmailAsync(MailMessage mailRequest, CancellationToken token)
        {
            await _mailService.SendEmailAsync(mailRequest, token);
        }
    }
}