using Microsoft.AspNetCore.Mvc;
using SurfsUpAPI.Data;
using SurfsUpAPI.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SurfsUpAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardEquipmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoardEquipmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<BoardEquipment>> Get()
        {
            try
            {
                IQueryable<BoardEquipment> BoardEquipment = from s in _context.BoardEquipment select s;
                List<BoardEquipment> boardEquipment = BoardEquipment.ToList();
                if (boardEquipment == null)
                    return NotFound();
                return Ok(boardEquipment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<BoardEquipment> GetEquipment(int id)
        {
            try
            {
                if (_context.BoardEquipment == null)
                {
                    return NotFound();
                }

                BoardEquipment be = _context.BoardEquipment.FirstOrDefault(m => m.BoardEquipmentId == id);
                if (be == null)
                {
                    return NotFound();
                }
                return Ok(be);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(BoardEquipment be)
        {
            try
            {
                if (be == null)
                { 
                    return BadRequest(); 
                }
                _context.BoardEquipment.Add(be);
                await _context.SaveChangesAsync();
                return Ok(be);
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
                BoardEquipment be = _context.BoardEquipment.FirstOrDefault(m => m.BoardEquipmentId == id);
                if (be == null)
                {
                    return NotFound($"BoardEquipment with id:{id} could not be found.");
                }
                _context.Remove(be);
                await _context.SaveChangesAsync();
                return Ok(be);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex}");
            }
        }
    }
}
