using Microsoft.AspNetCore.Mvc;
using SurfsUp.Data;
using SurfsUp.Models;
using System.Linq;
using System.Collections.Generic;

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
        public List<Equipment> Get()
        {
            IQueryable<Equipment> Equipment = from s in _context.Equipment select s;
            List<Equipment> equipment = Equipment.ToList();
            return equipment;
        }
    }
}
