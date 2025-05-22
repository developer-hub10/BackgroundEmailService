
using BackgroundEmailService.Models;
using BackgroundEmailService.Dtos;
using BackgroundEmailService.IMappers;
using System.Net.Http.Headers;

namespace BackgroundEmailService.Mappers
{

    public class ApplicantMapper : IApplicantMapper
    {


        private readonly IConfiguration _config;

        public ApplicantMapper(IConfiguration config)
        {
            _config = config;
        }

        public Applicant ToApplicant(ApplicantRequest request)
        {

            byte[] data = null;
            if (request.Image != null)
            {
                using var memoryStream = new MemoryStream();
                request.Image.CopyTo(memoryStream);
                data = memoryStream.ToArray();
            }


            return new Applicant
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Position = request.Position,
                ExpInYears = request.ExpInYears,
                ImageData = data,
                ImageType = request.Image.ContentType,
                ImageName = request.Image.FileName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        public ApplicantResponse ToResponse(Applicant applicant)
        {

            string baseUrl = _config.GetValue<string>("AppSettings:BaseUrl");
            string imageUrl = $"{baseUrl}/api/applicant/resume/{applicant.Id}";

            return new ApplicantResponse
            {
                Id = applicant.Id,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Email = applicant.Email,
                Position = applicant.Position,
                ExpInYears = applicant.ExpInYears,
                ImageUrl = imageUrl,
                CreatedAt = applicant.CreatedAt,
                UpdatedAt = applicant.UpdatedAt
            };
        }
    }


}