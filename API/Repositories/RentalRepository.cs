using Filmstudion.API.Data;
using Filmstudion.API.Models.FilmCopy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.API.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly ApplicationDbContext _context;
        public RentalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RentFilmAsync(int filmId, int studioId, DateTime rentalStart, DateTime rentalEnd)
        {
            var film = await _context.Films.Include(f => f.FilmCopies)
                                           .FirstOrDefaultAsync(f => f.FilmId == filmId);
            if (film == null)
                return false;

            bool alreadyRented = film.FilmCopies.Any(fc => fc.FilmStudioId == studioId && !fc.IsAvailable);
            if (alreadyRented)
                return false;
            var availableCopy = film.FilmCopies.FirstOrDefault(fc => fc.IsAvailable);
            if (availableCopy == null)
                return false;
            availableCopy.IsAvailable = false;
            availableCopy.FilmStudioId = studioId;
            availableCopy.RentalStartDate = rentalStart;
            availableCopy.RentalEndDate = rentalEnd;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReturnFilmAsync(int filmId, int studioId)
        {
            var film = await _context.Films.Include(f => f.FilmCopies)
                                           .FirstOrDefaultAsync(f => f.FilmId == filmId);
            if (film == null)
                return false;
            var rentedCopy = film.FilmCopies.FirstOrDefault(fc => fc.FilmStudioId == studioId && !fc.IsAvailable);
            if (rentedCopy == null)
                return false;
            rentedCopy.IsAvailable = true;
            rentedCopy.FilmStudioId = null;
            rentedCopy.RentalStartDate = null;
            rentedCopy.RentalEndDate = null;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FilmCopy>> GetRentalsByStudioAsync(int studioId)
        {
            return await _context.FilmCopies
                      .Where(fc => fc.FilmStudioId == studioId && !fc.IsAvailable)
                      .Include(fc => fc.Film)
                      .ToListAsync();
        }
    }
}
