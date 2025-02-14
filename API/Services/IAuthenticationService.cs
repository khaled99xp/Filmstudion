using Filmstudion.API.Models.User;
using Filmstudion.API.Interfaces;
using System.Threading.Tasks;

namespace Filmstudion.API.Services
{
    public interface IAuthenticationService
    {
        Task<(User user, string token)> AuthenticateAsync(IUserAuthenticate authModel);
        Task<User> RegisterUserAsync(IUserRegister registerModel);
        Task<(Filmstudion.API.Models.FilmStudio.FilmStudio filmStudio, User user, string token)> RegisterFilmStudioAsync(IRegisterFilmStudio registerModel);
        void Logout(string token);
    }
}
