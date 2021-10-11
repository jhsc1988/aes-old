using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class RacuniElektraRateController : Controller, IRacunController
    {
        private readonly IDatatablesGenerator _datatablesGenerator;
        private readonly IRacunWorkshop _racunWorkshop;
        private readonly IPredmetWorkshop _predmetWorkshop;
        private readonly IRacunElektraRateWorkshop _racunElektraRateWorkshop;
        private readonly ApplicationDbContext _context;

        public RacuniElektraRateController(ApplicationDbContext context, IDatatablesGenerator datatablesGenerator,
            IRacunWorkshop racunWorkshop, IRacunElektraRateWorkshop racunElektraRateWorkshop, IPredmetWorkshop predmetWorkshop)
        {
            _context = context;
            _racunWorkshop = racunWorkshop;
            _racunElektraRateWorkshop = racunElektraRateWorkshop;
            _predmetWorkshop = predmetWorkshop;
            _datatablesGenerator = datatablesGenerator;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: RacuniElektraRate/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraRate racunElektraRate = await _context.RacunElektraRate
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            return racunElektraRate == null ? NotFound() : View(racunElektraRate);
        }

        // GET: RacuniElektraRate/Create
        // GET: RacuniElektra/Create
        [Authorize]
        public async Task<IActionResult> CreateAsync()
        {
            List<RacunElektraRate> applicationDbContext = await _context.RacunElektraRate.ToListAsync();

            return View(applicationDbContext);
        }

        // POST: RacuniElektraRate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,ElektraKupacId,Razdoblje,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunElektraRate racunElektraRate)
        {
            if (ModelState.IsValid)
            {
                racunElektraRate.VrijemeUnosa = DateTime.Now;
                _ = _context.Add(racunElektraRate);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(racunElektraRate);
        }

        // GET: RacuniElektraRate/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraRate racunElektraRate = await _context.RacunElektraRate.FindAsync(id);
            if (racunElektraRate == null)
            {
                return NotFound();
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektraRate.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektraRate.ElektraKupacId);
            return View(racunElektraRate);
        }

        // POST: RacuniElektraRate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,ElektraKupacId,Razdoblje,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunElektraRate racunElektraRate)
        {
            if (id != racunElektraRate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = _context.Update(racunElektraRate);
                    _ = await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunElektraRateExists(racunElektraRate.Id))
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
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektraRate.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektraRate.ElektraKupacId);
            return View(racunElektraRate);
        }

        // GET: RacuniElektraRate/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraRate racunElektraRate = await _context.RacunElektraRate
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            return racunElektraRate == null ? NotFound() : View(racunElektraRate);
        }

        // POST: RacuniElektraRate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunElektraRate racunElektraRate = await _context.RacunElektraRate.FindAsync(id);
            _ = _context.RacunElektraRate.Remove(racunElektraRate);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraRateExists(int id)
        {
            return _context.RacunElektraRate.Any(e => e.Id == id);
        }

        // validation
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {
            if (brojRacuna.Length is < 19 or > 19)
            {
                return Json($"Broj računa nije ispravan");
            }

            RacunElektraRate db = await _context.RacunElektraRate.FirstOrDefaultAsync(x => x.BrojRacuna.Equals(brojRacuna));
            return db != null ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string GetUid() => _racunWorkshop.GetUid(User);
        public JsonResult GetDopisiDataForFilter(int predmetId) 
            => Json(_context.Dopis.Where(element => element.PredmetId == predmetId).ToList());
        public JsonResult GetPredmetiCreate() => Json(_context.Predmet.ToList());
        public string GetKupci() => JsonConvert.SerializeObject(_context.ElektraKupac.ToList());
        public JsonResult UpdateDbForInline(string id, string updatedColumn, string x) 
            => _racunWorkshop.UpdateDbForInline(id, updatedColumn, x, _context.RacunElektraRate, _context);
        public JsonResult SaveToDB(string _dopisId) => _racunWorkshop.SaveToDb(GetUid(), _dopisId, _context.RacunElektraRate, _context);
        public JsonResult RemoveRow(string racunId) => _racunWorkshop.RemoveRow(racunId, _context.RacunElektraRate, _context);
        public JsonResult RemoveAllFromDb() => _racunWorkshop.RemoveAllFromDbTemp(GetUid(), _context.RacunElektraRate, _context);
        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId) 
            => new JsonResult(_racunElektraRateWorkshop.AddNewTemp(brojRacuna, iznos, date, dopisId, GetUid(), _context));
        public JsonResult GetPredmetiDataForFilter() => Json(_predmetWorkshop.GetPredmetiDataForFilter(_context.RacunElektraRate, _context));
        public JsonResult GetList(bool IsFiltered, string klasa, string urbroj) 
            => _racunElektraRateWorkshop.GetList(IsFiltered, klasa, urbroj, _datatablesGenerator, _context, Request, GetUid());
    }
}
