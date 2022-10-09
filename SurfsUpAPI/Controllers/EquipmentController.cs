using Microsoft.AspNetCore.Mvc;
using SurfsUp.Data;
using SurfsUp.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using SurfsUp.Data.Migrations;
using Equipment = SurfsUp.Models.Equipment;

namespace SurfsUpAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetEquipments()
        {
            try
            {
                List<Equipment> equipment = await _context.Equipment.ToListAsync();
                return Ok(equipment);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        //Finder det Equipment objekt med det tilhørende id fra db
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Equipment>> GetEquipment(int id)
        {
            try
            {
                if (id == null || _context.Equipment == null)
                {
                    return NotFound();
                }

                Equipment equipment = await _context.Equipment.FirstOrDefaultAsync(m => m.EquipmentId == id);
                if (equipment == null)
                {
                    return NotFound();
                }
                return Ok(equipment);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                  "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Equipment>> CreateEquipment(Equipment equipment)
        {
            try
            {
                if(equipment == null)
                { return BadRequest(); }
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Equipment>> UpdateEquipment(int id, Equipment equipment)
        {
            try
            {
                if (id != equipment.EquipmentId)
                {
                    return BadRequest("Equipment ID mismatch");
                }
                var equipmentToUpdate = await _context.Equipment.FindAsync(id);
                if (equipmentToUpdate == null)
                { 
                    return NotFound($"Equipment with id = {id} not found "); 
                 }

                equipmentToUpdate.Name = equipment.Name;
                equipmentToUpdate.Boards = equipment.Boards;
                equipmentToUpdate.BoardEquipments = equipment.BoardEquipments;

                _context.Update(equipmentToUpdate);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEquipment (int id)
        {
            try
            {
                var equipmentToDelete = await _context.Equipment.FindAsync(id);
                if(equipmentToDelete == null)
                {
                    return NotFound($"Equipment with id:{id} could not be found");
                }
                _context.Remove(equipmentToDelete);
               await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database");
            }

        }

    }
}
