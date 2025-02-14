using Filmstudion.API.Interfaces;

namespace Filmstudion.API.DTO
{
    public class UserAuthenticateDTO : IUserAuthenticate
    {
         public string Username { get; set; }
         public string Password { get; set; }
    }
}
