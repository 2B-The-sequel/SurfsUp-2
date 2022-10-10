using SurfsUpAPI.Data;
using SurfsUpAPI.Models;

namespace SurfsUpAPI.Controllers
{
    public class BoardsController : GenericAPIController<Board>
    {
        public BoardsController(ApplicationDbContext context) : base(context) { }
    }
}