using Filmstudion.API.Interfaces;

namespace Filmstudion.API.DTO
{
    public class UserRegisterDTO : IUserRegister
    {
         public string Username { get; set; }
         public string Password { get; set; }
         public bool IsAdmin { get; set; }
    }
}
