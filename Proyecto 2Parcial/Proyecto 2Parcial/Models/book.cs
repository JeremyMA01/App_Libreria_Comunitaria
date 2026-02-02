using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto_2Parcial.Models
{
    public class Book
    {
        public int Id {  get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public int GenreId { get; set; }
        public bool Active { get; set; }
        public DateTime ReleaseDate {  get; set; }
        public decimal? Budget { get; set; }
        public string Poster {  get; set; }

        public virtual Genre Genre { get; set; }


    }
}
