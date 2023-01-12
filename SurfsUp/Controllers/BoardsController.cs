using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Models;
using SurfsUp.Data;
using SurfsUpLibrary.Models;
using SurfsUpLibrary.Models.Repositories;
using System.Security.Claims;

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
        public async Task<IActionResult> Details(int id)
        {
            // Hent specifik board fra API
            Board board = await BoardRepo.GetFromAPI(id);
           
            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        // GET: Boards/Create
        // Husk og ændre ting i databasen så rollen er Admin
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Create()
        {
            //Hent alle Equipment til checkboxes
            List<Equipment> BoardEquipment = await EquipmentRepo.Retrieve();
          
            BoardViewModel bvm = new()
            {
                EquipmentVM = new List<EquipmentViewModel>()
            };

            foreach (Equipment equipment in BoardEquipment)
            {
                EquipmentViewModel evm = new()
                {
                    Id = equipment.Id,
                    Name = equipment.Name,
                    Checked = false
                };
                bvm.EquipmentVM.Add(evm);
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

            List<Equipment> EquipmentAll = await EquipmentRepo.Retrieve();
            foreach (Equipment equipment in EquipmentAll)
            {
                foreach (EquipmentViewModel equipmentViewModel in bvm.EquipmentVM)
                {
                    if (equipment.Id == equipmentViewModel.Id && equipmentViewModel.Checked)
                        board.Equipment.Add(equipment);
                }
            }

            if (ModelState.IsValid)
            {
                await BoardRepo.PostToAPI(board);
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(bvm);
        }

        // GET: Boards/Edit/5
        // Husk og ændre ting i databasen så rollen er Admin
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Edit(int id)
        {
            if (Lock(id))
                return RedirectToAction(nameof(Index), new { Error = "Der er en som allerede er ved at ændre dette board." });

            Board boardWithId;
            try 
            { 
                boardWithId = await BoardRepo.GetFromAPI(id); 
            }
            catch (Exception)
            { 
                return NotFound(); 
            }

            List<Equipment> equipmentAll;
            try
            {
                equipmentAll = await EquipmentRepo.Retrieve();
            }
            catch (Exception)
            {
                return NotFound();
            }

            BoardViewModel bvm = new()
            {
                Id = boardWithId.Id,
                Name = boardWithId.Name,
                Image = boardWithId.Image,
                Length = boardWithId.Length,
                Width = boardWithId.Width,
                Thickness = boardWithId.Thickness,
                Price = boardWithId.Price,
                Type = boardWithId.Type,
                EquipmentVM = new List<EquipmentViewModel>()
            };

            foreach (Equipment equipment in equipmentAll)
            {
                EquipmentViewModel evm = new()
                {
                    Id = equipment.Id,
                    Name = equipment.Name,
                    Checked = false
                };
                foreach (Equipment eq in boardWithId.Equipment)
                {
                    if (evm.Id == eq.Id)
                    {
                        evm.Checked = true;
                    }
                }
                bvm.EquipmentVM.Add(evm);
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
            if (id != bvm.Id)
            {
                return NotFound();
            }
            Board boardToEdit;
            try
            {
                boardToEdit = await BoardRepo.GetFromAPI(id);
            }
            catch (Exception)
            {
                return NotFound();
            }

            boardToEdit.Equipment.Clear();

            List<Equipment> equipmentAll;
            try
            {
                equipmentAll = await EquipmentRepo.Retrieve();
            }
            catch (Exception)
            {
                return NotFound();
            }

            foreach (Equipment equipment in equipmentAll)
            {
                foreach (EquipmentViewModel equipmentViewModel in bvm.EquipmentVM)
                {
                    if (equipment.Id == equipmentViewModel.Id && equipmentViewModel.Checked)
                        boardToEdit.Equipment.Add(equipment);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await BoardRepo.PutToAPI(boardToEdit);
                }
                catch (Exception)
                {
                    return NotFound();
                }

                Unlock(id);

                return RedirectToAction(nameof(Index));
            }

            return View(boardToEdit);
        }

        // GET: Boards/Delete/5
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Delete(int id)
        {
            if (Lock(id))
                return RedirectToAction(nameof(Index), new { Error = "Der er en som allerede er ved at ændre dette board." });

            Board boardToEdit;
            try
            {
                boardToEdit = await BoardRepo.GetFromAPI(id);
            }
            catch (Exception)
            {
                return NotFound();
            }

            if(boardToEdit == null)
            { return NotFound(); }

            return View(boardToEdit);
        }

        // POST: Boards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
               await BoardRepo.DeleteToAPI(id);
            }
            catch (Exception)
            {
                return NotFound();
            }

            Unlock(id);

            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> CreateRental(int id)
        {
            if (Lock(id))
                return RedirectToAction(nameof(Index), new { Error = "Der er en som allerede er ved at leje dette board." });

            Board board;
            try
            {
               board = await BoardRepo.GetFromAPI(id);
            }
            catch (Exception)
            {
                return NotFound();
            }
        
            Rental rental = new()
            {
                Board = board,
                BoardId = board.Id,
                StartRental = DateTime.Now,
                EndRental = DateTime.Now
            };

            return View(rental);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRental(Rental rental, int id)
        {
            //Nyt start
            string guestid = "1";
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            if (claimsIdentity.FindFirst(ClaimTypes.NameIdentifier as string) == null)
            {
                rental.UsersId = guestid;
            }
            else
            {
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier as string);
                rental.UsersId = claims.Value;
            }
            rental.BoardId = id;
            rental.Id = 0;
            ViewData["SelectedBoardId"] = rental.StartRental;

            try
            {
                rental.Board = await BoardRepo.GetFromAPI(id);
            }
            catch (Exception)
            { 
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await RentalRepo.PostToAPI(rental);
                }
                catch (Exception)
                {
                    return NotFound("Rental could not be Posted");
                }
                
                Unlock(id);

                return RedirectToAction(nameof(Index));
            }

            return View(rental);
        }
    }
}