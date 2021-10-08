﻿using aes.Data;
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
    public class RacuniElektraController : Controller, IRacunController
    {
        private readonly IDatatablesParamsGenerator _datatablesParamsGeneratorcs;
        private readonly IRacunWorkshop _racunWorkshop;
        private readonly IRacunElektraWorkshop _racunElektraWorkshop;
        private readonly ApplicationDbContext _context;
        private List<RacunElektra> racunElektraList;
        private DatatablesParams Params;

        public RacuniElektraController(ApplicationDbContext context, IDatatablesParamsGenerator datatablesParamsGeneratorcs, IRacunWorkshop racunWorkshop, IRacunElektraWorkshop racunElektraWorkshop)
        {
            _context = context;
            _racunWorkshop = racunWorkshop;
            _racunElektraWorkshop = racunElektraWorkshop;
            _datatablesParamsGeneratorcs = datatablesParamsGeneratorcs;
            racunElektraList = _context.RacunElektra.ToList();
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: RacuniElektra/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektra racunElektra = await _context.RacunElektra
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);

            return racunElektra == null ? NotFound() : View(racunElektra);
        }

        // GET: RacuniElektra/Create
        [Authorize]
        public async Task<IActionResult> CreateAsync()
        {
            List<RacunElektra> applicationDbContext = await _context.RacunElektra.ToListAsync();

            return View(applicationDbContext);
        }

        // POST: RacuniElektra/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(
            [Bind(
                "Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")]
            RacunElektra racunElektra)
        {
            // ModelState debbuger:
            // var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                racunElektra.VrijemeUnosa = DateTime.Now;
                _ = _context.Add(racunElektra);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //return View(racunElektra);
            return View();
        }

        // GET: RacuniElektra/Edit/5

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektra racunElektra = await _context.RacunElektra.FindAsync(id);
            if (racunElektra == null)
            {
                return NotFound();
            }

            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektra.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektra.ElektraKupacId);

            return View(racunElektra);
        }

        // POST: RacuniElektra/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")]
            RacunElektra racunElektra)
        {
            if (id != racunElektra.Id)
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
                    RacunElektra re = _context.RacunElektra.FirstOrDefault(e => e.Id == id);
                    _context.Entry(re).State = EntityState.Detached;
                    _context.Entry(racunElektra).State = EntityState.Modified;

                    _ = _context.Update(racunElektra);
                    _ = await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunElektraExists(racunElektra.Id))
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

            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektra.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektra.ElektraKupacId);

            return View(racunElektra);
        }

        // GET: RacuniElektra/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektra racunElektra = await _context.RacunElektra
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            return racunElektra == null ? NotFound() : View(racunElektra);
        }

        // POST: RacuniElektra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunElektra racunElektra = await _context.RacunElektra.FindAsync(id);
            _ = _context.RacunElektra.Remove(racunElektra);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraExists(int id)
        {
            return _context.RacunElektra.Any(e => e.Id == id);
        }

        /// <summary>
        /// Validation
        /// </summary>
        /// <param name="broj računa"></param>
        /// <returns>async Task<IActionResult> (JSON)</returns>
        [HttpGet]
        public JsonResult BrojRacunaValidation(string brojRacuna)
        {
            return brojRacuna.Length is < 19 or > 19 ? Json($"Broj računa nije ispravan") : Json(true);

            //RacunElektra db = await _context.RacunElektra.FirstOrDefaultAsync(x => x.BrojRacuna.Equals(brojRacuna));
            //return db != null ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string GetUid()
        {
            ClaimsPrincipal currentUser;
            currentUser = User;
            return currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public JsonResult GetDopisiDataForFilter(int predmetId)
        {
            return Json(_context.Dopis.Where(element => element.PredmetId == predmetId).ToList());
        }
        public JsonResult GetPredmetiCreate()
        {
            return Json(_context.Predmet.ToList());
        }
        public string GetKupci()
        {
            return JsonConvert.SerializeObject(_context.ElektraKupac.ToList());
        }
        public JsonResult UpdateDbForInline(string id, string updatedColumn, string x)
        {
            return _racunWorkshop.UpdateDbForInline(id, updatedColumn, x, _context.RacunElektra, _context);
        }
        public JsonResult SaveToDB(string _dopisId)
        {
            return _racunWorkshop.SaveToDb(GetUid(), _dopisId, _context.RacunElektra, _context);
        }
        public JsonResult RemoveRow(string racunId)
        {
            return _racunWorkshop.RemoveRow(racunId, _context.RacunElektra, _context);
        }
        public JsonResult RemoveAllFromDb()
        {
            return _racunWorkshop.RemoveAllFromDb(GetUid(), _context.RacunElektra, _context);
        }
        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId)
        {
            return new JsonResult(_racunElektraWorkshop.AddNewTemp(brojRacuna, iznos, date, dopisId, GetUid(), _context));
        }

        public JsonResult GetPredmetiDataForFilter()
        {
            return Json(Predmet.GetPredmetiDataForFilter(RacunTip.RacunElektra, _context));
        }
        public JsonResult GetList(bool isFiltered, string klasa, string urbroj)
        {

            Params = _datatablesParamsGeneratorcs.GetParams(Request);

            if (isFiltered)
            {
                int predmetIdAsInt = klasa is null ? 0 : int.Parse(klasa);
                int dopisIdAsInt = urbroj is null ? 0 : int.Parse(urbroj);
                racunElektraList = _racunElektraWorkshop.GetList(predmetIdAsInt, dopisIdAsInt, _context);
            }
            else
            {
                racunElektraList = _racunElektraWorkshop.GetListCreateList(GetUid(), _context);
            }

            int totalRows = racunElektraList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue)) // filter
            {
                racunElektraList = _racunElektraWorkshop.GetRacuniElektraForDatatables(Params, _context);
            }
            int totalRowsAfterFiltering = racunElektraList.Count;

            racunElektraList = racunElektraList.AsQueryable().OrderBy(Params.SortColumnName + " " + Params.SortDirection).ToList(); // sorting
            racunElektraList = racunElektraList.Skip(Params.Start).Take(Params.Length).ToList(); // paging

            return Json(new
            {
                data = racunElektraList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }
    }
}
