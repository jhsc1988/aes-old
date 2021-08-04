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
    public class RacunElektraIzvrsenjaUslugesController : Controller, IRacunController
    {
        private readonly ApplicationDbContext _context;
        private readonly Predmet predmet;
        private readonly Dopis dopis;
        private readonly List<Predmet> predmetList;
        private readonly List<ElektraKupac> elektraKupacList;
        private List<RacunElektraIzvrsenjeUsluge> racunElektraIzvrsenjeList;

        /// <summary>
        /// datatables params
        /// </summary>
        private string start, length, searchValue, sortColumnName, sortDirection;

        public RacunElektraIzvrsenjaUslugesController(ApplicationDbContext context)
        {
            _context = context;
            predmet = new(_context);
            dopis = new(_context);
            racunElektraIzvrsenjeList = _context.RacunElektraIzvrsenjeUsluge.ToList();
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

        // GET: RacunElektraIzvrsenjaUsluges/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraIzvrsenjeUsluge = await _context.RacunElektraIzvrsenjeUsluge
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraIzvrsenjeUsluge == null)
            {
                return NotFound();
            }

            return View(racunElektraIzvrsenjeUsluge);
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
                _context.Add(racunElektraIzvrsenjeUsluge);
                await _context.SaveChangesAsync();
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

            var racunElektraIzvrsenjeUsluge = await _context.RacunElektraIzvrsenjeUsluge.FindAsync(id);
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
                    _context.Update(racunElektraIzvrsenjeUsluge);
                    await _context.SaveChangesAsync();
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

            var racunElektraIzvrsenjeUsluge = await _context.RacunElektraIzvrsenjeUsluge
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraIzvrsenjeUsluge == null)
            {
                return NotFound();
            }

            return View(racunElektraIzvrsenjeUsluge);
        }

        // POST: RacunElektraIzvrsenjaUsluges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var racunElektraIzvrsenjeUsluge = await _context.RacunElektraIzvrsenjeUsluge.FindAsync(id);
            _context.RacunElektraIzvrsenjeUsluge.Remove(racunElektraIzvrsenjeUsluge);
            await _context.SaveChangesAsync();
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
            if (brojRacuna.Length < 19 || brojRacuna.Length > 19)
            {
                return Json($"Broj računa nije ispravan");
            }

            var db = await _context.RacunElektraIzvrsenjeUsluge.FirstOrDefaultAsync(x => x.BrojRacuna.Equals(brojRacuna));
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

            racunElektraIzvrsenjeList = RacunElektraIzvrsenjeUsluge.GetList(predmetIdAsInt, dopisIdAsInt, _context);

            // popunjava podatke za JSON da mogu vezane podatke pregledavati u datatables
            foreach (RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge in racunElektraIzvrsenjeList)
            {
                racunElektraIzvrsenjeUsluge.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.Id == racunElektraIzvrsenjeUsluge.ElektraKupacId);
            }

            // filter
            int totalRows = racunElektraIzvrsenjeList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                racunElektraIzvrsenjeList = await racunElektraIzvrsenjeList.
                    Where(
                    x => x.BrojRacuna.Contains(searchValue)
                    || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                    || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                    || x.DatumIzvrsenja.ToString("dd.MM.yyyy").Contains(searchValue)
                    || (x.Usluga != null && x.Usluga.ToLower().Contains(searchValue.ToLower()))
                    || x.Iznos.ToString().Contains(searchValue)
                    || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                    || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue))).ToDynamicListAsync<RacunElektraIzvrsenjeUsluge>();
            }
            int totalRowsAfterFiltering = racunElektraIzvrsenjeList.Count;

            // sorting
            racunElektraIzvrsenjeList = racunElektraIzvrsenjeList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            racunElektraIzvrsenjeList = racunElektraIzvrsenjeList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<RacunElektraIzvrsenjeUsluge>();

            return Json(new { data = racunElektraIzvrsenjeList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        public async Task<IActionResult> GetListCreate()
        {
            GetDatatablesParamas();

            racunElektraIzvrsenjeList = RacunElektraIzvrsenjeUsluge.GetListCreateList(GetUid(), _context);

            // filter
            int totalRows = racunElektraIzvrsenjeList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                racunElektraIzvrsenjeList = await racunElektraIzvrsenjeList.Where(
                        x => x.RedniBroj.ToString().Contains(searchValue)
                             || x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.Ods.Stan.StanId.ToString().Contains(searchValue)
                             || (x.ElektraKupac.Ods.Stan.Adresa != null && x.ElektraKupac.Ods.Stan.Adresa.Contains(searchValue))
                             || (x.ElektraKupac.Ods.Stan.Korisnik != null &&
                             x.ElektraKupac.Ods.Stan.Korisnik.Contains(searchValue))
                             || (x.ElektraKupac.Ods.Stan.Vlasništvo != null &&
                             x.ElektraKupac.Ods.Stan.Vlasništvo.Contains(searchValue))
                             || x.DatumIzdavanja.ToString().Contains(searchValue)
                             || x.DatumIzvrsenja.ToString().Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue)
                             || x.Napomena.ToString().Contains(searchValue))
                    .ToDynamicListAsync<RacunElektraIzvrsenjeUsluge>();
            }

            int totalRowsAfterFiltering = racunElektraIzvrsenjeList.Count;

            // sorting
            racunElektraIzvrsenjeList = racunElektraIzvrsenjeList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            racunElektraIzvrsenjeList = racunElektraIzvrsenjeList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = racunElektraIzvrsenjeList,
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
            return Json(predmet.GetPredmetiDataForFilter(RacunTip.ElektraIzvrsenje));
        }

        public JsonResult GetDopisiDataForFilter(int predmetId)
        {
            return Json(dopis.GetDopisiDataForFilter(predmetId));
        }

        public JsonResult GetPredmetiCreate()
        {
            return Json(predmetList);
        }

        public JsonResult GetDopisiCreate(int predmetId)
        {
            return Json(dopis.GetDopisiDataForFilter(predmetId));
        }

        public string GetKupci()
        {
            return JsonConvert.SerializeObject(elektraKupacList);
        }

        public JsonResult UpdateDbForInline(string id, string updatedColumn, string x)
        {
            return Racun.UpdateDbForInline(RacunTip.ElektraIzvrsenje, id, updatedColumn, x, _context);
        }

        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string datumIzvrsenja, string usluga, string dopisId)
        {
            return new JsonResult(RacunElektraIzvrsenjeUsluge.AddNewTemp(brojRacuna, iznos, date, datumIzvrsenja, usluga, dopisId, GetUid(), _context));
        }

        public JsonResult SaveToDB(string _dopisId)
        {
            return Racun.SaveToDb(RacunTip.ElektraIzvrsenje, GetUid(), _dopisId, _context);
        }

        public JsonResult RemoveRow(string racunId)
        {
            return Racun.RemoveRow(RacunTip.ElektraIzvrsenje, racunId, _context);
        }

        public JsonResult RemoveAllFromDb()
        {
            return Racun.RemoveAllFromDb(RacunTip.ElektraIzvrsenje, GetUid(), _context);
        }
    }
}
