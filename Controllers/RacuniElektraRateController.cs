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
        private readonly ApplicationDbContext _context;
        private readonly Predmet predmet;
        private readonly Dopis dopis;
        private readonly List<Predmet> predmetList;
        private readonly List<ElektraKupac> elektraKupacList;
        private List<RacunElektraRate> racunElektraRateList;

        /// <summary>
        /// datatables params
        /// </summary>
        private string start, length, searchValue, sortColumnName, sortDirection;
        public RacuniElektraRateController(ApplicationDbContext context)
        {
            _context = context;
            predmet = new();
            dopis = new();
            racunElektraRateList = _context.RacunElektraRate.ToList();
            elektraKupacList = _context.ElektraKupac.ToList();
            predmetList = _context.Predmet.ToList();

            foreach (ElektraKupac e in _context.ElektraKupac.ToList())
            {
                e.Ods = _context.Ods.FirstOrDefault(o => o.Id == e.OdsId);
                e.Ods.Stan = _context.Stan.FirstOrDefault(o => o.Id == e.Ods.StanId);
            }
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
                _context.Add(racunElektraRate);
                await _context.SaveChangesAsync();
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

            var racunElektraRate = await _context.RacunElektraRate.FindAsync(id);
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
                    _context.Update(racunElektraRate);
                    await _context.SaveChangesAsync();
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

            var racunElektraRate = await _context.RacunElektraRate
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraRate == null)
            {
                return NotFound();
            }

            return View(racunElektraRate);
        }

        // POST: RacuniElektraRate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var racunElektraRate = await _context.RacunElektraRate.FindAsync(id);
            _context.RacunElektraRate.Remove(racunElektraRate);
            await _context.SaveChangesAsync();
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
            if (brojRacuna.Length < 19 || brojRacuna.Length > 19)
            {
                return Json($"Broj računa nije ispravan");
            }

            var db = await _context.RacunElektraRate.FirstOrDefaultAsync(x => x.BrojRacuna.Equals(brojRacuna));
            if (db != null)
            {
                return Json($"Račun {brojRacuna} već postoji.");
            }
            return Json(true);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void GetDatatablesParamas()
        {
            // server side parameters
            start = Request.Form["start"].FirstOrDefault();
            length = Request.Form["length"].FirstOrDefault();
            searchValue = Request.Form["search[value]"].FirstOrDefault();
            sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        }

        [HttpPost]
        public async Task<IActionResult> GetList(string klasa, string urbroj)
        {

            int predmetIdAsInt = klasa is null ? 0 : int.Parse(klasa);
            int dopisIdAsInt = urbroj is null ? 0 : int.Parse(urbroj);

            GetDatatablesParamas();

            racunElektraRateList = RacunElektraRate.GetList(predmetIdAsInt, dopisIdAsInt, _context);

            int totalRows = racunElektraRateList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                racunElektraRateList = await racunElektraRateList.
                    Where(
                    x => x.BrojRacuna.Contains(searchValue)
                    || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                    || x.Razdoblje.ToString("MM.yyyy").Contains(searchValue)
                    || x.Iznos.ToString().Contains(searchValue)
                    || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                    || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))).ToDynamicListAsync<RacunElektraRate>();
            }
            int totalRowsAfterFiltering = racunElektraRateList.Count;

            // sorting
            racunElektraRateList = racunElektraRateList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            racunElektraRateList = racunElektraRateList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<RacunElektraRate>();

            return Json(new { data = racunElektraRateList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        public async Task<IActionResult> GetListCreate()
        {
            GetDatatablesParamas();

            racunElektraRateList = RacunElektraRate.GetListCreateList(GetUid(), _context);

            // filter
            int totalRows = racunElektraRateList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                racunElektraRateList = await racunElektraRateList.Where(
                        x => x.RedniBroj.ToString().Contains(searchValue)
                             || x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.Ods.Stan.StanId.ToString().Contains(searchValue)
                             || (x.ElektraKupac.Ods.Stan.Adresa != null && x.ElektraKupac.Ods.Stan.Adresa.Contains(searchValue))
                             || (x.ElektraKupac.Ods.Stan.Korisnik != null &&
                             x.ElektraKupac.Ods.Stan.Korisnik.Contains(searchValue))
                             || (x.ElektraKupac.Ods.Stan.Vlasništvo != null &&
                             x.ElektraKupac.Ods.Stan.Vlasništvo.Contains(searchValue))
                             || x.DatumIzdavanja.ToString().Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue))
                    .ToDynamicListAsync<RacunElektraRate>();
            }

            int totalRowsAfterFiltering = racunElektraRateList.Count;

            // sorting
            racunElektraRateList = racunElektraRateList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            racunElektraRateList = racunElektraRateList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = racunElektraRateList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string GetUid()
        {
            ClaimsPrincipal currentUser;
            currentUser = User;
            return currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public JsonResult GetPredmetiDataForFilter()
        {
            return Json(predmet.GetPredmetiDataForFilter(RacunTip.RacunElektraRate, _context));
        }

        public JsonResult GetDopisiDataForFilter(int predmetId)
        {
            return Json(dopis.GetDopisiDataForFilter(predmetId, _context));
        }

        public JsonResult GetPredmetiCreate()
        {
            return Json(predmetList);
        }

        public JsonResult GetDopisiCreate(int predmetId)
        {
            return Json(dopis.GetDopisiDataForFilter(predmetId, _context));
        }

        public string GetKupci()
        {
            return JsonConvert.SerializeObject(elektraKupacList);
        }

        public JsonResult UpdateDbForInline(string id, string updatedColumn, string x)
        {
            return Racun.UpdateDbForInline(RacunTip.RacunElektraRate, id, updatedColumn, x, _context);
        }

        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId)
        {
            return new JsonResult(RacunElektraRate.AddNewTemp(brojRacuna, iznos, date, dopisId, GetUid(), _context));
        }

        public JsonResult SaveToDB(string _dopisId)
        {
            return Racun.SaveToDb(RacunTip.RacunElektraRate, GetUid(), _dopisId, _context);
        }

        public JsonResult RemoveRow(string racunId)
        {
            return Racun.RemoveRow(RacunTip.RacunElektraRate, racunId, _context);
        }

        public JsonResult RemoveAllFromDb()
        {
            return Racun.RemoveAllFromDb(RacunTip.RacunElektraRate, GetUid(), _context);
        }
    }
}
