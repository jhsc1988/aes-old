using aes.CommonDependecies;
using aes.Models;
using aes.Models.Racuni;
using aes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class ElektraCustomersController : Controller
    {
        private readonly ICommonDependencies _c;

        public ElektraCustomersController(ICommonDependencies c)
        {
            _c = c;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: ElektraCustomers/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ElektraKupac elektraKupac = await _c.UnitOfWork.ElektraCustomer.FindExactById((int)id);

            return elektraKupac == null ? NotFound() : View(elektraKupac);
        }

        // GET: ElektraCustomers/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["OdsId"] = new SelectList(await _c.UnitOfWork.Ods.GetAll(), "Id", "Id");
            return View();
        }

        // POST: ElektraCustomers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,UgovorniRacun,OdsId,Napomena,VrijemeUnosa")] ElektraKupac elektraKupac)
        {
            if (ModelState.IsValid)
            {
                elektraKupac.VrijemeUnosa = DateTime.Now;
                _c.UnitOfWork.ElektraCustomer.Add(elektraKupac);
                _ = await _c.UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OdsId"] = new SelectList(await _c.UnitOfWork.Ods.GetAll(), "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // GET: ElektraCustomers/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ElektraKupac elektraKupac = await _c.UnitOfWork.ElektraCustomer.FindExactById((int)id);

            if (elektraKupac == null)
            {
                return NotFound();
            }
            ViewData["OdsId"] = new SelectList(await _c.UnitOfWork.Ods.GetAll(), "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // POST: ElektraCustomers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UgovorniRacun,OdsId,Napomena,VrijemeUnosa")] ElektraKupac elektraKupac)
        {
            if (id != elektraKupac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = await _c.UnitOfWork.ElektraCustomer.Update(elektraKupac);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElektraKupacExists(elektraKupac.Id))
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
            ViewData["OdsId"] = new SelectList(await _c.UnitOfWork.Ods.GetAll(), "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // GET: ElektraCustomers/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ElektraKupac elektraKupac = await _c.UnitOfWork.ElektraCustomer.FindExactById((int)id);

            return elektraKupac == null ? NotFound() : View(elektraKupac);
        }

        // POST: ElektraCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ElektraKupac elektraKupac = await _c.UnitOfWork.ElektraCustomer.Get(id);
            _c.UnitOfWork.ElektraCustomer.Remove(elektraKupac);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool ElektraKupacExists(int id)
        {
            return _c.UnitOfWork.ElektraCustomer.Any(e => e.Id == id);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // validation

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UgovorniRacunValidation(long ugovorniRacun)
        {
            if (ugovorniRacun is < 1000000000 or > 9999999999)
            {
                return Json($"Ugovorni račun nije ispravan");
            }

            ElektraKupac db = await _c.UnitOfWork.ElektraCustomer.FindExact(ugovorniRacun);
            return db != null ? Json($"Ugovorni račun {ugovorniRacun} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetList()
        {
            IEnumerable<ElektraKupac> list = await _c.UnitOfWork.ElektraCustomer.GetAllCustomers();

            return await new DatatablesService<ElektraKupac>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetElektraCustomersForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetBillsElektraForElektraCustomer(int param)
        {
            IEnumerable<RacunElektra> list = await _c.UnitOfWork.BillsElektra.GetRacuniForCustomer(param);

            return await new DatatablesService<RacunElektra>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniElektraForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetBillsElektraAdvancesForElektraCustomer(int param)
        {
            IEnumerable<RacunElektraRate> list = await _c.UnitOfWork.BillsElektraAdvances.GetRacuniForCustomer(param);

            return await new DatatablesService<RacunElektraRate>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniElektraRateForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetBillsElektraServicesForElektraCustomer(int param)
        {
            IEnumerable<RacunElektraIzvrsenjeUsluge> list = await _c.UnitOfWork.BillsElektraServices.GetRacuniForCustomer(param);

            return await new DatatablesService<RacunElektraIzvrsenjeUsluge>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacunElektraIzvrsenjeUslugeForDatatables);
        }
    }
}
