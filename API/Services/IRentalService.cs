using Filmstudion.API.Models.FilmCopy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.API.Services
{
    public interface IRentalService
    {
        Task<bool> RentFilmAsync(int filmId, int studioId, DateTime rentalStart, DateTime rentalEnd);
        Task<bool> ReturnFilmAsync(int filmId, int studioId);
        Task<IEnumerable<FilmCopy>> GetRentalsByStudioAsync(int studioId);
    }
}
