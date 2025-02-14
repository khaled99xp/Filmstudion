
using Filmstudion.API.Models.Film;
using Filmstudion.API.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Filmstudion.API.Mediator.Handlers
{
     public class CreateFilmCommandHandler : IRequestHandler<Filmstudion.API.Mediator.Commands.CreateFilmCommand, Film>
     {
          private readonly IFilmService _filmService;
          public CreateFilmCommandHandler(IFilmService filmService)
          {
               _filmService = filmService;
          }

          public async Task<Film> Handle(Filmstudion.API.Mediator.Commands.CreateFilmCommand request, CancellationToken cancellationToken)
          {
               return await _filmService.CreateFilmAsync(request.CreateFilm);
          }
     }
}
