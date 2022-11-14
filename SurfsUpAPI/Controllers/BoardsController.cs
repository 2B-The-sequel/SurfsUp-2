using SurfsUpAPI.Data;
using SurfsUpLibrary.Models;

namespace SurfsUpAPI.Controllers
{
    public class BoardsController : GenericAPIController<Board>
    {
        public BoardsController(ApplicationDbContext context) : base(context) { }
    }
}