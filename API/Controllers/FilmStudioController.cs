using Filmstudion.API.DTO;
using Filmstudion.API.Interfaces;
using Filmstudion.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.API.Controllers
{
    [ApiController]
    [Route("api/filmstudio")]
    public class FilmStudioController : ControllerBase
    {
         private readonly IAuthenticationService _authService;
         private readonly IFilmStudioService _filmStudioService;
         public FilmStudioController(IAuthenticationService authService, IFilmStudioService filmStudioService)
         {
              _authService = authService;
              _filmStudioService = filmStudioService;
         }

         [HttpPost("register")]
         public async Task<IActionResult> Register([FromBody] RegisterFilmStudioDTO registerModel)
         {
              try
              {
                   var (filmStudio, user, token) = await _authService.RegisterFilmStudioAsync(registerModel);
                   return Ok(new { filmStudio.FilmStudioId, filmStudio.Name, filmStudio.City });
              }
              catch (System.Exception ex)
              {
                   return BadRequest(ex.Message);
              }
         }

         [HttpGet("{id}")]
         public async Task<IActionResult> GetFilmStudio(int id)
         {
              bool includeDetails = false;
              if(User.Identity.IsAuthenticated)
              {
                   var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
                   if(role == "admin")
                   {
                        includeDetails = true;
                   }
                   else if(role == "filmstudio")
                   {
                        var userStudioId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "FilmStudioId")?.Value ?? "0");
                        if(userStudioId == id)
                             includeDetails = true;
                   }
              }
              var studio = await _filmStudioService.GetFilmStudioByIdAsync(id, includeDetails);
              if(studio == null)
                  return NotFound();
              if(!includeDetails)
              {
                   studio.City = null;
                   studio.RentedFilmCopies = null;
              }
              return Ok(studio);
         }

         [HttpGet]
         public async Task<IActionResult> GetAllFilmStudios()
         {
              bool includeDetails = false;
              if(User.Identity.IsAuthenticated)
              {
                   var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
                   if(role == "admin")
                        includeDetails = true;
              }
              var studios = await _filmStudioService.GetAllFilmStudiosAsync(includeDetails);
              if(!includeDetails)
              {
                   studios = studios.Select(s => { s.City = null; s.RentedFilmCopies = null; return s; });
              }
              return Ok(studios);
         }
    }
}
