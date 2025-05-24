using BackgroundEmailService.Models;
using BackgroundEmailService.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundEmailService.Controllers
{

    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {

        private readonly IEmailRepository _emailRepo;
        
        public EmailController(IEmailRepository emailRepo)
        {
            _emailRepo = emailRepo;
        }

        [HttpPost("insert-bulk")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> InsetBulkEmails([FromBody] List<Email> emailList)
        {
            var result = await _emailRepo.RegisterEmails(emailList);
            return Ok($"{result} rows has been affected");
        }

    }

}