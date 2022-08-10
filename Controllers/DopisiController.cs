using aes.CommonDependecies;
using aes.Controllers.IControllers;
using aes.Models;
using aes.Services;
using aes.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class DopisiController : Controller, IDopisiController
    {
        private readonly IDopisiervice _Dopisiervice;
        private readonly ICommonDependencies _c;

        public DopisiController(IDopisiervice Dopisiervice, ICommonDependencies c)
        {
            _Dopisiervice = Dopisiervice;
            _c = c;
        }

        [Authorize]
        public IActionResult Index()
        {
            return Redirect("/Predmeti");
            //return View("Index", "Predmeti");
        }

        // GET: Dopisi/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dopis dopis = await _c.UnitOfWork.Dopis.IncludePredmet(await _c.UnitOfWork.Dopis.Get((int)id));
            return dopis == null ? NotFound() : View(dopis);
        }

        // GET: Dopisi/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dopisi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,PredmetId,Urbroj,Datum,VrijemeUnosa")] Dopis dopis)
        {
            if (ModelState.IsValid)
            {
                dopis.VrijemeUnosa = DateTime.Now;
                _c.UnitOfWork.Dopis.Add(dopis);
                _ = await _c.UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }

            return View(dopis);
        }

        // GET: Dopisi/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dopis dopis = await _c.UnitOfWork.Dopis.IncludePredmet(await _c.UnitOfWork.Dopis.Get((int)id));
            if (dopis == null)
            {
                return NotFound();
            }
            ViewData["PredmetId"] = new SelectList(await _c.UnitOfWork.Predmet.GetAll(), "Id", "Klasa", dopis.PredmetId);
            return View(dopis);
        }

        // POST: Dopisi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PredmetId,Urbroj,Datum,VrijemeUnosa")] Dopis dopis)
        {
            if (id != dopis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dopis = await _c.UnitOfWork.Dopis.IncludePredmet(dopis);
                    _ = await _c.UnitOfWork.Dopis.Update(dopis);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DopisExists(dopis.Id))
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
            return View(dopis);
        }

        // GET: Dopisi/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dopis dopis = await _c.UnitOfWork.Dopis.IncludePredmet(await _c.UnitOfWork.Dopis.Get((int)id));
            return dopis == null ? NotFound() : View(dopis);
        }

        // POST: Dopisi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Dopis dopis = await _c.UnitOfWork.Dopis.Get(id);
            _c.UnitOfWork.Dopis.Remove(dopis);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool DopisExists(int id)
        {
            return _c.UnitOfWork.Dopis.Any(e => e.Id == id);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList(int predmetId)
        {
            IEnumerable<Dopis> list = await _c.UnitOfWork.Dopis.GetDopisiForPredmet(predmetId);

            return new DatatablesService<Dopis>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetDopisiForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveToDB(string predmetId, string urbroj, string datumDopisa)
        {
            return await _Dopisiervice.SaveToDB(predmetId, urbroj, datumDopisa);

        }
    }
}
