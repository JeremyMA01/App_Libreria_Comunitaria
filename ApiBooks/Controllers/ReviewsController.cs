using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App_Libreria_Comunitaria.Contexts;
using App_Libreria_Comunitaria.Models;

namespace ApiLibreria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly BookContexts _context;

        public ReviewsController(BookContexts context)
        {
            _context = context;
        }


        /*
        ============================================================================== 
                                   SERVICIOS REVIEW
        ==============================================================================
         */

        /*
            Función: Cargar Todas las Reseñas en la Tabla.
            Nota: Implementado en el FrontEnd
        */
        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> getReviews()
        {
            return await _context.reviews.ToListAsync();
        }


        /*
            Función: Cargar Todas las Reseñas Asociadas a un Libro en Especifico en la 
            Interfaz Gráfica.
            Nota: Implementado en el FrontEnd
        */
        [HttpGet("Book/{id_book}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsBook(int id_book)
        {
            var review = await _context.reviews
                .AsNoTracking()
                .Where(r => r.id_book == id_book)
                .ToListAsync();

            return Ok(review);
        }


        /*
            Función: Agregar Reseñas a la Base de Datos.
            Nota: Implementado en el FrontEnd
        */
        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult<Review>> addReview(Review review)
        {
            _context.reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.Id }, review);
        }

        /*
            Función: Actualiza las Reseñas en la Base de Datos.
            Nota: Implementado en el FrontEnd
        */
        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> updateReview(int id, Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        /*
            Función: Elimina la Reseña en la Base de Datos.
            Nota: Implementado en el FrontEnd
        */
        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*
            Función: Permite Cargar las Reseñas Asociadas a un Usuario -- Tabla
            Nota: Ya implementado en el FrontEnd
        */
        [HttpGet("search/{user}")]
        public async Task<ActionResult<IEnumerable<Review>>> searchUserReview(string user)
        {
            var usuario = await _context.reviews.AsNoTracking()
                .Where(r => r.User.ToLower().Contains(user.ToLower()))
                .ToListAsync();
            return Ok(usuario);
        }

        /*
            Método getReviewId Permite Cargar una reseña mediante su ID
            Mediante su Id
            --No tiene aplicación en el proyecto 04/02/2026--
        */
        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> getReviewId(int id)
        {
            var review = await _context.reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }


        /*
            Función: Busca los Usuarios que Hayan Publicado una Reseña en un Libro en Especifico -- GUI
            Nota: Implementado en el FrontEnd
        */

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Review>>> searchUserReview(string user, int id_book)
        {
            var query = _context.reviews.AsNoTracking().
                Where(r => r.id_book == id_book);

            if (!string.IsNullOrEmpty(user))
            {
                query = query.Where(r => r.User.Contains(user));
            }

            return await query.ToListAsync();
        }

        /*
            Función: Permite Filtrar las Reseñas por Puntaje -- Tabla
            Nota: Implementado en FrontEnd
        */
        [HttpGet("search/{score:int}")]
        public async Task<ActionResult<IEnumerable<Review>>> getScoreFilter(int score)
        {
            if (score < 0 || score > 5)
            {
                return BadRequest("Puntuación Invalida");
            }

            var review = await _context.reviews.AsNoTracking()
                  .Where(r => r.Score == score)
                   .ToListAsync();
            return Ok(review);
        }

        private bool ReviewExists(int id)
        {
            return _context.reviews.Any(e => e.Id == id);
        }
    }
}