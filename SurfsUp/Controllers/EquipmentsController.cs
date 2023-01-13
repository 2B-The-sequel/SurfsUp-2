using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurfsUp.Data;
using SurfsUpLibrary.Models;
using SurfsUpLibrary.Models.Repositories;

namespace SurfsUp.Controllers
{
    public class EquipmentsController : LockableController
    {
        private readonly ApplicationDbContext _context;

        public EquipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Equipments
        public async Task<IActionResult> Index(int? unlock)
        {
            if (unlock != null)
            {
                Unlock(unlock);
            }

            return View(await EquipmentRepo.Retrieve());
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                Equipment equipment = await EquipmentRepo.Retrieve(id);
                return View(equipment);
            }
            catch (Exception)
            {
                return NotFound();
            }
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
        public async Task<IActionResult> Create(Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                await EquipmentRepo.Create(equipment);

                return RedirectToAction(nameof(Index));
            }

            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (Lock(id))
                return RedirectToAction(nameof(Index), new { Error = "Der er en som allerede er ved at ændre dette udstyr." });

            try
            {
                Equipment equipment = await EquipmentRepo.Retrieve(id);
                return View(equipment);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await EquipmentRepo.Update(equipment);

                    Unlock(id);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }

            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (Lock(id))
                return RedirectToAction(nameof(Index), new { Error = "Der er en som allerede er ved at ændre dette udstyr." });

            try
            {
                Equipment equipment = await EquipmentRepo.Retrieve(id);
                return View(equipment);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Equipment equipment = await EquipmentRepo.Retrieve(id);

            try
            {
                await EquipmentRepo.Delete(equipment);
                Unlock(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}