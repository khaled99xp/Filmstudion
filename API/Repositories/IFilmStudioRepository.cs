using Filmstudion.API.Models.FilmStudio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.API.Repositories
{
    public interface IFilmStudioRepository
    {
         Task<FilmStudio> GetByIdAsync(int id, bool includeRented = false);
         Task<IEnumerable<FilmStudio>> GetAllAsync(bool includeRented = false);
         Task<FilmStudio> AddAsync(FilmStudio filmStudio);
         Task<FilmStudio> UpdateAsync(FilmStudio filmStudio);
    }
}
