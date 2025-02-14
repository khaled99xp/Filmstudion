using Filmstudion.API.DTO; 
using Filmstudion.API.Interfaces;
using Filmstudion.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
         private readonly IAuthenticationService _authService;
         public UsersController(IAuthenticationService authService)
         {
              _authService = authService;
         }

         [HttpPost("register")]
         public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerModel)
         {
              try
              {
                   var user = await _authService.RegisterUserAsync(registerModel);
                   if (user == null)
                      return Conflict("User already exists.");
                   return Ok(new { user.UserId, user.Username, user.Role });
              }
              catch (System.Exception ex)
              {
                   return BadRequest(ex.Message);
              }
         }

         [HttpPost("authenticate")]
         public async Task<IActionResult> Authenticate([FromBody] UserAuthenticateDTO authModel)
         {
              var (user, token) = await _authService.AuthenticateAsync(authModel);
              if (user == null)
                 return Unauthorized("Invalid credentials.");
              var result = new
              {
                   user.UserId,
                   user.Username,
                   user.Role,
                   FilmStudioId = user.FilmStudioId,
                   Token = token
              };
              return Ok(result);
         }

         [HttpPost("logout")]
         public IActionResult Logout()
         {
              var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
              if (string.IsNullOrEmpty(token))
                  return BadRequest("No token provided.");
              _authService.Logout(token);
              return Ok("Logged out successfully.");
         }

         
    }
}
