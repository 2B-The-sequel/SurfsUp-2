using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;
using SurfsUp.Models.Repositories;
using System.Security.Claims;
using System.Text.Json;

namespace SurfsUp.Controllers
{
    public class BoardsController : LockableController
    {
        private readonly ApplicationDbContext _context;

        public BoardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Boards
        public async Task<IActionResult> Index(string sortOrder,string searchString, string currentFilter, int? pageNumber, int? unlock)
        {
            if (unlock != null)
                Unlock(unlock);

            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<Board> boards = await BoardRepo.GetAllFromAPI();

            // Få fat i sorteringsparametre
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "type_desc" : "Type";
            ViewData["CurrentFilter"] = searchString;

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
                { boards = boards.Where(s => s.Type == Found || s.Name.ToLower().Contains(searchString)).ToList(); }
                else if (!hasChanged && Found == BoardType.Shortboard)
                {
                    boards = boards.Where(s => s.Name.ToLower().Contains(searchString)).ToList(); 
                }
                else
                {
                    boards = boards.Where(s => s.Type == Found || s.Name.ToLower().Contains(searchString)).ToList();
                }
            }

            boards = sortOrder switch
            {
                "name_desc" => boards.OrderByDescending(s => s.Name).ToList(),
                "Price" => boards.OrderBy(s => s.Price).ToList(),
                "price_desc" => boards.OrderByDescending(s => s.Price).ToList(),
                "Type" => boards.OrderBy(s => s.Type).ToList(),
                "type_desc" => boards.OrderByDescending(s => s.Type).ToList(),
                _ => boards.OrderBy(s => s.Name).ToList(),
            };
            int pageSize = 5;
            return View(PaginatedList<Board>.Create(boards, pageNumber ?? 1, pageSize));
        }

        // GET: Boards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // LAV FRA DATABASE TIL API
            /*
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .Include(e => e.BoardEquipments)
                .ThenInclude(be => be.Equipment)
                .FirstOrDefaultAsync(m => m.BoardId == id);
            */
            Board board = null;
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
            // LAV FRA DATABASE TIL API
            /*List<Equipment> BoardEquipment = (from s in _context.Equipment select s).ToList();
            BoardViewModel bvm = new()
            {
                EquipmentVM = new List<EquipmentViewModel>()
            };

            foreach (Equipment equipment in BoardEquipment)
            {
                EquipmentViewModel evm = new()
                {
                    Id = equipment.EquipmentId,
                    Name = equipment.Name,
                    Checked = false
                };
                bvm.EquipmentVM.Add(evm);
            }*/

            BoardViewModel bvm = null;
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
            /*
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
                foreach (EquipmentViewModel equipmentViewModel in bvm.EquipmentVM)
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
            */
            
            return View(bvm);
        }

        // GET: Boards/Edit/5
        // Husk og ændre ting i databasen så rollen er Admin
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Edit(int id)
        {
            if (Lock(id))
                return RedirectToAction(nameof(Index), new { Error = "Der er en som allerede er ved at ændre dette board." });

            /*
            if (_context.Board == null)
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
                EquipmentVM = new List<EquipmentViewModel>()
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
                bvm.EquipmentVM.Add(evm);
            }
            */
            BoardViewModel bvm = null;

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
            if (id != bvm.Id)
            {
                return NotFound();
            }

            /*
            Board board = await _context.Board
                .Include(e => e.BoardEquipments)
                .ThenInclude(be => be.Equipment)
                .FirstOrDefaultAsync(m => m.BoardId == id);

            List<Equipment> DatabaseEquipment = (from s in _context.Equipment select s).ToList();
            board.Equipment.Clear();
            foreach (Equipment equipment in DatabaseEquipment)
            {
                foreach (EquipmentViewModel equipmentViewModel in bvm.EquipmentVM)
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

                Unlock(id);

                return RedirectToAction(nameof(Index));
            }
            */
            Board board = null;

            return View(board);
        }

        // GET: Boards/Delete/5
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Delete(int id)
        {
            if (Lock(id))
                return RedirectToAction(nameof(Index), new { Error = "Der er en som allerede er ved at ændre dette board." });

            /*
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
            */
            Board board = null;

            return View(board);
        }

        // POST: Boards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /*
            if (_context.Board == null)
            {
                return Problem("Entity set 'SurfsUpContext.Board'  is null.");
            }
            var board = await _context.Board.FindAsync(id);
            if (board != null)
            {
                _context.Board.Remove(board);
            }
            */

            Unlock(id);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoardExists(int id)
        {
            //return (_context.Board?.Any(e => e.BoardId == id)).GetValueOrDefault();
            return false;
        }

        [Authorize]
        public async Task<IActionResult> CreateRental(int id)
        {
            if (Lock(id))
                return RedirectToAction(nameof(Index), new { Error = "Der er en som allerede er ved at leje dette board." });

            /*
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
            Rental rental = new()
            {
                Board = board,
                BoardId = board.BoardId,
                StartRental = DateTime.Now,
                EndRental = DateTime.Now
            };
            */
            Rental rental = null;

            return View(rental);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateRental( Rental rental, int id)
        {
            /*
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            rental.UsersId = claims.Value;
            rental.BoardId = id;
            ViewData["SelectedBoardId"] = rental.StartRental;
            rental.Board = await _context.Board
                .FirstOrDefaultAsync(m => m.BoardId == id);
            rental.User = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == rental.UsersId);

            if (ModelState.IsValid)
            {
                _context.Add(rental);

                Unlock(id);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                .Where(y => y.Count > 0)
                .ToList();
            }
            */
            
            return View(rental);
        }
         
    }
}