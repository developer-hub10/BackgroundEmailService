using Microsoft.AspNetCore.Mvc;

using BackgroundEmailService.Repository;
using BackgroundEmailService.Models;
using BackgroundEmailService.Dtos;
using Microsoft.AspNetCore.Authorization;

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
      
      [AllowAnonymous]
      [HttpPost("register")]
      public async Task<IActionResult> CreateAuth([FromBody] Auth data)
      {
          await _authRepo.Register(data);
         
         return Ok("Sucessfully Registered");
      }
      
      [AllowAnonymous]
      [HttpPost("login")]
      public async Task<IActionResult> LoginAuth([FromBody] AuthRequest request)
      {
         var result = await _authRepo.Login(request.Username, request.Password);
         
         if(result == null) return NotFound("Try again sometimes later...");

         return Ok(result);
      }

    }
}