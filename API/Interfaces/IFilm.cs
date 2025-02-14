using Filmstudion.API.Models.FilmCopy;
using System.Collections.Generic;

namespace Filmstudion.API.Interfaces
{
    public interface IFilm
    {
         int FilmId { get; set; }
         string Title { get; set; }
         string Description { get; set; }
         string Director { get; set; }
         int ReleaseYear { get; set; }
         ICollection<FilmCopy> FilmCopies { get; set; }
    }
}
