using System.Text.RegularExpressions;

namespace Filmstudion.API.Helpers
{

    public static class PasswordPolicy
    {

         public static bool Validate(string password)
         {
              if(string.IsNullOrWhiteSpace(password))
                  return false;
              
              
              var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
              return regex.IsMatch(password);
         }
    }
}
