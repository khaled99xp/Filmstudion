using Filmstudion.API.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Filmstudion.API.DTO
{
    public class CreateFilmFormDTO : ICreateFilm
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public int ReleaseYear { get; set; }
        public int NumberOfCopies { get; set; }
        public IFormFile CoverImage { get; set; }
    }
}
