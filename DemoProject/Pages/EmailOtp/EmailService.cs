using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace DemoProject.Pages.EmailOtp
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your Name", _configuration["EmailSettings:From"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), false);
                    await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                    await client.SendAsync(emailMessage);
                }
                catch (SmtpCommandException ex)
                {
                    // Log email details and the exception
                    _logger.LogError(ex, $"SMTP Command Error: {ex.Message} - Email: {email}, Subject: {subject}, Message: {message}");
                    throw new Exception($"SMTP Command Error: {ex.Message}");
                }
                catch (SmtpProtocolException ex)
                {
                    // Log email details and the exception
                    _logger.LogError(ex, $"SMTP Protocol Error: {ex.Message} - Email: {email}, Subject: {subject}, Message: {message}");
                    throw new Exception($"SMTP Protocol Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Log email details and the exception
                    _logger.LogError(ex, $"Unexpected Error: {ex.Message} - Email: {email}, Subject: {subject}, Message: {message}");
                    throw new Exception($"Unexpected Error: {ex.Message}");
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
