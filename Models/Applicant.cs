
namespace BackgroundEmailService.Models
{

    public class Applicant {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Position { get; set; }

        public int ExpInYears { get; set; }

        public DateTime CreatedAt {get; set; }
    }
}