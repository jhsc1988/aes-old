using aes.CommonDependecies.ICommonDependencies;
using aes.Controllers.IControllers;
using aes.Models;
using aes.Services;
using aes.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aes.Controllers
{
    public class PredmetiController : Controller, IPredmetiController
    {
        private readonly ICommonDependencies _c;
        private readonly IPredmetiervice _predmetiService;

        public PredmetiController(IPredmetiervice predmetiService, ICommonDependencies c)
        {
            _predmetiService = predmetiService;
            _c = c;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Predmeti/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Predmet predmet = await _c.UnitOfWork.Predmet.Get((int)id);
            return predmet == null ? NotFound() : View(predmet);
        }

        // GET: Predmeti/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Predmeti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Klasa,Naziv,VrijemeUnosa")] Predmet predmet)
        {
            if (ModelState.IsValid)
            {
                predmet.VrijemeUnosa = DateTime.Now;
                await _c.UnitOfWork.Predmet.Add(predmet);
                _ = await _c.UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(predmet);
        }

        // GET: Predmeti/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Predmet predmet = await _c.UnitOfWork.Predmet.Get((int)id);
            return predmet == null ? NotFound() : View(predmet);
        }

        // POST: Predmeti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Klasa,Naziv,VrijemeUnosa,Archived")] Predmet predmet)
        {
            if (id != predmet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = await _c.UnitOfWork.Predmet.Update(predmet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PredmetExists(predmet.Id))
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
            return View(predmet);
        }

        // GET: Predmeti/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Predmet predmet = await _c.UnitOfWork.Predmet.Get((int)id);
            return predmet == null ? NotFound() : View(predmet);
        }

        // POST: Predmeti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Predmet predmet = await _c.UnitOfWork.Predmet.Get(id);
            _c.UnitOfWork.Predmet.Remove(predmet);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PredmetExists(int id)
        {
            return await _c.UnitOfWork.Predmet.Any(e => e.Id == id);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveToDB(string klasa, string naziv)
        {
            return await _predmetiService.SaveToDB(klasa, naziv);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList()
        {
            IEnumerable<Predmet> list = await _c.UnitOfWork.Predmet.GetAll();

            return new DatatablesService<Predmet>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetPredmetiForDatatables);
        }
    }
}
