using Filmstudion.API.Models.Film;
using Filmstudion.API.Models.FilmStudio;
using Filmstudion.API.Models.User;
using Filmstudion.API.Models.FilmCopy;
using Microsoft.EntityFrameworkCore;

namespace Filmstudion.API.Data
{
    public class ApplicationDbContext : DbContext
    {
         public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

         public DbSet<Film> Films { get; set; }
         public DbSet<FilmStudio> FilmStudios { get; set; }
         public DbSet<User> Users { get; set; }
         public DbSet<FilmCopy> FilmCopies { get; set; }
    }
}
