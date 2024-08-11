using MailSenderService.Models;
using MailSenderService.Services;
using Microsoft.AspNetCore.Mvc;

namespace TestMailing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public async Task SendEmailAsync(MailMessage mailRequest, CancellationToken token)
        {
            await _mailService.SendEmailAsync(mailRequest, token);
        }
    }
}