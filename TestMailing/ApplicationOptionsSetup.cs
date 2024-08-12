using MailSenderService.Settings;
using Microsoft.Extensions.Options;

namespace TestMailing
{
    public class ApplicationOptionsSetup : IConfigureOptions<MailingSettings>
    {
        private readonly IConfiguration _configuration;

        public ApplicationOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(MailingSettings options)
        {
            _configuration.GetSection(nameof(MailingSettings))
                .Bind(options);
        }
    }
}
