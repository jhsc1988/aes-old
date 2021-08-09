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
    public class RacuniHoldingController : Controller, IRacunController
    {
        private readonly ApplicationDbContext _context;
        private readonly Predmet predmet;
        private readonly Dopis dopis;
        private readonly List<Predmet> predmetList;
        private readonly List<Stan> stanList;
        private List<RacunHolding> racunHoldingList;

        /// <summary>
        /// datatables params
        /// </summary>
        private string start, length, searchValue, sortColumnName, sortDirection;

        public RacuniHoldingController(ApplicationDbContext context)
        {
            _context = context;
            predmet = new(_context);
            dopis = new();
            racunHoldingList = _context.RacunHolding.ToList();
            stanList = _context.Stan.ToList();
            predmetList = _context.Predmet.ToList();
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: RacuniHolding/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunHolding = await _context.RacunHolding
                .Include(r => r.Dopis)
                .Include(r => r.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunHolding == null)
            {
                return NotFound();
            }

            return View(racunHolding);
        }

        // GET: RacuniHolding/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa");
            return View();
        }

        // POST: RacuniHolding/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,StanId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunHolding racunHolding)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racunHolding);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", racunHolding.StanId);
            return View(racunHolding);
        }

        // GET: RacuniHolding/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunHolding = await _context.RacunHolding.FindAsync(id);
            if (racunHolding == null)
            {
                return NotFound();
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", racunHolding.StanId);
            return View(racunHolding);
        }

        // POST: RacuniHolding/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,StanId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunHolding racunHolding)
        {
            if (id != racunHolding.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racunHolding);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunHoldingExists(racunHolding.Id))
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
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", racunHolding.StanId);
            return View(racunHolding);
        }

        // GET: RacuniHolding/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunHolding = await _context.RacunHolding
                .Include(r => r.Dopis)
                .Include(r => r.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunHolding == null)
            {
                return NotFound();
            }

            return View(racunHolding);
        }

        // POST: RacuniHolding/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunHolding racunHolding = await _context.RacunHolding.FindAsync(id);
            _ = _context.RacunHolding.Remove(racunHolding);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunHoldingExists(int id)
        {
            return _context.RacunHolding.Any(e => e.Id == id);
        }

        // validation
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {
            if (brojRacuna.Length is < 20 or > 20)
            {
                return Json($"Broj računa nije ispravan");
            }

            RacunHolding db = await _context.RacunHolding.FirstOrDefaultAsync(x => x.BrojRacuna.Equals(brojRacuna));
            return db != null ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
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

        public async Task<IActionResult> GetList(string klasa, string urbroj)
        {

            int predmetIdAsInt = klasa is null ? 0 : int.Parse(klasa);
            int dopisIdAsInt = urbroj is null ? 0 : int.Parse(urbroj);

            GetDatatablesParamas();

            racunHoldingList = RacunHolding.GetList(predmetIdAsInt, dopisIdAsInt, _context);

            // filter
            int totalRows = racunHoldingList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                racunHoldingList = await racunHoldingList.
                    Where(
                    x => x.BrojRacuna.Contains(searchValue)
                    || x.Stan.SifraObjekta.ToString().Contains(searchValue)
                    || x.Stan.StanId.ToString().Contains(searchValue)
                    || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                    || x.Iznos.ToString().Contains(searchValue)
                    || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                    || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))).ToDynamicListAsync<RacunHolding>();
            }
            int totalRowsAfterFiltering = racunHoldingList.Count;
            racunHoldingList = racunHoldingList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            racunHoldingList = racunHoldingList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new { data = racunHoldingList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        public async Task<IActionResult> GetListCreate()
        {

            GetDatatablesParamas();
            racunHoldingList = RacunHolding.GetListCreateList(GetUid(), _context);
           
            int totalRows = racunHoldingList.Count;
           
            // filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                racunHoldingList = await racunHoldingList.
                    Where(
                    x => x.BrojRacuna.Contains(searchValue)
                    || x.Stan.SifraObjekta.ToString().Contains(searchValue)
                    || x.Stan.StanId.ToString().Contains(searchValue)
                    || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                    || x.Iznos.ToString().Contains(searchValue)
                    || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                    || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))).ToDynamicListAsync<RacunHolding>();
            }
            int totalRowsAfterFiltering = racunHoldingList.Count;
            racunHoldingList = racunHoldingList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            racunHoldingList = racunHoldingList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new { data = racunHoldingList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string GetUid()
        {
            ClaimsPrincipal currentUser;
            currentUser = User; // User postoji samo u COntrolleru
            return currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public JsonResult GetPredmetiDataForFilter()
        {
            return Json(predmet.GetPredmetiDataForFilter(RacunTip.Holding));
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
            return JsonConvert.SerializeObject(stanList);
        }

        public JsonResult UpdateDbForInline(string id, string updatedColumn, string x)
        {
            return Racun.UpdateDbForInline(RacunTip.Holding, id, updatedColumn, x, _context);
        }

        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId)
        {
            return new JsonResult(RacunHolding.AddNewTemp(brojRacuna, iznos, date, dopisId, GetUid(), _context));
        }

        public JsonResult SaveToDB(string _dopisId)
        {
            return Racun.SaveToDb(RacunTip.Holding, GetUid(), _dopisId, _context);
        }

        public JsonResult RemoveRow(string racunId)
        {
            return Racun.RemoveRow(RacunTip.Holding, racunId, _context);
        }

        public JsonResult RemoveAllFromDb()
        {
            return Racun.RemoveAllFromDb(RacunTip.Holding, GetUid(), _context);
        }
    }
}
