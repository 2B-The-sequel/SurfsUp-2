using Microsoft.AspNetCore.Mvc;
using SurfsUpAPI.Data;
using SurfsUpAPI.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

        protected abstract DbSet<T> Set();

        [HttpGet]
        public ActionResult<List<T>> Get()
        {
            try
            {
                List<T> list = Set().ToList();
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
        public ActionResult<T> Get(int id)
        {
            try
            {
                if (Set() == null)
                {
                    return NotFound();
                }

                T item = Set().FirstOrDefault(t => t.Id == id);
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

        [HttpPost]
        public async Task<ActionResult> Create(T item)
        {
            try
            {
                if (item == null)
                {
                    return BadRequest();
                }
                Set().Add(item);
                await _context.SaveChangesAsync();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<T>> Update(T item)
        {
            try
            {
                T findInDatabase = Set().FirstOrDefault(t => t.Id == item.Id);
                if (findInDatabase == null)
                {
                    return NotFound($"{nameof(T)} with id = {item.Id} not found ");
                }
                
                Set().Update(item);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                T item = Set().FirstOrDefault(m => m.Id == id);
                if (item == null)
                {
                    return NotFound($"{nameof(T)} with id:{id} could not be found.");
                }
                Set().Remove(item);
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
