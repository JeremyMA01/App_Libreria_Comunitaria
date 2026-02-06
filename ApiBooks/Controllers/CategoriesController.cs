using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App_Libreria_Comunitaria.Contexts;
using App_Libreria_Comunitaria.Models;
using System.Text.RegularExpressions;

namespace ApiBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly LibreriaContexts _context;

        public CategoriesController(LibreriaContexts context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound("Categoría no encontrada");
            }

            return category;
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest("El ID de la categoría no coincide");
            }

            // Validaciones adicionales
            var validationResult = ValidateCategory(category);
            if (!string.IsNullOrEmpty(validationResult))
            {
                return BadRequest(validationResult);
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound("Categoría no encontrada");
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Categoría actualizada exitosamente" });
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            
            var validationResult = ValidateCategory(category);
            if (!string.IsNullOrEmpty(validationResult))
            {
                return BadRequest(validationResult);
            }

            
            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower()))
            {
                return BadRequest("Ya existe una categoría con ese nombre");
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Categoría no encontrada");
            }


            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Categoría eliminada exitosamente" });
        }

        // GET: api/Categories/search?search=term
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Category>>> SearchCategories(string? search)
        {
            IQueryable<Category> query = _context.Categories;

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(search) ||
                    c.Description.ToLower().Contains(search));
            }

            var categories = await query.ToListAsync();

            if (!categories.Any())
            {
                return NotFound("No se encontraron categorías que coincidan con la búsqueda");
            }

            return categories;
        }

        // PUT: api/Categories/deactivate/5
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Categoría no encontrada");
            }

            category.Active = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar la categoría");
            }

            return Ok(new { message = "Categoría desactivada exitosamente" });
        }

        // PUT: api/Categories/activate/5
        [HttpPut("activate/{id}")]
        public async Task<IActionResult> ActivateCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Categoría no encontrada");
            }

            category.Active = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar la categoría");
            }

            return Ok(new { message = "Categoría activada exitosamente" });
        }

        // GET: api/Categories/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Category>>> GetActiveCategories()
        {
            return await _context.Categories.Where(c => c.Active).ToListAsync();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        // VALIDACIONES
        private string ValidateCategory(Category category)
        {
            
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                return "El nombre es obligatorio";
            }

            if (category.Name.Length < 3 || category.Name.Length > 50)
            {
                return "El nombre debe tener entre 3 y 50 caracteres";
            }

           
            if (!Regex.IsMatch(category.Name, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                return "El nombre solo puede contener letras y espacios";
            }

    
            if (string.IsNullOrWhiteSpace(category.Description))
            {
                return "La descripción es obligatoria";
            }

            if (category.Description.Length < 10 || category.Description.Length > 200)
            {
                return "La descripción debe tener entre 10 y 200 caracteres";
            }

         
            if (category.CreatedDate == default)
            {
                return "La fecha de creación es obligatoria";
            }

            if (category.CreatedDate > DateTime.Now)
            {
                return "La fecha de creación no puede ser futura";
            }

            return string.Empty;
        }
    }
}