
using Filmstudion.API.Interfaces;

using FilmCopyType = Filmstudion.API.Models.FilmCopy.FilmCopy;
using System.Collections.Generic;

namespace Filmstudion.API.Models.FilmStudio
{
    public class FilmStudio : IFilmStudio
    {
        public int FilmStudioId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public ICollection<FilmCopyType> RentedFilmCopies { get; set; } = new List<FilmCopyType>();

        public bool IsBlocked { get; set; }
    }
}
