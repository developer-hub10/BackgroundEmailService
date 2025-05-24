using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BackgroundEmailService.Repository;
using BackgroundEmailService.Models;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Net;

namespace BackgroundEmailService.Services
{
    public class MyBackgroundService : BackgroundService
    {

        private IEmailRepository _emailRepo;

        private IConfiguration _config;

        private readonly string Host;

        private readonly int Port;

        private readonly string Username;

        private readonly string Password;

        private readonly bool EnableSsl;

        private readonly string From;



        public MyBackgroundService(IEmailRepository emailRepo, IConfiguration config)
        {
            _emailRepo = emailRepo;
            _config = config;
            Host = _config.GetValue<string>("SmtpSettings:Host");
            Port = _config.GetValue<int>("SmtpSettings:Port");
            Username = _config.GetValue<string>("SmtpSettings:Username");
            Password = _config.GetValue<string>("SmtpSettings:Password");
            EnableSsl = _config.GetValue<bool>("SmtpSettings:EnableSsl");
            From = _config.GetValue<string>("SmtpSettings:From");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                List<Email> pendingEmailList = await GetAllPendingEmails();
                string body = "<h1>Hello this is grow more</h1>";
                foreach (Email email in pendingEmailList)
                {
                    try
                    {
                        await SendEmail(email.UserEmail, "Testing the email service", body);
                        await _emailRepo.UpdateEmailStatus(new Email
                        {
                            Id = email.Id,
                            EmailStatus = "Sent",
                            Times = email.Times
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Mail Exception: " + ex.Message);
                        await _emailRepo.UpdateEmailStatus(new Email
                        {
                            Id = email.Id,
                            EmailStatus = "Pending",
                            Times = email.Times
                        });
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }

        }

        private async Task<List<Email>> GetAllPendingEmails()
        {
            List<Email> pendingEmails = await _emailRepo.GetPendingEmails();
            return pendingEmails;
        }

        private async Task SendEmail(string to, string subject, string body)
        {

            using (var client = new SmtpClient(Host, Port))
            {

                client.Credentials = new NetworkCredential(Username, Password);
                client.EnableSsl = EnableSsl;

                var mail = new MailMessage(From, to, subject, body);
                mail.IsBodyHtml = true;

                await client.SendMailAsync(mail);
            }
        }
    }
}
