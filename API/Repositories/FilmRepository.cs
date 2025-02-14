using Filmstudion.API.Data;
using Filmstudion.API.Models.Film;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.API.Repositories
{
     public class FilmRepository : IFilmRepository
     {
          private readonly ApplicationDbContext _context;
          public FilmRepository(ApplicationDbContext context)
          {
               _context = context;
          }

          public async Task<IEnumerable<Film>> GetAllFilmsAsync(bool includeCopies = false)
          {
               if (includeCopies)
               {
                    return await _context.Films.Include(f => f.FilmCopies).ToListAsync();
               }
               return await _context.Films.ToListAsync();
          }

          public async Task<Film> GetFilmByIdAsync(int id, bool includeCopies = false)
          {
               if (includeCopies)
               {
                    return await _context.Films.Include(f => f.FilmCopies)
                                               .FirstOrDefaultAsync(f => f.FilmId == id);
               }
               return await _context.Films.FirstOrDefaultAsync(f => f.FilmId == id);
          }

          public async Task<Film> AddFilmAsync(Film film)
          {
               _context.Films.Add(film);
               await _context.SaveChangesAsync();
               return film;
          }

          public async Task<Film> UpdateFilmAsync(Film film)
          {
               _context.Films.Update(film);
               await _context.SaveChangesAsync();
               return film;
          }
          
          public async Task DeleteFilmAsync(Film film)
          {
               _context.Films.Remove(film);
               await _context.SaveChangesAsync();
          }

        public Task<bool> DeleteFilmAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
