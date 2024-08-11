using MailSenderService.Models;
using MailSenderService.Services;
using MailSenderService.Settings;

namespace MailSenderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<MailSettings>(
                builder.Configuration.GetSection("MailSettings"));

            builder.Services.AddScoped<IMailService, MailService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // send email.
            app.MapPost("/sendMail", async (MailMessage mailRequest, CancellationToken token, IMailService mailService) =>
            {
                await mailService.SendEmailAsync(mailRequest, token);
            })
            .WithName("SendMail")
            .WithOpenApi();

            app.Run();
        }
    }
}