using CodeFirstClases.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFirstClases.Contexts
{
    public class MoviesContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public MoviesContext(DbContextOptions<MoviesContext> options)
              : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la relación Movie-Genre
            //cómo se relaciona Movie con otras entidades (en este caso, Genre).
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Genre) //Esto indica que Movie tiene una propiedad de navegación llamada Genre que apunta a una única entidad Genre.
                .WithMany(g => g.Movies) //Aquí defines que Genre tiene una colección de películas (Movies) como propiedad de navegación.
                .HasForeignKey(m => m.GenreId);//Indica que la relación se establece mediante una clave foránea en la tabla Movie.

            /*Ejemplo si movie tuviera mas relaciones Relación Movie -> Director (uno a muchos)
         modelBuilder.Entity<Movie>()
        .HasOne(m => m.Director)         // Movie tiene una relación con Director
        .WithMany(d => d.Movies)         // Director tiene muchas Movies
        .HasForeignKey(m => m.DirectorId) // Clave foránea en Movie
            */
            base.OnModelCreating(modelBuilder);
        }
    }

}
