
using Filmstudion.API.Interfaces;

namespace Filmstudion.API.Models.User
{
    public class User : IUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? FilmStudioId { get; set; }

        public Filmstudion.API.Models.FilmStudio.FilmStudio FilmStudio { get; set; }
    }
}
