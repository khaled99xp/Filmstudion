
namespace Filmstudion.API.Interfaces
{
    public interface IUser
    {
        int UserId { get; set; }
        string Username { get; set; }
        string Role { get; set; }
        int? FilmStudioId { get; set; }
    }
}
