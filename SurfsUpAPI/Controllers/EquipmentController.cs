using SurfsUpAPI.Data;
using SurfsUpAPI.Models;

namespace SurfsUpAPI.Controllers
{
    public class EquipmentController : GenericAPIController<Equipment>
    {
        public EquipmentController(ApplicationDbContext context) : base(context) { }
    }
}
