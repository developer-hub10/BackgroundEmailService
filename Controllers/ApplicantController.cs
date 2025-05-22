using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BackgroundEmailService.Models;
using BackgroundEmailService.Repository;
using Microsoft.AspNetCore.Authorization;

using BackgroundEmailService.Dtos;
using BackgroundEmailService.IMappers;
using BackgroundEmailService.Services;

namespace BackgroundEmailService.Controllers
{

    [ApiController]
    [Route("api/applicant")]
    public class ApplicantController : ControllerBase
    {

        private readonly IApplicantRepository _applicantRepository;

        private readonly IApplicantMapper _applicantMapper;

        public ApplicantController(IApplicantRepository applicantRepository, IApplicantMapper applicantMapper)
        {
            _applicantRepository = applicantRepository;
            _applicantMapper = applicantMapper;
        }

        [AllowAnonymous]
        [HttpPost("/apply-job")]
        public async Task<IActionResult> ApplyJob([FromForm] ApplicantRequest request)
        {

            Applicant data = _applicantMapper.ToApplicant(request);
            var result = await _applicantRepository.RegisterApplicantAsync(data);

            if (result == null) return NotFound("404 try again sometimes later....");

            return Ok("Succesfully Applied for this job");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllApplicants()
        {
            List<Applicant> applicantList = await _applicantRepository.GetAllApplicant();
            List<ApplicantResponse> applicantResponseList = new List<ApplicantResponse>();

            foreach (Applicant applicant in applicantList)
            {
                ApplicantResponse response = _applicantMapper.ToResponse(applicant);
                applicantResponseList.Add(response);
            }

            if (applicantResponseList == null) return NotFound("No applicants found...");

            return Ok(applicantResponseList);
        }

        //   "imageUrl": "http://localhost:5000/api/applicant/resume/2",
        [Authorize(Roles = "admin")]
        [HttpGet("resume/{id}")]
        public async Task<IActionResult> GetApplicantResumeById([FromRoute] int id)
        {
            Applicant applicant = await _applicantRepository.GetApplicantById(id);

            if (applicant == null) return NotFound($"No Applicant exist with this {id}");
            if (applicant.ImageData == null) return NotFound("Resume not found for this id");
            return File(applicant.ImageData, applicant.ImageType, applicant.ImageName);
        }
        

    }

}