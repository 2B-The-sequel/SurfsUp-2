using Microsoft.AspNetCore.Mvc;
using SurfsUpAPI.Data;
using SurfsUpAPI.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

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
            IQueryable<BoardEquipment> BoardEquipment = from s in _context.BoardEquipment select s;
            List<BoardEquipment> boardEquipment = BoardEquipment.ToList();
            if (boardEquipment == null)
                return NotFound();
            return Ok(boardEquipment);
        }

        [HttpGet("{id:int}")]
        public ActionResult GetEquipment(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public ActionResult Create(BoardEquipment equipment)
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Update(BoardEquipment equipment)
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error retrieving data from the database");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteBoardEquipment(int id)
        {
            try
            {
                return Ok(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
