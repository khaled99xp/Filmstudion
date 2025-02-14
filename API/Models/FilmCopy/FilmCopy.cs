using System;
using System.Text.Json.Serialization;
using Filmstudion.API.Interfaces;

namespace Filmstudion.API.Models.FilmCopy
{
    public class FilmCopy : IFilmCopy
    {
        public int FilmCopyId { get; set; }
        public int FilmId { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? RentalStartDate { get; set; }
        public DateTime? RentalEndDate { get; set; }

        [JsonIgnore]
        public Filmstudion.API.Models.Film.Film Film { get; set; }

        public int? FilmStudioId { get; set; }
        public Filmstudion.API.Models.FilmStudio.FilmStudio FilmStudio { get; set; }
    }
}