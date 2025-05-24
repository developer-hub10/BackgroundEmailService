namespace BackgroundEmailService.Models
{


    public class Email
    {

        public int Id { get; set; }

        public string UserEmail { get; set; }

        public string EmailStatus { get; set; }

        public int Times { get; set; }

    }
}