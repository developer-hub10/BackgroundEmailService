using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundEmailService.Services
{
    public class EmailService : IHostedLifecycleService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }
        
        // Load Db config and establish db connection
        public Task StartingAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting EmailService...");
            return Task.CompletedTask;
        }
         

         // fetch the emails 
        public Task StartedAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started EmailService...");
            return Task.CompletedTask;
        }
        

        // Do the email service here
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start EmailService...");
            return Task.CompletedTask;
        }

        public Task StoppingAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping EmailService...");
            return Task.CompletedTask;
        }

        public Task StoppedAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopped EmailService...");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stop Email Service...");
            return Task.CompletedTask;
        }
    }
}
