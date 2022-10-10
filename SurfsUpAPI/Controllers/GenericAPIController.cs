using Microsoft.AspNetCore.Mvc;
using SurfsUpAPI.Data;
using SurfsUpAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace SurfsUpAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class GenericAPIController<T> : Controller where T : class, IIdentifiable
    {
        protected readonly ApplicationDbContext _context;

        public GenericAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public virtual async Task<ActionResult<T>> Create(T item)
        {
            try
            {
                if (item == null)
                {
                    return BadRequest();
                }
                _context.Set<T>().Add(item);
                await _context.SaveChangesAsync();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }

        [HttpGet]
        public virtual ActionResult<List<T>> Retrieve()
        {
            try
            {
                List<T> list = _context.Set<T>().ToList();
                if (list == null)
                    return NotFound();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }

        [HttpGet("{id:int}")]
        public virtual ActionResult<T> Retrieve(int id)
        {
            try
            {
                if (_context.Set<T>() == null)
                {
                    return NotFound();
                }

                T item = _context.Set<T>().FirstOrDefault(t => t.Id == id);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }

        [HttpPut("{id:int}")]
        public virtual async Task<ActionResult<T>> Update(T item)
        {
            try
            {
                T findInDatabase = _context.Set<T>().FirstOrDefault(t => t.Id == item.Id);
                if (findInDatabase == null)
                {
                    return NotFound($"{nameof(T)} with id = {item.Id} not found ");
                }

                _context.Set<T>().Update(item);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }

        [HttpDelete("{id:int}")]
        public virtual async Task<ActionResult<T>> Delete(int id)
        {
            try
            {
                T item = _context.Set<T>().FirstOrDefault(m => m.Id == id);
                if (item == null)
                {
                    return NotFound($"{nameof(T)} with id:{id} could not be found.");
                }
                _context.Set<T>().Remove(item);
                await _context.SaveChangesAsync();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }
    }
}
