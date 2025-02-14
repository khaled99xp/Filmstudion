using System;
using System.Linq;
using System.Threading.Tasks;
using Filmstudion.API.DTO;
using Filmstudion.API.Interfaces;
using Filmstudion.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filmstudion.API.Controllers
{
    [ApiController]
    [Route("api/films")]
    public class FilmsController : ControllerBase
    {
        private readonly IFilmService _filmService;
        public FilmsController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFilms()
        {
            var films = await _filmService.GetAllFilmsAsync(includeCopies: true);
            var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value?.ToLower();
            if (string.IsNullOrEmpty(role) || (role != "admin" && role != "filmstudio"))
            {
                films = films.Select(f => { f.FilmCopies = null; return f; });
            }
            return Ok(films);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilm(int id)
        {
            var film = await _filmService.GetFilmByIdAsync(id, includeCopies: true);
            if (film == null)
                return NotFound();
            var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value?.ToLower();
            if (string.IsNullOrEmpty(role) || (role != "admin" && role != "filmstudio"))
            {
                film.FilmCopies = null;
            }
            return Ok(film);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFilm([FromForm] CreateFilmFormDTO createFilm)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value?.ToLower();
            if (role != "admin")
                return Unauthorized();
            var film = await _filmService.CreateFilmAsync(createFilm);
            return Ok(film);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateFilm(int id, [FromBody] CreateFilmDTO updateFilm)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            if (role != "admin")
                return Unauthorized();
            var film = await _filmService.UpdateFilmAsync(id, updateFilm);
            if (film == null)
                return NotFound();
            return Ok(film);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            if (role != "admin")
                return Unauthorized();
            var success = await _filmService.DeleteFilmAsync(id);
            if (!success)
                return NotFound("Film not found.");
            return Ok("Film deleted successfully.");
        }

        [HttpPost("rent")]
        public async Task<IActionResult> RentFilm([FromQuery] int id, [FromQuery] int studioid, [FromQuery] DateTime rentalStart, [FromQuery] DateTime rentalEnd)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            if (role != "filmstudio")
                return Unauthorized();
            var userStudioId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "FilmStudioId")?.Value ?? "0");
            if (userStudioId != studioid)
                return Unauthorized();
            var rentalService = HttpContext.RequestServices.GetService(typeof(IRentalService)) as IRentalService;
            var success = await rentalService.RentFilmAsync(id, studioid, rentalStart, rentalEnd);
            if (!success)
                return Conflict("Cannot rent film.");
            return Ok("Film rented successfully.");
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnFilm([FromQuery] int id, [FromQuery] int studioid)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            if (role != "filmstudio")
                return Unauthorized();
            var userStudioId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "FilmStudioId")?.Value ?? "0");
            if (userStudioId != studioid)
                return Unauthorized();
            var rentalService = HttpContext.RequestServices.GetService(typeof(IRentalService)) as IRentalService;
            var success = await rentalService.ReturnFilmAsync(id, studioid);
            if (!success)
                return Conflict("Cannot return film.");
            return Ok("Film returned successfully.");
        }
    }
}
