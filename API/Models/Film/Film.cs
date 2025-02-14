using Filmstudion.API.Interfaces;
using FilmCopyType = Filmstudion.API.Models.FilmCopy.FilmCopy;
using System.Collections.Generic;

namespace Filmstudion.API.Models.Film
{

    public class Film : IFilm
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public int ReleaseYear { get; set; }
        public ICollection<FilmCopyType> FilmCopies { get; set; } = new List<FilmCopyType>();
        public string CoverImagePath { get; set; }
    }

}
