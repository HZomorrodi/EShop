using EShop.Services.Contracts;
using EShop.ViewModels.Application;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;


namespace EShop.Services
{
    public class EmailSenderService(
        IOptionsSnapshot<EmailConfigsViewModel> emailConfig,
        IWebHostEnvironment env) : IEmailSenderService  
    {
        private readonly IOptionsSnapshot<EmailConfigsViewModel> _emailConfig = emailConfig;
        private readonly IWebHostEnvironment _env = env;

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_emailConfig.Value.SiteTitle, _emailConfig.Value.SiteAddress));
            mimeMessage.To.Add(new MailboxAddress("", to));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };
            if (!_env.IsDevelopment())
            {
                await using var stream = new FileStream($@"C:\Users\268\Desktop\New folder\Email\Email-{Guid.NewGuid():N}.eml", FileMode.CreateNew);
                await mimeMessage.WriteToAsync(stream);
            }
            else
            {
                using var client = new SmtpClient();
                //client.LocalDomain = "";
                await client.ConnectAsync(_emailConfig.Value.Host, _emailConfig.Value.Port, MailKit.Security.SecureSocketOptions.StartTls).ConfigureAwait(false);
                //await client.ConnectAsync(_emailConfig.Value.Host, _emailConfig.Value.Port, _emailConfig.Value.UseSSL).ConfigureAwait(false);
                await client.AuthenticateAsync(_emailConfig.Value.UserName, _emailConfig.Value.Password).ConfigureAwait(false);
                await client.SendAsync(mimeMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
