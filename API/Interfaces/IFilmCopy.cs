using System;

namespace Filmstudion.API.Interfaces
{
    public interface IFilmCopy
    {
         int FilmCopyId { get; set; }
         int FilmId { get; set; }
         bool IsAvailable { get; set; }
         DateTime? RentalStartDate { get; set; }
         DateTime? RentalEndDate { get; set; }
    }
}
