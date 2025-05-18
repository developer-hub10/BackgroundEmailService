using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BackgroundEmailService.Models;
using BackgroundEmailService.Repository;


namespace BackgroundEmailService.Controllers
{

    [ApiController]
    [Route("api/applicant")]
    public class ApplicantController : ControllerBase
    {
        
        private readonly IApplicantRepository _applicantRepository;

        public ApplicantController(IApplicantRepository applicantRepository)
        {
            _applicantRepository = applicantRepository;
        }

        [HttpPost("/apply-job")]
        public async Task<IActionResult> ApplyJob([FromBody] Applicant data)
        {
           var result = await _applicantRepository.RegisterApplicantAsync(data);

           if(result == null) return NotFound("404 try again sometimes later....");

           return Ok("Succesfully Applied for this job");
        }

        [HttpGet("/get-all")]
        public async Task<IActionResult> GetAllApplicants() 
        {
            var result = await _applicantRepository.GetAllApplicant();

            if(result == null) return NotFound("No applicants found...");

            return Ok(result);
        }

    }

}