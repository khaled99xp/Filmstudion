
using Filmstudion.API.Interfaces;
using MediatR;

namespace Filmstudion.API.Mediator.Commands
{
     public class CreateFilmCommand : IRequest<Filmstudion.API.Models.Film.Film>
     {
          public ICreateFilm CreateFilm { get; set; }
          public CreateFilmCommand(ICreateFilm createFilm)
          {
               CreateFilm = createFilm;
          }
     }
}
