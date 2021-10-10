using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class StanoviController : Controller, IStanController
    {
        private readonly IDatatablesGenerator _datatablesGenerator;
        private readonly ApplicationDbContext _context;
        private readonly IStanoviWorkshop _stanoviWorkshop;
        private readonly IRacunWorkshop _racunWorkshop;
        private readonly IRacunElektraWorkshop _racunElektraWorkshop;
        private readonly IRacunElektraRateWorkshop _racunElektraRateWorkshop;
        private readonly IRacunElektraIzvrsenjeUslugeWorkshop _racunElektraIzvrsenjeUslugeWorkshop;
        private readonly IRacunHoldingWorkshop _racunHoldingWorkshop;
        private readonly IElektraKupacWorkshop _elektraKupacWorkshop;

        public StanoviController(ApplicationDbContext context, IDatatablesGenerator datatablesGenerator,
            IStanoviWorkshop stanoviWorkshop, IRacunWorkshop racunWorkshop, IRacunElektraWorkshop racunElektraWorkshop,
            IRacunElektraRateWorkshop racunElektraRateWorkshop, IRacunElektraIzvrsenjeUslugeWorkshop racunElektraIzvrsenjeUslugeWorkshop,
            IRacunHoldingWorkshop racunHoldingWorkshop, IElektraKupacWorkshop elektraKupacWorkshop)
        {
            _datatablesGenerator = datatablesGenerator;
            _stanoviWorkshop = stanoviWorkshop;
            _racunWorkshop = racunWorkshop;
            _racunElektraRateWorkshop = racunElektraRateWorkshop;
            _racunElektraWorkshop = racunElektraWorkshop;
            _racunElektraIzvrsenjeUslugeWorkshop = racunElektraIzvrsenjeUslugeWorkshop;
            _racunHoldingWorkshop = racunHoldingWorkshop;
            _elektraKupacWorkshop = elektraKupacWorkshop;
            _context = context;
        }

        // GET: Stanovi
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Stan.ToListAsync());
        //}
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Stanovi/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Stan stan = await _context.Stan
                .FirstOrDefaultAsync(m => m.Id == id);
            return stan == null ? NotFound() : View(stan);
        }

        // GET: Stanovi/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stanovi/Create
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
                _ = _context.Add(stan);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(stan);
        }

        // GET: Stanovi/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Stan stan = await _context.Stan.FindAsync(id);
            return stan == null ? NotFound() : View(stan);
        }

        // POST: Stanovi/Edit/5
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
                    _ = _context.Update(stan);
                    _ = await _context.SaveChangesAsync();
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

        // GET: Stanovi/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Stan stan = await _context.Stan
                .FirstOrDefaultAsync(m => m.Id == id);
            return stan == null ? NotFound() : View(stan);
        }

        // POST: Stanovi/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Stan stan = await _context.Stan.FindAsync(id);
            _ = _context.Stan.Remove(stan);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StanExists(int id)
        {
            return _context.Stan.Any(e => e.Id == id);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public JsonResult GetList()
        {
            return _stanoviWorkshop.GetList(false,_datatablesGenerator, Request, _context);
        }

        public JsonResult GetListFiltered()
        {
            return _stanoviWorkshop.GetList(true, _datatablesGenerator, Request, _context);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Details page

        public JsonResult GetRacuniForStan(int param)
            => _stanoviWorkshop.GetRacuniForStan(_racunWorkshop, _elektraKupacWorkshop, Request, _datatablesGenerator, _racunElektraWorkshop, _context, param);
        public JsonResult GetRacuniRateForStan(int param)
            => _stanoviWorkshop.GetRacuniRateForStan(_racunWorkshop,  _elektraKupacWorkshop, Request, _datatablesGenerator, _racunElektraRateWorkshop, _context, param);
        public JsonResult GetRacuniElektraIzvrsenjeForStan(int param)
            => _stanoviWorkshop.GetRacuniElektraIzvrsenjeForStan(_racunWorkshop, _elektraKupacWorkshop, Request, _datatablesGenerator, _racunElektraIzvrsenjeUslugeWorkshop, _context, param);
        public JsonResult GetHoldingRacuniForStan(int param)
            => _racunHoldingWorkshop.GetList(false, null, null, _datatablesGenerator, _context, Request, null, param);

    }
}