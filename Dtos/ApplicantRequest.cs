

namespace BackgroundEmailService.Dtos
{


    public class ApplicantRequest
    {

        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Position { get; set; }

        public int ExpInYears { get; set; }

        public IFormFile Image { get; set; }

    }
}