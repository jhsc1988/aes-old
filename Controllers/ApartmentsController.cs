using aes.CommonDependecies;
using aes.Controllers.IControllers;
using aes.Models;
using aes.Models.Racuni;
using aes.Services;
using aes.Services.BillsServices.BillsHoldingService.IService;
using aes.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class ApartmentsController : Controller, IApartmentsController
    {
        private readonly IBillsHoldingService _billsHoldingService;
        private readonly ICommonDependencies _c;
        private readonly IApartmentUploadService _apartmentUploadService;
        private readonly ILogger _logger;

        public ApartmentsController(IBillsHoldingService billsHoldingService,
            ICommonDependencies c, IApartmentUploadService apartmentUploadService, ILogger logger)
        {
            _c = c;
            _billsHoldingService = billsHoldingService;
            _apartmentUploadService = apartmentUploadService;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Apartments/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Stan stan = await _c.UnitOfWork.Apartment.FindExact(m => m.Id == id);
            return stan == null ? NotFound() : View(stan);
        }

        // GET: Apartments/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(
            [Bind(
                "Id,StanId,SifraObjekta,Vrsta,Adresa,Kat,BrojSTana,Naselje,Četvrt,Površina,StatusKorištenja,Korisnik,Vlasništvo,DioNekretnine,Sektor,Status,VrijemeUnosa")]
            Stan stan)
        {
            if (ModelState.IsValid)
            {
                stan.VrijemeUnosa = DateTime.Now;
                _c.UnitOfWork.Apartment.Add(stan);
                _ = await _c.UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }

            return View(stan);
        }

        // GET: Apartments/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Stan stan = await _c.UnitOfWork.Apartment.Get((int)id);
            return stan == null ? NotFound() : View(stan);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id,StanId,SifraObjekta,Vrsta,Adresa,Kat,BrojSTana,Naselje,Četvrt,Površina,StatusKorištenja,Korisnik,Vlasništvo,DioNekretnine,Sektor,Status,VrijemeUnosa")]
            Stan stan)
        {
            if (id != stan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = await _c.UnitOfWork.Apartment.Update(stan);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StanExists(stan.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(stan);
        }

        // GET: Apartments/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Stan stan = await _c.UnitOfWork.Apartment.FindExact(m => m.Id == id);
            return stan == null ? NotFound() : View(stan);
        }

        // POST: Apartments/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Stan stan = await _c.UnitOfWork.Apartment.Get(id);
            _c.UnitOfWork.Apartment.Remove(stan);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool StanExists(int id)
        {
            return _c.UnitOfWork.Apartment.Any(e => e.Id == id);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Upload

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            return await _apartmentUploadService.Upload(Request, User.Identity.Name);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Main Datatables

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList()
        {
            IEnumerable<Stan> list = await _c.UnitOfWork.Apartment.GetApartments();

            return await new DatatablesService<Stan>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetStanoviForDatatables);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Apartments wihout ods-omm (ODS Create page)

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetListFiltered()
        {
            IEnumerable<Stan> list = await _c.UnitOfWork.Apartment.GetApartmentsWithoutODSOmm();

            return await new DatatablesService<Stan>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetStanoviForDatatables);

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Details page - bills for apartment

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetBillsElektraForApartment(int param)
        {
            IEnumerable<RacunElektra> list = await _c.UnitOfWork.Ods.GetBillsForOmm<RacunElektra>(param);

            return await new DatatablesService<RacunElektra>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniElektraForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetBillsElektraAdvancesForApartment(int param)
        {
            IEnumerable<RacunElektraRate> list = await _c.UnitOfWork.Ods.GetBillsForOmm<RacunElektraRate>(param);

            return await new DatatablesService<RacunElektraRate>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniElektraRateForDatatables);

        }
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetBillsElektraServicesForApartment(int param)
        {
            IEnumerable<RacunElektraIzvrsenjeUsluge> list = await _c.UnitOfWork.Ods.GetBillsForOmm<RacunElektraIzvrsenjeUsluge>(param);

            return await new DatatablesService<RacunElektraIzvrsenjeUsluge>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacunElektraIzvrsenjeUslugeForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetBillsHoldingForApartment(int param)
        {
            IEnumerable<RacunHolding> list = await _c.UnitOfWork.BillsHolding.GetBillsForApartment(param);

            return await new DatatablesService<RacunHolding>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniHoldingForDatatables);
        }
    }
}