
namespace BackgroundEmailService.Models
{

    public class Applicant
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Position { get; set; }

        public int ExpInYears { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageType { get; set; }

        public string ImageName { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}