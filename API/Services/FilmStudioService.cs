using Filmstudion.API.Models.FilmStudio;
using Filmstudion.API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filmstudion.API.Services
{
    public class FilmStudioService : IFilmStudioService
    {
         private readonly IFilmStudioRepository _filmStudioRepository;
         public FilmStudioService(IFilmStudioRepository filmStudioRepository)
         {
              _filmStudioRepository = filmStudioRepository;
         }

         public async Task<IEnumerable<FilmStudio>> GetAllFilmStudiosAsync(bool includeDetails)
         {
              return await _filmStudioRepository.GetAllAsync(includeDetails);
         }

         public async Task<FilmStudio> GetFilmStudioByIdAsync(int id, bool includeDetails)
         {
              return await _filmStudioRepository.GetByIdAsync(id, includeDetails);
         }

         public async Task<FilmStudio> BlockFilmStudioAsync(int id)
         {
              var studio = await _filmStudioRepository.GetByIdAsync(id, true);
              if (studio != null)
              {
                   studio.IsBlocked = true;
                   return await _filmStudioRepository.UpdateAsync(studio);
              }
              return null;
         }
    }
}
