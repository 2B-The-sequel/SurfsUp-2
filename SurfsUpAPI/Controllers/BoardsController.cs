using Microsoft.EntityFrameworkCore;
using SurfsUpAPI.Data;
using SurfsUpAPI.Models;

namespace SurfsUpAPI.Controllers
{
    public class BoardsController : GenericAPIController<Board>
    {
        public BoardsController(ApplicationDbContext context) : base(context) { }

        protected override DbSet<Board> Set()
        {
            return _context.Board;
        }
    }
}