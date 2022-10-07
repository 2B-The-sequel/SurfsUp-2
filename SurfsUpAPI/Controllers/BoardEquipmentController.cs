using Microsoft.AspNetCore.Mvc;
using SurfsUp.Data;
using SurfsUp.Models;
using System.Linq;
using System.Collections.Generic;

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
        public List<BoardEquipment> Get()
        {
            IQueryable<BoardEquipment> BoardEquipment = from s in _context.BoardEquipment select s;
            List<BoardEquipment> boardEquipment = BoardEquipment.ToList();
            return boardEquipment;
        }
    }
}
