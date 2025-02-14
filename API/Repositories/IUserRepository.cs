using Filmstudion.API.Models.User;
using System.Threading.Tasks;

namespace Filmstudion.API.Repositories
{
    public interface IUserRepository
    {
         Task<User> GetByUsernameAsync(string username);
         Task<User> AddAsync(User user);
    }
}
