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
        private readonly IDatatablesParamsGenerator _datatablesParamsGeneratorcs;
        private readonly IRacunWorkshop _racunWorkshop;
        private readonly IRacunElektraRateWorkshop _racunElektraRateWorkshop;
        private readonly ApplicationDbContext _context;
        private List<RacunElektraRate> racunElektraRateList;
        private DatatablesParams Params;

        public RacuniElektraRateController(ApplicationDbContext context, IDatatablesParamsGenerator datatablesParamsGeneratorcs, IRacunWorkshop racunWorkshop, IRacunElektraRateWorkshop racunElektraRateWorkshop)
        {
            _context = context;
            _racunWorkshop = racunWorkshop;
            _racunElektraRateWorkshop = racunElektraRateWorkshop;
            _datatablesParamsGeneratorcs = datatablesParamsGeneratorcs;
            racunElektraRateList = _context.RacunElektraRate.ToList();
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
            return _racunWorkshop.UpdateDbForInline(id, updatedColumn, x, _context.RacunElektraRate, _context);
        }
        public JsonResult SaveToDB(string _dopisId)
        {
            return _racunWorkshop.SaveToDb(GetUid(), _dopisId, _context.RacunElektraRate, _context);
        }
        public JsonResult RemoveRow(string racunId)
        {
            return _racunWorkshop.RemoveRow(racunId, _context.RacunElektraRate, _context);
        }
        public JsonResult RemoveAllFromDb()
        {
            return _racunWorkshop.RemoveAllFromDb(GetUid(), _context.RacunElektraRate, _context);
        }
        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId)
        {
            return new JsonResult(_racunElektraRateWorkshop.AddNewTemp(brojRacuna, iznos, date, dopisId, GetUid(), _context));
        }
        public JsonResult GetPredmetiDataForFilter()
        {
            return Json(Predmet.GetPredmetiDataForFilter(RacunTip.RacunElektraRate, _context));
        }
        public JsonResult GetList(bool IsFiltered, string klasa, string urbroj)
        {
            Params = _datatablesParamsGeneratorcs.GetParams(Request);

            if (IsFiltered)
            {
                int predmetIdAsInt = klasa is null ? 0 : int.Parse(klasa);
                int dopisIdAsInt = urbroj is null ? 0 : int.Parse(urbroj);
                racunElektraRateList = _racunElektraRateWorkshop.GetList(predmetIdAsInt, dopisIdAsInt, _context);
            }
            else
            {
                racunElektraRateList = _racunElektraRateWorkshop.GetListCreateList(GetUid(), _context);
            }

            int totalRows = racunElektraRateList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                racunElektraRateList = _racunElektraRateWorkshop.GetRacuniElektraRateForDatatables(Params, _context);
            }
            int totalRowsAfterFiltering = racunElektraRateList.Count;

            racunElektraRateList = racunElektraRateList.AsQueryable().OrderBy(Params.SortColumnName + " " + Params.SortDirection).ToList(); // sorting
            racunElektraRateList = racunElektraRateList.Skip(Params.Start).Take(Params.Length).ToList(); // paging

            return Json(new
            {
                data = racunElektraRateList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }
    }
}
