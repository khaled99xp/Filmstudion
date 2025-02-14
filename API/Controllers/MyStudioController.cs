using Filmstudion.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.API.Controllers
{
    [ApiController]
    [Route("api/mystudio")]
    public class MyStudioController : ControllerBase
    {
         private readonly IRentalService _rentalService;
         public MyStudioController(IRentalService rentalService)
         {
              _rentalService = rentalService;
         }

         [HttpGet("rentals")]
         public async Task<IActionResult> GetRentals()
         {
              var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
              if(role != "filmstudio")
                  return Unauthorized();
              var studioId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "FilmStudioId")?.Value ?? "0");
              var rentals = await _rentalService.GetRentalsByStudioAsync(studioId);
              return Ok(rentals);
         }
    }
}
