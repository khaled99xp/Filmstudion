using Filmstudion.API.Models.FilmStudio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.API.Services
{
    public interface IFilmStudioService
    {
         Task<IEnumerable<FilmStudio>> GetAllFilmStudiosAsync(bool includeDetails);
         Task<FilmStudio> GetFilmStudioByIdAsync(int id, bool includeDetails);
         Task<FilmStudio> BlockFilmStudioAsync(int id);
    }
}
