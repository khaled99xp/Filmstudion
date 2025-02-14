using BCrypt.Net;

namespace Filmstudion.API.Helpers
{

     public static class BCryptHelper
     {
          public static string HashPassword(string password)
          {
               return BCrypt.Net.BCrypt.HashPassword(password);
          }
          public static bool VerifyPassword(string password, string hash)
          {
               return BCrypt.Net.BCrypt.Verify(password, hash);
          }
     }
}
