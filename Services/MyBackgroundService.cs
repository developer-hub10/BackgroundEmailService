using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BackgroundEmailService.Repository;
using BackgroundEmailService.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundEmailService.Services
{
    public class MyBackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        private readonly ILogger<MyBackgroundService> _logger;

        private readonly string Host;
        private readonly int Port;
        private readonly string Username;
        private readonly string Password;
        private readonly bool EnableSsl;
        private readonly string From;

        public MyBackgroundService(IServiceProvider serviceProvider, IConfiguration config, ILogger<MyBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _config = config;
            _logger = logger;

            Host = _config.GetValue<string>("SmtpSettings:Host");
            Port = _config.GetValue<int>("SmtpSettings:Port");
            Username = _config.GetValue<string>("SmtpSettings:Username");
            Password = _config.GetValue<string>("SmtpSettings:Password");
            EnableSsl = _config.GetValue<bool>("SmtpSettings:EnableSsl");
            From = _config.GetValue<string>("SmtpSettings:From");
        }

         public  async Task ExecuteAsync()
        {
            

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var emailRepo = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
                        List<Email> pendingEmailList = await emailRepo.GetPendingEmails();

                        _logger.LogInformation("Found {Count} pending emails.", pendingEmailList.Count);
                        

                        string body = "<h1>Hello this is grow more</h1>";

                        foreach (Email email in pendingEmailList)
                        {
                            try
                            {   
                                 _logger.LogInformation("Email sending to {Email}.", email.UserEmail);
                                await SendEmail(email.UserEmail, "Testing the email service", body);
                                
                                await emailRepo.UpdateEmailStatus(new Email
                                {
                                    Id = email.Id,
                                    EmailStatus = "Sent",
                                    Times = email.Times
                                });

                                _logger.LogInformation("Email sent to {Email}.", email.UserEmail);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Failed to send email to {Email}.", email.UserEmail);

                                await emailRepo.UpdateEmailStatus(new Email
                                {
                                    Id = email.Id,
                                    EmailStatus = "Pending",
                                    Times = email.Times
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error while processing emails.");
                }

               
            }

            
        

        private async Task SendEmail(string to, string subject, string body)
        {
            using (var client = new SmtpClient(Host, Port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Username, Password);
                client.EnableSsl = EnableSsl;

                var mail = new MailMessage(From, to, subject, body)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(mail);
            }
        }
    }
}
