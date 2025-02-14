using Filmstudion.API.Models.FilmCopy;
using System.Collections.Generic;

namespace Filmstudion.API.Interfaces
{
    public interface IFilmStudio
    {
         int FilmStudioId { get; set; }
         string Name { get; set; }
         string City { get; set; }
         ICollection<FilmCopy> RentedFilmCopies { get; set; }
    }
}
