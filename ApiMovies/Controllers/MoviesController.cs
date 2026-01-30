using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeFirstClases.Contexts;
using CodeFirstClases.Models;

namespace ApiMovies.Controllers
{
    //http://localhost/api/Movies
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesContext _context;

        //constructor q inicializa el  atributo de la clase
        public MoviesController(MoviesContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            //return await _context.Movies.ToListAsync();

            return await _context.Movies
            .Include(m => m.Genre) //Incluir objeto genero
            .Where(m => m.Active == true) //filtrar por peliculas activas
            .ToListAsync();

        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        // PUT: api/genres/deactivate/{id}
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound("película no encontrado."); //retorna codigo 404, infica que no existe 
            }
            // Realizar la eliminación lógica
            movie.Active = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar la película.");
            }
            return NoContent(); // Indica éxito sin devolver contenido, retorna respuesta http codigo 204
                                
        }

        // GET: api/Movies/getMovieAll/5
        [HttpGet("getMovieAll/{id}")]
        public async Task<ActionResult<Movie>> GetMovieAll(int id)
        {
            //var movie = await _context.Movies.FindAsync(id);
            var movie = await _context.Movies.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // Obtener todas las películas o buscar por título o genre solo activas
        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies(string? search, int? genreId)
        {  // Comenzamos con la lista de todas las películas
           //var moviesQuery = _context.Movies.AsQueryable(); //activas y no activas
           // Comenzamos con la lista de todas las películas activas
            var moviesQuery = _context.Movies
                                      .Include(m => m.Genre) // Incluir información del género
                                      .Where(m => m.Active) // Filtrar por películas activas
                                      .AsQueryable();
            // Si se proporciona un parámetro de búsqueda, filtramos por título
            if (!string.IsNullOrEmpty(search))
            {
                moviesQuery = moviesQuery.Where(m => m.Title.Contains(search));
            }
            // Si se proporciona un parámetro de género, filtramos por género
            if (genreId != null)
            {
                moviesQuery = moviesQuery.Where(m => m.Genre.Id == genreId);
            }
            // Ejecutamos la consulta y obtenemos los resultados
            var movies = await moviesQuery.ToListAsync();
            if (!movies.Any())
            {
                return NotFound("No se encontraron películas que coincidan con los criterios.");
            }
            return Ok(movies);
        }

    }
}
