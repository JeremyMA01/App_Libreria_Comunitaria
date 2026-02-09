// ApiLibreria/Controllers/DonationsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App_Libreria_Comunitaria.Contexts;
using App_Libreria_Comunitaria.Models;
using System.Text.RegularExpressions;

namespace ApiBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationsController : ControllerBase
    {
        private readonly LibreriaContexts _context;

        public DonationsController(LibreriaContexts context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donation>>> GetDonations()
        {
            return await _context.Donations
                .Include(d => d.Category)
                .ToListAsync();
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Donation>>> GetActiveDonations()
        {
            return await _context.Donations
                .Include(d => d.Category)
                .Where(d => d.Active)
                .ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Donation>> GetDonation(int id)
        {
            var donation = await _context.Donations
                .Include(d => d.Category)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
            {
                return NotFound("Donación no encontrada");
            }

            return donation;
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonation(int id, Donation donation)
        {
            if (id != donation.Id)
            {
                return BadRequest("El ID de la donación no coincide");
            }

            
            var validationResult = ValidateDonation(donation);
            if (!string.IsNullOrEmpty(validationResult))
            {
                return BadRequest(validationResult);
            }

            
            if (!await _context.Categories.AnyAsync(c => c.Id == donation.CategoryId && c.Active))
            {
                return BadRequest("La categoría seleccionada no existe o no está activa");
            }

            _context.Entry(donation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationExists(id))
                {
                    return NotFound("Donación no encontrada");
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Donación actualizada exitosamente" });
        }

        [HttpPut("activate/{id}")]
        public async Task<IActionResult> ActivateDonation(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound("Donación no encontrada");
            }

            donation.Active = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationExists(id))
                {
                    return NotFound("Donación no encontrada");
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Donación reactivada exitosamente" });
        }


        [HttpPost]
        public async Task<ActionResult<Donation>> PostDonation(Donation donation)
        {
            // Asegurar que Active sea true por defecto
            donation.Active = true;

            var validationResult = ValidateDonation(donation);
            if (!string.IsNullOrEmpty(validationResult))
            {
                return BadRequest(validationResult);
            }

            if (!await _context.Categories.AnyAsync(c => c.Id == donation.CategoryId && c.Active))
            {
                return BadRequest("La categoría seleccionada no existe o no está activa");
            }

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDonation", new { id = donation.Id }, donation);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound("Donación no encontrada");
            }

            // En lugar de eliminar físicamente, marcamos como inactivo
            donation.Active = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationExists(id))
                {
                    return NotFound("Donación no encontrada");
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Donación desactivada exitosamente" });
        }


        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Donation>>> SearchDonations(string? search)
        {
            IQueryable<Donation> query = _context.Donations
                .Include(d => d.Category);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(d =>
                    d.Title.ToLower().Contains(search) ||
                    d.Author.ToLower().Contains(search) ||
                    d.DonatedBy.ToLower().Contains(search));
            }

            var donations = await query.ToListAsync();

            if (!donations.Any())
            {
                return NotFound("No se encontraron donaciones que coincidan con la búsqueda");
            }

            return donations;
        }

        
        [HttpPut("convert-to-inventory/{id}")]
        public async Task<IActionResult> ConvertToInventory(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound("Donación no encontrada");
            }

            if (donation.ConvertedToInventory)
            {
                return BadRequest("Esta donación ya fue convertida a inventario");
            }

            
            var book = new Book
            {
                Title = donation.Title,
                Author = donation.Author,
                Year = donation.Year,
                GenreId = 1, 
                Active = true,
                ReleaseDate = DateTime.Now,
                Budget = 0,
                Poster = ""
            };

            
            donation.ConvertedToInventory = true;

            _context.books.Add(book);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Donación convertida a inventario exitosamente",
                bookId = book.Id
            });
        }

        
        [HttpGet("not-converted")]
        public async Task<ActionResult<IEnumerable<Donation>>> GetNotConvertedDonations()
        {
            return await _context.Donations
                .Include(d => d.Category)
                .Where(d => !d.ConvertedToInventory)
                .ToListAsync();
        }

        private bool DonationExists(int id)
        {
            return _context.Donations.Any(e => e.Id == id);
        }

        private string ValidateDonation(Donation donation)
        {
            
            if (string.IsNullOrWhiteSpace(donation.Title))
            {
                return "El título es obligatorio";
            }

            if (donation.Title.Length < 3 || donation.Title.Length > 200)
            {
                return "El título debe tener entre 3 y 200 caracteres";
            }

            
            if (string.IsNullOrWhiteSpace(donation.Author))
            {
                return "El autor es obligatorio";
            }

            if (donation.Author.Length < 3 || donation.Author.Length > 200)
            {
                return "El autor debe tener entre 3 y 200 caracteres";
            }

            
            if (donation.Year < 1000 || donation.Year > DateTime.Now.Year)
            {
                return $"El año debe estar entre 1000 y {DateTime.Now.Year}";
            }

            
            if (string.IsNullOrWhiteSpace(donation.DonatedBy))
            {
                return "El nombre del donante es obligatorio";
            }

            if (donation.DonatedBy.Length < 3 || donation.DonatedBy.Length > 100)
            {
                return "El nombre del donante debe tener entre 3 y 100 caracteres";
            }

            
            if (string.IsNullOrWhiteSpace(donation.Estado))
            {
                return "El estado del libro es obligatorio";
            }

            var validStates = new[] { "Nuevo", "Usado", "Excelente", "Bueno", "Regular" };
            if (!validStates.Contains(donation.Estado))
            {
                return $"El estado debe ser uno de: {string.Join(", ", validStates)}";
            }

            return string.Empty;
        }
    }
}