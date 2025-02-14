using Filmstudion.API.Repositories;
using Filmstudion.API.Models.FilmCopy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.API.Services
{
     public class RentalService : IRentalService
     {
          private readonly IRentalRepository _rentalRepository;
          public RentalService(IRentalRepository rentalRepository)
          {
               _rentalRepository = rentalRepository;
          }

          public async Task<bool> RentFilmAsync(int filmId, int studioId, DateTime rentalStart, DateTime rentalEnd)
          {
               return await _rentalRepository.RentFilmAsync(filmId, studioId, rentalStart, rentalEnd);
          }

          public async Task<bool> ReturnFilmAsync(int filmId, int studioId)
          {
               return await _rentalRepository.ReturnFilmAsync(filmId, studioId);
          }

          public async Task<IEnumerable<FilmCopy>> GetRentalsByStudioAsync(int studioId)
          {
               return await _rentalRepository.GetRentalsByStudioAsync(studioId);
          }
     }
}
