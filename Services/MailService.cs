using System.Runtime.CompilerServices;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BackgroundEmailService.Services
{


    public class MailService
    {

        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }



    }


}