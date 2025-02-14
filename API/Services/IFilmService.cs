using Filmstudion.API.Models.Film;
using Filmstudion.API.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.API.Services
{
    public interface IFilmService
    {
        Task<IEnumerable<Film>> GetAllFilmsAsync(bool includeCopies);
        Task<Film> GetFilmByIdAsync(int id, bool includeCopies);
        Task<Film> CreateFilmAsync(ICreateFilm createFilm);
        Task<Film> UpdateFilmAsync(int id, ICreateFilm updateFilm);
        Task<bool> DeleteFilmAsync(int id);

    }
}
