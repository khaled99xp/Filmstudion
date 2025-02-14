using Filmstudion.API.Models.Film;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.API.Repositories
{
    public interface IFilmRepository
    {
        Task<IEnumerable<Film>> GetAllFilmsAsync(bool includeCopies = false);
        Task<Film> GetFilmByIdAsync(int id, bool includeCopies = false);
        Task<Film> AddFilmAsync(Film film);
        Task<Film> UpdateFilmAsync(Film film);
        // API/Repositories/IFilmRepository.cs
        Task DeleteFilmAsync(Film film);

    }
}
