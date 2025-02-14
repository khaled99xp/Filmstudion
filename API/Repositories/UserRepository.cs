using Filmstudion.API.Data;
using Filmstudion.API.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Filmstudion.API.Repositories
{
    public class UserRepository : IUserRepository
    {
         private readonly ApplicationDbContext _context;
         public UserRepository(ApplicationDbContext context)
         {
              _context = context;
         }

         public async Task<User> GetByUsernameAsync(string username)
         {
              return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
         }

         public async Task<User> AddAsync(User user)
         {
              _context.Users.Add(user);
              await _context.SaveChangesAsync();
              return user;
         }
    }
}
