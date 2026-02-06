using App_Libreria_Comunitaria.Contexts;
using App_Libreria_Comunitaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibreriaContexts _context;

        public BooksController(LibreriaContexts context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Getbooks()
        {
            return await _context.books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.books.FindAsync(id);

            if (book == null)
            {
                return NotFound("Libro no encontrado");
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound("libro no encontrado");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.books.FindAsync(id);
            if (book == null)
            {
                return NotFound("Libro no encontrado");
            }

            _context.books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.books.Any(e => e.Id == id);
        }

        // PUT: api/genres/deactivate/{id}
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateMovie(int id)
        {
            var book = await _context.books.FindAsync(id);
            if (book == null)
            {
                return NotFound("libro no encontrado.");
            }

            book.Active = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar el libro");
            }
            return NoContent();
        }
    
        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(string? search, int? genreId)
        {
            var booksQuery = _context.books.Include(m => m.Genre).Where(m => m.Active).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                booksQuery = booksQuery.Where(m => m.Title.Contains(search));
            }

            if (genreId != null)
            {
                booksQuery = booksQuery.Where(m => m.Genre.Id == genreId);
            }

            var books = await booksQuery.ToListAsync();
            if (!books.Any())
            {
                return NotFound("No se encontraron libros que coincidan con los criterios.");
            }
            return Ok(books);
        }

    }
}
