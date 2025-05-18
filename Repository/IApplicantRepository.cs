using System.Threading.Tasks;
using BackgroundEmailService.Models;

namespace BackgroundEmailService.Repository 
{


    public interface IApplicantRepository 
    {

         public Task<int> RegisterApplicantAsync(Applicant data);
        
         public Task<List<Applicant>> GetAllApplicant();
    }


}