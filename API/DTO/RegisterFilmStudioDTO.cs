using Filmstudion.API.Interfaces;
using Filmstudion.API.Models.Film;
using System.Collections.Generic;

namespace Filmstudion.API.DTO
{
    public class RegisterFilmStudioDTO : IRegisterFilmStudio
    {
         public string Name { get; set; }
         public string City { get; set; }
         public string Username { get; set; }
         public string Password { get; set; }
    }
}
