using Filmstudion.API.Interfaces;

namespace Filmstudion.API.DTO
{

    public class CreateFilmDTO : ICreateFilm
    {
         public string Title { get; set; }
         public string Description { get; set; }
         public string Director { get; set; }
         public int ReleaseYear { get; set; }
         public int NumberOfCopies { get; set; }
    }
}
