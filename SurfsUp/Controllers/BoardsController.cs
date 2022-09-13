using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Reflection;

namespace SurfsUp.Controllers
{
    
    public class BoardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Boards
        public async Task<IActionResult> Index(string sortOrder,string searchString, string currentFilter, int? pageNumber)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier as string);
            ViewData["UserId"] = claims.Value;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "type_desc" : "Type";
            ViewData["CurrentFilter"] = searchString;
            var Board = from s in _context.Board
                           select s;

            //Metode der tjekker både navn og type for match med searchString.
            //Tjek om den første char samt resten af alle chars i searchstring kronologisk passer med Type i hvert board.
            //Substring(searchString[0],searchString.Length)

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                bool hasChanged = false;
                BoardType Found = BoardType.Shortboard;
                searchString = searchString.ToLower();
                switch (searchString)
                {
                    case "sup":
                        Found = BoardType.SUP;
                        break;
                    case "shortboard":
                        hasChanged = true;
                        break;
                    case "funboard":
                        Found = BoardType.Funboard;
                        break;
                    case "fish":
                        Found = BoardType.Fish;
                        break;
                    case "longboard":
                        Found = BoardType.Longboard;
                        break;
                    default:
                        break;
                }
                string l = Found.ToString();
                if (hasChanged && Found == BoardType.Shortboard)
                { Board = Board.Where(s => s.Type == Found || s.Name.ToLower().Contains(searchString)); }
                else if (!hasChanged && Found == BoardType.Shortboard)
                { 
                    Board = Board.Where(s => s.Name.ToLower().Contains(searchString)); 
                }
                else
                {
                    Board = Board.Where(s => s.Type == Found || s.Name.ToLower().Contains(searchString));
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    Board = Board.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    Board = Board.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    Board = Board.OrderByDescending(s => s.Price);
                    break;
                case "Type":
                    Board = Board.OrderBy(s => s.Type);
                    break;
                case "type_desc":
                    Board = Board.OrderByDescending(s => s.Type);
                    break;
                default:
                    Board = Board.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 5;
            return View(await PaginatedList<Board>.CreateAsync(Board
                            .Include(e => e.BoardEquipments)
                            .ThenInclude(be => be.Equipment).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Boards/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.BoardId == id);
            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        // GET: Boards/Create
        // Husk og ændre ting i databasen så rollen er Admin
        [Authorize(Roles = "Adminstrators")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Boards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Husk og ændre ting i databasen så rollen er Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Create([Bind("Id,Name,Length,Width,Thickness,Volume,Price,Type")] Board board)
        {
            if (ModelState.IsValid)
            {
                _context.Add(board);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Boards/Edit/5
        // Husk og ændre ting i databasen så rollen er Admin
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Husk og ændre ting i databasen så rollen er Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Edit(int id, [Bind("BoardId,Name,Image,Length,Width,Thickness,Volume,Price,Type")] Board board)
        {
            if (id != board.BoardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(board);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardExists(board.BoardId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Boards/Delete/5
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.BoardId == id);
            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        // POST: Boards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Board == null)
            {
                return Problem("Entity set 'SurfsUpContext.Board'  is null.");
            }
            var board = await _context.Board.FindAsync(id);
            if (board != null)
            {
                _context.Board.Remove(board);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoardExists(int id)
        {
          return (_context.Board?.Any(e => e.BoardId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> CreateRental(int id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.BoardId == id);
            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmRental( Rental rental, int id, DateTime start, DateTime end)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier as string);
            rental.UsersId = claims.Value;
            rental.BoardId = id;
            rental.StartRental = start;
            rental.EndRental = end;

            if (ModelState.IsValid)
            {
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
