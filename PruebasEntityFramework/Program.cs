using CodeFirstClases.Contexts;
using CodeFirstClases.Models;
using Microsoft.EntityFrameworkCore;

using (var context = new MoviesContext())
{
    //crear nuevo genero 
    var genre = new Genre()
    {
        Name = "Comedy",
        Description = "peliculas que te hacen reir "
    }; 
    context.Genres.Add(genre);//insertando un genero 
    context.SaveChanges();
    //crear una nueva movie 
    var movie = new Movie()
    {
        Title = "Dos hombres en fuga",
        GenreId = genre.Id,
        ReleaseDate = new DateTime(2010, 7, 16),
        Poster = "peli.jpg",
        Budget = 123000,
        Rating = 8.4,
        IsAvailable = true,
        Active = true
    }; 
    context.Movies.Add(movie);
    context.SaveChanges();

    //consultar peliculas 
    var movies = context.Movies.Include(m=>m.Genre).ToList();
    foreach(var mov in movies)
    {
        Console.WriteLine(mov.Title + " " + mov.Genre.Name);
        
    }


}