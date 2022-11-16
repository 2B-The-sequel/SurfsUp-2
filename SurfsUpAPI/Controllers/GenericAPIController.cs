using SurfsUpAPI.Data;
using SurfsUpLibrary.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Cors;

namespace SurfsUpAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // IIdentifiable kræver at 'T' har en property der hedder 'Id'. T : class kræver at 'T' er en reference type
    public abstract class GenericAPIController<T> : Controller where T : class, IIdentifiable
    {
        protected readonly ApplicationDbContext _context;
        private static readonly string[] authorized_apikeys = { "4d1bb604-377f-41e0-99c7-59846080bb47" };

        public GenericAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public virtual ActionResult<T> Create(string apikey, T item)
        {
            try
            {
                // Check if apikey is valid
                ObjectResult result = IsKeyValid(apikey);
                if (result.StatusCode != 200)
                    return result;

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
        public virtual ActionResult<List<T>> Retrieve(string apikey)
        {
            try
            {
                // Check if apikey is valid
                ObjectResult result = IsKeyValid(apikey);
                if (result.StatusCode != 200)
                    return result;

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
        public virtual ActionResult<T> Retrieve(string apikey, int Id)
        {
            try
            {
                // Check if apikey is valid
                ObjectResult result = IsKeyValid(apikey);
                if (result.StatusCode != 200)
                    return result;

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
        public virtual ActionResult<T> Update(string apikey, T item)
        {
            try
            {
                // Check if apikey is valid
                ObjectResult result = IsKeyValid(apikey);
                if (result.StatusCode != 200)
                    return result;

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
        public virtual ActionResult<T> Delete(string apikey, int Id)
        {
            try
            {
                // Check if apikey is valid
                ObjectResult result = IsKeyValid(apikey);
                if (result.StatusCode != 200)
                    return result;

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

        private ObjectResult IsKeyValid(string apikey)
        {
            if (apikey == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "An apikey is required. Insert apikey in query as string.");
            }
            else
            {
                bool found = false;
                int i = 0;
                while (i < authorized_apikeys.Length && !found)
                {
                    if (authorized_apikeys[i] == apikey)
                        found = true;
                    i++;
                }
                if (!found)
                    return StatusCode(StatusCodes.Status401Unauthorized, "Invalid apikey.");
            }

            return StatusCode(StatusCodes.Status200OK, "Key is valid.");
        }
    }
}
