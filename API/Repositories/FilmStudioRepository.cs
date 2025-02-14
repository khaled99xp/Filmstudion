using Filmstudion.API.Data;
using Filmstudion.API.Models.FilmStudio;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.API.Repositories
{
     public class FilmStudioRepository : IFilmStudioRepository
     {
          private readonly ApplicationDbContext _context;
          public FilmStudioRepository(ApplicationDbContext context)
          {
               _context = context;
          }

          public async Task<FilmStudio> GetByIdAsync(int id, bool includeRented = false)
          {
               if (includeRented)
               {
                    return await _context.FilmStudios.Include(fs => fs.RentedFilmCopies)
                                                      .FirstOrDefaultAsync(fs => fs.FilmStudioId == id);
               }
               return await _context.FilmStudios.FirstOrDefaultAsync(fs => fs.FilmStudioId == id);
          }

          public async Task<IEnumerable<FilmStudio>> GetAllAsync(bool includeRented = false)
          {
               if (includeRented)
               {
                    return await _context.FilmStudios.Include(fs => fs.RentedFilmCopies).ToListAsync();
               }
               return await _context.FilmStudios.ToListAsync();
          }

          public async Task<FilmStudio> AddAsync(FilmStudio filmStudio)
          {
               _context.FilmStudios.Add(filmStudio);
               await _context.SaveChangesAsync();
               return filmStudio;
          }

          public async Task<FilmStudio> UpdateAsync(FilmStudio filmStudio)
          {
               _context.FilmStudios.Update(filmStudio);
               await _context.SaveChangesAsync();
               return filmStudio;
          }
     }
}
