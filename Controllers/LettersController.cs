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
    public class LettersController : Controller, ILettersController
    {
        private readonly ILetterService _letterService;
        private readonly ICommonDependencies _c;

        public LettersController(ILetterService letterService, ICommonDependencies c)
        {
            _letterService = letterService;
            _c = c;
        }

        [Authorize]
        public IActionResult Index()
        {
            return Redirect("/CaseFiles");
            //return View("Index", "CaseFiles");
        }

        // GET: Letters/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dopis dopis = await _c.UnitOfWork.Letter.IncludeCaseFile(await _c.UnitOfWork.Letter.Get((int)id));
            return dopis == null ? NotFound() : View(dopis);
        }

        // GET: Letters/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Letters/Create
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
                _c.UnitOfWork.Letter.Add(dopis);
                _ = await _c.UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }

            return View(dopis);
        }

        // GET: Letters/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dopis dopis = await _c.UnitOfWork.Letter.IncludeCaseFile(await _c.UnitOfWork.Letter.Get((int)id));
            if (dopis == null)
            {
                return NotFound();
            }
            ViewData["PredmetId"] = new SelectList(await _c.UnitOfWork.CaseFile.GetAll(), "Id", "Klasa", dopis.PredmetId);
            return View(dopis);
        }

        // POST: Letters/Edit/5
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
                    dopis = await _c.UnitOfWork.Letter.IncludeCaseFile(dopis);
                    _ = await _c.UnitOfWork.Letter.Update(dopis);
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

        // GET: Letters/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dopis dopis = await _c.UnitOfWork.Letter.IncludeCaseFile(await _c.UnitOfWork.Letter.Get((int)id));
            return dopis == null ? NotFound() : View(dopis);
        }

        // POST: Letters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Dopis dopis = await _c.UnitOfWork.Letter.Get(id);
            _c.UnitOfWork.Letter.Remove(dopis);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool DopisExists(int id)
        {
            return _c.UnitOfWork.Letter.Any(e => e.Id == id);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList(int predmetId)
        {
            IEnumerable<Dopis> list = await _c.UnitOfWork.Letter.GetLettersForCaseFile(predmetId);

            return await new DatatablesService<Dopis>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetLettersForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveToDB(string predmetId, string urbroj, string datumDopisa)
        {
            return await _letterService.SaveToDB(predmetId, urbroj, datumDopisa);

        }
    }
}
