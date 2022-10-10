using Microsoft.AspNetCore.Mvc;
using SurfsUpAPI.Data;
using SurfsUpAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace SurfsUpAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // IIdentifiable kræver at 'T' har en property der hedder 'Id'. T : class kræver at 'T' er en reference type
    public abstract class GenericAPIController<T> : Controller where T : class, IIdentifiable
    {
        protected readonly ApplicationDbContext _context;

        public GenericAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public virtual ActionResult<T> Create(T item)
        {
            try
            {
                // Er det et valid item der sendes til API?
                if (item == null)
                {
                    return BadRequest();
                }

                // Tilføj item til databasen. Sætningen er det samme som _context.Board.Add(board) osv.
                _context.Set<T>().Add(item);
                
                // Gem ændringerne
                _context.SaveChanges();
                
                // Returnér item med det tildelte id
                return Ok(item);
            }
            catch (Exception ex)
            {
                // Send fejl hvis der sker en exception.
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }

        [HttpGet]
        public virtual ActionResult<List<T>> Retrieve()
        {
            try
            {
                // Findes T i databasen?
                if (_context.Set<T>() == null)
                {
                    return NotFound();
                }

                // Hent en liste af T fra databasen. Er det samme som _context.Board.ToList() osv.
                List<T> list = _context.Set<T>().ToList();

                // Findes listen?
                if (list == null)
                    return NotFound();

                // Returnér listen hvis den findes
                return Ok(list);
            }
            catch (Exception ex)
            {
                // Send fejl hvis der sker en exception.
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }

        [HttpGet("{Id:int}")]
        public virtual ActionResult<T> Retrieve(int Id)
        {
            try
            {
                // Findes T i databasen?
                if (_context.Set<T>() == null)
                {
                    return NotFound();
                }

                // Find T med et givent Id.  Er det samme som _context.Board.FirstOrDefault(t => t.Id == Id) osv.
                T item = _context.Set<T>().FirstOrDefault(t => t.Id == Id);

                // Blev dette item fundet?
                if (item == null)
                {
                    return NotFound($"{nameof(T)} with id:{Id} could not be found.");
                }

                // Returnér item hvis det findes
                return Ok(item);
            }
            catch (Exception ex)
            {
                // Send fejl hvis der sker en exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }

        [HttpPut]
        public virtual ActionResult<T> Update(T item)
        {
            try
            {
                // Find item i databasen før det opdateres
                T findInDatabase = _context.Set<T>().AsNoTracking().FirstOrDefault(t => t.Id == item.Id); //AsNoTracking er nødvendigt, for ellers giver Databasen ikke slip når Update kaldes.

                // Findes det i databasen?
                if (findInDatabase == null)
                {
                    return NotFound($"{typeof(T).FullName} with id = {item.Id} not found ");
                }

                // Opdatér databasen med item
                EntityEntry<T> entry = _context.Set<T>().Update(item);

                // Gem ændringer
                _context.SaveChanges();

                // Returnér det opdateret item
                return Ok(item);
            }
            catch (Exception ex)
            {
                // Send fejl hvis der sker en exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }

        [HttpDelete("{Id:int}")]
        public virtual ActionResult<T> Delete(int Id)
        {
            try
            {
                // Find item med det givne Id
                T item = _context.Set<T>().AsNoTracking().FirstOrDefault(m => m.Id == Id);

                // Findes der et item med Id?
                if (item == null)
                {
                    return NotFound($"{typeof(T).FullName} with id:{Id} could not be found.");
                }

                // Hvis det findes, så fjern item fra databasen
                _context.Set<T>().Remove(item);

                // Gem ændringer
                _context.SaveChanges();

                // Returnér det slettede item
                return Ok(item);
            }
            catch (Exception ex)
            {
                // Send fejl hvis der sker en exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }
    }
}
