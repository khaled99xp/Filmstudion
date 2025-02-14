using Filmstudion.API.Helpers;
using Filmstudion.API.Interfaces;
using Filmstudion.API.Models.FilmStudio;
using Filmstudion.API.Models.User;
using Filmstudion.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Filmstudion.API.DTO;

namespace Filmstudion.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
         private readonly IUserRepository _userRepository;
         private readonly IFilmStudioRepository _filmStudioRepository;
         private readonly IConfiguration _configuration;
        
         private static HashSet<string> tokenBlacklist = new HashSet<string>();

         public AuthenticationService(IUserRepository userRepository, IFilmStudioRepository filmStudioRepository, IConfiguration configuration)
         {
              _userRepository = userRepository;
              _filmStudioRepository = filmStudioRepository;
              _configuration = configuration;
         }

         public async Task<(User user, string token)> AuthenticateAsync(IUserAuthenticate authModel)
         {
              var user = await _userRepository.GetByUsernameAsync(authModel.Username);
              if (user == null || !BCryptHelper.VerifyPassword(authModel.Password, user.Password))
                 return (null, null);
              var token = JwtHelper.GenerateJwtToken(user, _configuration);
              return (user, token);
         }

         public async Task<User> RegisterUserAsync(IUserRegister registerModel)
         {
              var existingUser = await _userRepository.GetByUsernameAsync(registerModel.Username);
              if (existingUser != null)
                 return null;
              if (!PasswordPolicy.Validate(registerModel.Password))
                 throw new Exception("Password does not meet policy requirements.");
              var newUser = new User
              {
                  Username = registerModel.Username,
                  Password = BCryptHelper.HashPassword(registerModel.Password),
                  Role = registerModel.IsAdmin ? "admin" : "user"
              };
              return await _userRepository.AddAsync(newUser);
         }

         public async Task<(FilmStudio filmStudio, User user, string token)> RegisterFilmStudioAsync(IRegisterFilmStudio registerModel)
         {
              var newFilmStudio = new FilmStudio
              {
                   Name = registerModel.Name,
                   City = registerModel.City,

              };
              newFilmStudio = await _filmStudioRepository.AddAsync(newFilmStudio);
              var newUser = new User
              {
                   Username = registerModel.Username,
                   Password = BCryptHelper.HashPassword(registerModel.Password),
                   Role = "filmstudio",
                   FilmStudioId = newFilmStudio.FilmStudioId
              };
              newUser = await _userRepository.AddAsync(newUser);
              var token = JwtHelper.GenerateJwtToken(newUser, _configuration);
              return (newFilmStudio, newUser, token);
         }

         public void Logout(string token)
         {
              tokenBlacklist.Add(token);
         }

         public static bool IsTokenBlacklisted(string token)
         {
              return tokenBlacklist.Contains(token);
         }
    }
}
