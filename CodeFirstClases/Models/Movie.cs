using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFirstClases.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int GenreId { get; set; } // Clave foránea
        public DateTime ReleaseDate { get; set; }
        public bool IsAvailable { get; set; }
        public double? Rating { get; set; }
        public string Poster { get; set; }
        public decimal? Budget { get; set; }
        public bool Active { get; set; }

        // Relación con Genre
        public virtual Genre Genre { get; set; }
    }

}
