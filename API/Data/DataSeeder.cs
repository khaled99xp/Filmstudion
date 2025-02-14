using Filmstudion.API.Helpers;
using Filmstudion.API.Models.Film;
using Filmstudion.API.Models.FilmCopy;
using Filmstudion.API.Models.FilmStudio;
using Filmstudion.API.Models.User;

namespace Filmstudion.API.Data
{
    public class DataSeeder
    {
         private readonly ApplicationDbContext _context;

         public DataSeeder(ApplicationDbContext context)
         {
             _context = context;
         }

         public void Seed()
         {
             if(!_context.Users.Any())
             {
                 var adminUser = new User
                 {
                     Username = "admin",
                     Password = BCryptHelper.HashPassword("Admin@123"),
                     Role = "admin"
                 };
                 _context.Users.Add(adminUser);
             }

             if(!_context.FilmStudios.Any())
             {
                 var demoStudio = new FilmStudio
                 {
                     Name = "Demo Studio",
                     City = "Stockholm"
                 };
                 _context.FilmStudios.Add(demoStudio);
                 
                 _context.SaveChanges();

                 var filmStudioUser = new User
                 {
                     Username = "demostudio",
                     Password = BCryptHelper.HashPassword("Studio@123"),
                     Role = "filmstudio",
                     FilmStudioId = demoStudio.FilmStudioId
                 };
                 _context.Users.Add(filmStudioUser);
             }

             if(!_context.Films.Any())
             {
                 string defaultCoverPath = "/Uploads/default.jpg";

                 var film1 = new Film
                 {
                     Title = "Inception",
                     Description = "A mind-bending thriller",
                     Director = "Christopher Nolan",
                     ReleaseYear = 2010,
                     CoverImagePath = defaultCoverPath,
                     FilmCopies = new List<FilmCopy>()
                 };
                 for (int i = 0; i < 5; i++)
                 {
                     film1.FilmCopies.Add(new FilmCopy { IsAvailable = true });
                 }

                 var film2 = new Film
                 {
                     Title = "The Matrix",
                     Description = "A hacker discovers the nature of reality",
                     Director = "Lana Wachowski, Lilly Wachowski",
                     ReleaseYear = 1999,
                     CoverImagePath = defaultCoverPath,
                     FilmCopies = new List<FilmCopy>()
                 };
                 for (int i = 0; i < 5; i++)
                 {
                     film2.FilmCopies.Add(new FilmCopy { IsAvailable = true });
                 }

                 var film3 = new Film
                 {
                     Title = "Interstellar",
                     Description = "A journey through space and time",
                     Director = "Christopher Nolan",
                     ReleaseYear = 2014,
                     CoverImagePath = defaultCoverPath,
                     FilmCopies = new List<FilmCopy>()
                 };
                 for (int i = 0; i < 5; i++)
                 {
                     film3.FilmCopies.Add(new FilmCopy { IsAvailable = true });
                 }

                 _context.Films.AddRange(film1, film2, film3);
             }

             _context.SaveChanges();
         }
    }
}
