using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUpAPI.Data;
using SurfsUpAPI.Models;

namespace SurfsUpAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardsController : GenericAPIController<Board>
    {
        public BoardsController(ApplicationDbContext context) : base(context) { }

        protected override DbSet<Board> Set()
        {
            return _context.Board;
        }
    }
}