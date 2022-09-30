using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUp.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly static List<Lock> locks = new();
        private readonly static object locksLock = new();

        public EquipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Equipments
        public async Task<IActionResult> Index(int? delock)
        {
            if (delock != null)
            {
                lock (locksLock)
                {
                    int i = 0;
                    bool found = false;
                    while (i < locks.Count && !found)
                    {
                        if (locks[i].Id == delock)
                        {
                            found = true;
                            locks.Remove(locks[i]);
                        }
                        else
                            i++;
                    }
                }
            }

            return _context.Equipment != null ? 
                          View(await _context.Equipment.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Equipment'  is null.");
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Equipment == null)
            {
                return NotFound();
            }

            Equipment equipment = await _context.Equipment.FirstOrDefaultAsync(m => m.EquipmentId == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipments/Create

        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Husk og ændre ting i databasen så rollen er Admin
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Create([Bind("Id,Name")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Edit(int id)
        {
            lock (locksLock)
            {
                int i = 0;
                bool found = false;

                while (i < locks.Count && !found)
                {
                    if (locks[i].Id == id)
                    {
                        if ((DateTime.Now - locks[i].Time).TotalSeconds >= 60 * 5)
                            locks.Remove(locks[i]);
                        else
                            found = true;
                    }
                    else
                        i++;
                }

                if (found)
                    return RedirectToAction(nameof(Index), new { Error = "Der er en som allerede er ved at ændre dette udstyr." });
                else
                    locks.Add(new Lock(id, DateTime.Now));
            }

            if (_context.Equipment == null)
            {
                return NotFound();
            }

            Equipment equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Edit(int id, [Bind("EquipmentId,Name")] Equipment equipment)
        {
            if (id != equipment.EquipmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.EquipmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                lock (locksLock)
                {
                    int i = 0;
                    bool found = false;
                    while (i < locks.Count && !found)
                    {
                        if (locks[i].Id == id)
                        {
                            found = true;
                            locks.Remove(locks[i]);
                        }
                        else
                            i++;
                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> Delete(int id)
        {
            lock (locksLock)
            {
                int i = 0;
                bool found = false;

                while (i < locks.Count && !found)
                {
                    if (locks[i].Id == id)
                    {
                        if ((DateTime.Now - locks[i].Time).TotalSeconds >= 60 * 5)
                            locks.Remove(locks[i]);
                        else
                            found = true;
                    }
                    else
                        i++;
                }

                if (found)
                    return RedirectToAction(nameof(Index), new { Error = "Der er en som allerede er ved at ændre dette udstyr." });
                else
                    locks.Add(new Lock(id, DateTime.Now));
            }

            if (_context.Equipment == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(m => m.EquipmentId == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adminstrators")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Equipment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Equipment'  is null.");
            }
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment != null)
            {
                _context.Equipment.Remove(equipment);
            }

            lock (locksLock)
            {
                int i = 0;
                bool found = false;
                while (i < locks.Count && !found)
                {
                    if (locks[i].Id == id)
                    {
                        found = true;
                        locks.Remove(locks[i]);
                    }
                    else
                        i++;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
          return (_context.Equipment?.Any(e => e.EquipmentId == id)).GetValueOrDefault();
        }
    }
}
