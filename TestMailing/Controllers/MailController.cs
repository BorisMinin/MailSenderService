using MailSenderService.Models;
using MailSenderService.Services;
using Microsoft.AspNetCore.Mvc;

namespace TestMailing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailingService _mailService;

        public MailController(IMailingService mailService) => _mailService = mailService;

        [HttpPost]  
        public async Task SendEmailAsync(MailRequest mailRequest, CancellationToken token)
        {
            await _mailService.SendEmailAsync(mailRequest, token);
        }
    }
}