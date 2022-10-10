using Microsoft.AspNetCore.Mvc;
using SurfsUpAPI.Data;
using SurfsUpAPI.Models;
using System.Linq;
using System.Collections.Generic;

namespace SurfsUpAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BoardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Board> Get()
        {
            IQueryable<Board> Board = from s in _context.Board select s;
            List<Board> boards = Board.ToList();
            return boards;
        }
    }
}