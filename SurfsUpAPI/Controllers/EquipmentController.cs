using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUpAPI.Data;
using SurfsUpAPI.Models;

namespace SurfsUpAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : GenericAPIController<Equipment>
    {
        public EquipmentController(ApplicationDbContext context) : base(context) { }

        protected override DbSet<Equipment> Set()
        {
            return _context.Equipment;
        }
    }
}
