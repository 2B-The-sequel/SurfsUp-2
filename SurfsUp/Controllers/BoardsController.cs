using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;
using System.Security.Claims;

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

            Board = sortOrder switch
            {
                "name_desc" => Board.OrderByDescending(s => s.Name),
                "Price" => Board.OrderBy(s => s.Price),
                "price_desc" => Board.OrderByDescending(s => s.Price),
                "Type" => Board.OrderBy(s => s.Type),
                "type_desc" => Board.OrderByDescending(s => s.Type),
                _ => Board.OrderBy(s => s.Name),
            };
            int pageSize = 5;
            return View(await PaginatedList<Board>.CreateAsync(Board
                            .Include(e => e.BoardEquipments)
                            .ThenInclude(be => be.Equipment)
                            .Include(r => r.rentals)
                            .AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        // GET: Boards/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .Include(e => e.BoardEquipments)
                .ThenInclude(be => be.Equipment)
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
            List<Equipment> BoardEquipment = (from s in _context.Equipment select s).ToList();
            BoardViewModel bvm = new()
            {
                Equipment = new List<EquipmentViewModel>()
            };

            foreach (Equipment equipment in BoardEquipment)
            {
                EquipmentViewModel evm = new()
                {
                    Id = equipment.EquipmentId,
                    Name = equipment.Name,
                    Checked = false
                };
                bvm.Equipment.Add(evm);
            }

            return View(bvm);
        }

        // POST: Boards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Husk og ændre ting i databasen så rollen er Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Create(BoardViewModel bvm)
        {
            Board board = new()
            {
                Name = bvm.Name,
                Image = bvm.Image,
                Length = bvm.Length,
                Width = bvm.Width,
                Thickness = bvm.Thickness,
                Price = bvm.Price,
                Type = bvm.Type
            };

            List<Equipment> DatabaseEquipment = (from s in _context.Equipment select s).ToList();
            foreach (Equipment equipment in DatabaseEquipment)
            {
                foreach (EquipmentViewModel equipmentViewModel in bvm.Equipment)
                {
                    if (equipment.EquipmentId == equipmentViewModel.Id && equipmentViewModel.Checked)
                        board.Equipment.Add(equipment);
                }
            }

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

            Board board = await _context.Board
                .Include(e => e.BoardEquipments)
                .ThenInclude(be => be.Equipment)
                .FirstOrDefaultAsync(m => m.BoardId == id);

            if (board == null)
            {
                return NotFound();
            }

            List<Equipment> BoardEquipment = (from s in _context.Equipment select s).ToList();
            BoardViewModel bvm = new()
            {
                BoardId = board.BoardId,
                Name = board.Name,
                Image = board.Image,
                Length = board.Length,
                Width = board.Width,
                Thickness = board.Thickness,
                Price = board.Price,
                Type = board.Type,
                Equipment = new List<EquipmentViewModel>()
            };

            foreach (Equipment equipment in BoardEquipment)
            {
                EquipmentViewModel evm = new()
                {
                    Id = equipment.EquipmentId,
                    Name = equipment.Name,
                    Checked = false
                };
                foreach (Equipment eq in board.Equipment)
                {
                    if (evm.Id == eq.EquipmentId)
                    {
                        evm.Checked = true;
                    }
                }
                bvm.Equipment.Add(evm);
            }
            return View(bvm);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Husk og ændre ting i databasen så rollen er Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Edit(int id, BoardViewModel bvm)
        {
            if (id != bvm.BoardId)
            {
                return NotFound();
            }

            Board board = await _context.Board
                .Include(e => e.BoardEquipments)
                .ThenInclude(be => be.Equipment)
                .FirstOrDefaultAsync(m => m.BoardId == id);

            List<Equipment> DatabaseEquipment = (from s in _context.Equipment select s).ToList();
            board.Equipment.Clear();
            foreach (Equipment equipment in DatabaseEquipment)
            {
                foreach (EquipmentViewModel equipmentViewModel in bvm.Equipment)
                {
                    if (equipment.EquipmentId == equipmentViewModel.Id && equipmentViewModel.Checked)
                        board.Equipment.Add(equipment);
                }
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

        [Authorize]
        public async Task<IActionResult> CreateRental(int id)
        {
            if (_context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.BoardId == id);
            if (board == null)
            {
                return NotFound();
            }
            Rental rental = new Rental();
            rental.Board = board;
            rental.BoardId = board.BoardId;
            rental.StartRental = DateTime.Now;
            rental.EndRental = DateTime.Now;

            return View(rental);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateRental( Rental rental, int id)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            rental.UsersId = claims.Value;
            rental.BoardId = id;
            ViewData["SelectedBoardId"] = rental.StartRental;
            rental.Board = await _context.Board
                .FirstOrDefaultAsync(m => m.BoardId == id);
            rental.User = (ApplicationUser)await _context.Users
                .FirstOrDefaultAsync(m => m.Id == rental.UsersId);

            if (ModelState.IsValid)
            {
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                .Where(y => y.Count > 0)
                .ToList();
            }
            return View(rental);
        }
    }
}