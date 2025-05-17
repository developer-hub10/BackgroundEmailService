using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BackgroundEmailService.Services
{
    public class EmailService : BackgroundService
    {
        private readonly ILogger<EmailService> _logger;

        private readonly IConfiguration _config;

        public EmailService(ILogger<EmailService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int counter = 1;
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Log" + counter++);


                if (counter == 10)
                {
                    _logger.LogInformation("Applicaton Stopping");
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }

            _logger.LogInformation("Background service stopped.");
        }
    }
}
