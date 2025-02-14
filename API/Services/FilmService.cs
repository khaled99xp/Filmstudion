using Filmstudion.API.DTO;
using Filmstudion.API.Interfaces;
using Filmstudion.API.Models.Film;
using Filmstudion.API.Models.FilmCopy;
using Filmstudion.API.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Filmstudion.API.Services
{
     public class FilmService : IFilmService
     {
          private readonly IFilmRepository _filmRepository;
          public FilmService(IFilmRepository filmRepository)
          {
               _filmRepository = filmRepository;
          }

          public async Task<IEnumerable<Film>> GetAllFilmsAsync(bool includeCopies)
          {
               return await _filmRepository.GetAllFilmsAsync(includeCopies);
          }

          public async Task<Film> GetFilmByIdAsync(int id, bool includeCopies)
          {
               return await _filmRepository.GetFilmByIdAsync(id, includeCopies);
          }

          public async Task<Film> CreateFilmAsync(ICreateFilm createFilm)
          {
               var film = new Film
               {
                    Title = createFilm.Title,
                    Description = createFilm.Description,
                    Director = createFilm.Director,
                    ReleaseYear = createFilm.ReleaseYear,
                    CoverImagePath = null
               };


               if (createFilm is CreateFilmFormDTO formDto && formDto.CoverImage != null)
               {

                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                         Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + formDto.CoverImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                         await formDto.CoverImage.CopyToAsync(fileStream);
                    }

                    film.CoverImagePath = "/Uploads/" + uniqueFileName;
               }


               for (int i = 0; i < createFilm.NumberOfCopies; i++)
               {
                    film.FilmCopies.Add(new FilmCopy { IsAvailable = true });
               }
               return await _filmRepository.AddFilmAsync(film);
          }

          public async Task<Film> UpdateFilmAsync(int id, ICreateFilm updateFilm)
          {
               var film = await _filmRepository.GetFilmByIdAsync(id, includeCopies: true);
               if (film == null)
                    return null;
               film.Title = updateFilm.Title;
               film.Description = updateFilm.Description;
               film.Director = updateFilm.Director;
               film.ReleaseYear = updateFilm.ReleaseYear;

               return await _filmRepository.UpdateFilmAsync(film);
          }

          public async Task<bool> DeleteFilmAsync(int id)
          {
               var film = await _filmRepository.GetFilmByIdAsync(id, includeCopies: true);
               if (film == null)
                    return false;
               await _filmRepository.DeleteFilmAsync(film);
               return true;
          }
     }
}
