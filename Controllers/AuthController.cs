using Microsoft.AspNetCore.Mvc;

using BackgroundEmailService.Repository;
using BackgroundEmailService.Models;
using BackgroundEmailService.Dtos;

namespace BackgroundEmailService.Controllers 
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
      

      private readonly IAuthRepository _authRepo;

      public AuthController(IAuthRepository authRepo) 
      {
        _authRepo = authRepo;
      }

      [HttpPost("register")]
      public async Task<IActionResult> CreateUser([FromBody] Auth data)
      {
          await _authRepo.Register(data);
         
         return Ok("Sucessfully Registered");
      }

      [HttpPost("login")]
      public async Task<IActionResult> CreateUser([FromBody] AuthRequest request)
      {
         var result = await _authRepo.Login(request.Username, request.Password);
         
         if(result == null) return NotFound("Try again sometimes later...");

         return Ok(result);
      }





    }
}