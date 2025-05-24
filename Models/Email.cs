namespace BackgroundEmailService.Models
{


    public enum MyEmailStatus
    {

        Success,
         
        Failed
    }
    
    public class Email
    {

        public int Id { get; set; }

        public string UserEmail { get; set; }

        public string EmailStatus { get; set; }

        public int Times { get; set; }

    }
}