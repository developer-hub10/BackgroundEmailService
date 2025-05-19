using BackgroundEmailService.Models;

namespace BackgroundEmailService.Repository
{


    public interface IEmailRepository
    {

        public Task<List<Email>> GetAllEmails();

         public Task<List<Email>> GetPendingEmails();

        public Task<int> RegisterEmails(List<Email> emailList);

        // public Task RegisterEmail(Email email);

        // public Task UpdateEmail(Email email);

        // public Task DeleteEmail(int id);
    }
}