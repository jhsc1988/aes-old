﻿using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class RacunElektraIzvrsenjaUslugesController : Controller, IRacunController
    {
        private readonly IDatatablesGenerator _datatablesGenerator;
        private readonly IPredmetWorkshop _predmetWorkshop;
        private readonly IRacunElektraIzvrsenjeUslugeWorkshop _racunElektraIzvrsenjeUslugeWorkshop;
        private readonly ApplicationDbContext _context;

        public RacunElektraIzvrsenjaUslugesController(ApplicationDbContext context, IDatatablesGenerator datatablesParamsGeneratorcs,
            IRacunElektraIzvrsenjeUslugeWorkshop racunElektraIzvrsenjeUslugeWorkshop, IPredmetWorkshop predmetWorkshop)
        {
            _context = context;
            _racunElektraIzvrsenjeUslugeWorkshop = racunElektraIzvrsenjeUslugeWorkshop;
            _datatablesGenerator = datatablesParamsGeneratorcs;
            _predmetWorkshop = predmetWorkshop;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: RacunElektraIzvrsenjaUsluges/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge = await _context.RacunElektraIzvrsenjeUsluge
                .Include(e => e.Dopis)
                .Include(e => e.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            return racunElektraIzvrsenjeUsluge == null ? NotFound() : View(racunElektraIzvrsenjeUsluge);
        }

        // GET: RacunElektraIzvrsenjaUsluges/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id");
            return View();
        }

        // POST: RacunElektraIzvrsenjaUsluges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge)
        {
            if (ModelState.IsValid)
            {
                _ = _context.Add(racunElektraIzvrsenjeUsluge);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektraIzvrsenjeUsluge.ElektraKupacId);
            return View(racunElektraIzvrsenjeUsluge);
        }

        // GET: RacunElektraIzvrsenjaUsluges/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge = await _context.RacunElektraIzvrsenjeUsluge.FindAsync(id);
            if (racunElektraIzvrsenjeUsluge == null)
            {
                return NotFound();
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektraIzvrsenjeUsluge.ElektraKupacId);
            return View(racunElektraIzvrsenjeUsluge);
        }

        // POST: RacunElektraIzvrsenjaUsluges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge)
        {
            if (id != racunElektraIzvrsenjeUsluge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // error fix
                    // The instance of entity type 'RacunElektra' cannot be tracked because another
                    // instance with the same key value for {'Id'} is already being tracked.
                    RacunElektraIzvrsenjeUsluge reu = _context.RacunElektraIzvrsenjeUsluge.FirstOrDefault(e => e.Id == id);
                    _context.Entry(reu).State = EntityState.Detached;
                    _context.Entry(racunElektraIzvrsenjeUsluge).State = EntityState.Modified;

                    _ = _context.Update(racunElektraIzvrsenjeUsluge);
                    _ = await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunElektraIzvrsenjeUslugeExists(racunElektraIzvrsenjeUsluge.Id))
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
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektraIzvrsenjeUsluge.ElektraKupacId);
            return View(racunElektraIzvrsenjeUsluge);
        }

        // GET: RacunElektraIzvrsenjaUsluges/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge = await _context.RacunElektraIzvrsenjeUsluge
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            return racunElektraIzvrsenjeUsluge == null ? NotFound() : View(racunElektraIzvrsenjeUsluge);
        }

        // POST: RacunElektraIzvrsenjaUsluges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge = await _context.RacunElektraIzvrsenjeUsluge.FindAsync(id);
            _ = _context.RacunElektraIzvrsenjeUsluge.Remove(racunElektraIzvrsenjeUsluge);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraIzvrsenjeUslugeExists(int id)
        {
            return _context.RacunElektraIzvrsenjeUsluge.Any(e => e.Id == id);
        }

        // validation
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {
            if (brojRacuna.Length is < 19 or > 19) return Json($"Broj računa nije ispravan");
            RacunElektraIzvrsenjeUsluge db = await _context.RacunElektraIzvrsenjeUsluge.FirstOrDefaultAsync(x => x.BrojRacuna.Equals(brojRacuna));
            return db != null ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string GetUid() => _racunElektraIzvrsenjeUslugeWorkshop.GetUid(User);
        public JsonResult GetDopisiDataForFilter(int predmetId) => Json(_context.Dopis.Where(element => element.PredmetId == predmetId).ToList());
        public JsonResult GetPredmetiCreate() => Json(_context.Predmet.ToList());
        public string GetKupci() => JsonConvert.SerializeObject(_context.ElektraKupac.ToList());
        public JsonResult UpdateDbForInline(string id, string updatedColumn, string x)
            => _racunElektraIzvrsenjeUslugeWorkshop.UpdateDbForInline(id, updatedColumn, x, _context.RacunElektraIzvrsenjeUsluge, _context);
        public JsonResult SaveToDB(string _dopisId) => _racunElektraIzvrsenjeUslugeWorkshop.SaveToDb(GetUid(), _dopisId, _context.RacunElektraIzvrsenjeUsluge, _context);
        public JsonResult RemoveRow(string racunId) => _racunElektraIzvrsenjeUslugeWorkshop.RemoveRow(racunId, _context.RacunElektraIzvrsenjeUsluge, _context);
        public JsonResult RemoveAllFromDb() => _racunElektraIzvrsenjeUslugeWorkshop.RemoveAllFromDbTemp(GetUid(), _context.RacunElektraIzvrsenjeUsluge, _context);
        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string datumIzvrsenja, string usluga, string dopisId)
            => new(_racunElektraIzvrsenjeUslugeWorkshop.AddNewTemp(brojRacuna, iznos, date, datumIzvrsenja, usluga, dopisId, GetUid(), _context));
        public JsonResult GetPredmetiDataForFilter() => Json(_predmetWorkshop.GetPredmetiDataForFilter(_context.RacunElektraIzvrsenjeUsluge, _context));
        public JsonResult GetList(bool IsFiltered, string klasa, string urbroj)
            => _racunElektraIzvrsenjeUslugeWorkshop.GetListMe(IsFiltered, klasa, urbroj, _datatablesGenerator, _context, _context.RacunElektraIzvrsenjeUsluge, Request, GetUid());
    }
}
