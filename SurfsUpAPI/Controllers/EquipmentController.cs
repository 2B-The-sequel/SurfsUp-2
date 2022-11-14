using SurfsUpAPI.Data;
using SurfsUpLibrary.Models;

namespace SurfsUpAPI.Controllers
{
    public class EquipmentController : GenericAPIController<Equipment>
    {
        public EquipmentController(ApplicationDbContext context) : base(context) { }
    }
}
