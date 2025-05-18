using BackgroundEmailService.Models;
using System.Threading.Tasks;

namespace BackgroundEmailService.Repository
{


    public interface IAuthRepository
    {

       public Task Register(Auth data);
       public Task<string> Login(string username, string password);
 
    }
}