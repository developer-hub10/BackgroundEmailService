using BackgroundEmailService.Models;
using BackgroundEmailService.Dtos;


namespace BackgroundEmailService.IMappers
{


    public interface IApplicantMapper
    {

        public Applicant ToApplicant(ApplicantRequest request);

        public ApplicantResponse ToResponse(Applicant applicant);
    }


}