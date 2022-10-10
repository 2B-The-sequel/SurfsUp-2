using SurfsUpAPI.Data;
using SurfsUpAPI.Models;

namespace SurfsUpAPI.Controllers
{
    public class BoardEquipmentController : GenericAPIController<BoardEquipment>
    {
        public BoardEquipmentController(ApplicationDbContext context) : base(context) { }
    }
}