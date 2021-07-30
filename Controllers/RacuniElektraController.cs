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
    public class RacuniElektraController : Controller, IRacunController
    {
        private readonly ApplicationDbContext _context;
        private readonly Predmet predmet;
        private readonly Dopis dopis;
        private readonly List<Predmet> predmetList;
        private readonly List<ElektraKupac> elektraKupacList;
        private List<RacunElektra> racunElektraList;
        private string userId;
        public RacuniElektraController(ApplicationDbContext context)
        {
            _context = context;
            predmet = new(_context);
            dopis = new(_context);
            racunElektraList = _context.RacunElektra.ToList();
            elektraKupacList = _context.ElektraKupac.ToList();
            predmetList = _context.Predmet.ToList();

            foreach (ElektraKupac e in _context.ElektraKupac.ToList())
            {
                e.Ods = _context.Ods.FirstOrDefault(o => o.Id == e.OdsId);
                e.Ods.Stan = _context.Stan.FirstOrDefault(o => o.Id == e.Ods.StanId);
            }

        }

        private string GetUid()
        {
            ClaimsPrincipal currentUser;
            currentUser = User;
            return currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
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
        /// <param name="brojRacuna"></param>
        /// <returns>async Task<IActionResult> (JSON)</returns>
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {
            if (brojRacuna.Length is < 19 or > 19) // pattern mathching syntax
            {
                return Json($"Broj računa nije ispravan");
            }

            RacunElektra db = await _context.RacunElektra.FirstOrDefaultAsync(x => x.BrojRacuna.Equals(brojRacuna));
            return db != null ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Datatables parameters
        /// </summary>
        private string start, length, searchValue, sortColumnName, sortDirection;

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

            List<RacunElektra> racunElektraList = new();

            if (predmetIdAsInt == 0 && dopisIdAsInt == 0)
            {
                racunElektraList = await _context.RacunElektra.ToListAsync();
            }

            if (predmetIdAsInt != 0)
            {
                racunElektraList = dopisIdAsInt == 0
                    ? await _context.RacunElektra.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt).ToListAsync()
                    : await _context.RacunElektra.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt && x.Dopis.Id == dopisIdAsInt).ToListAsync();
            }


            foreach (RacunElektra racunElektra in racunElektraList)
            {
                racunElektra.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.Id == racunElektra.ElektraKupacId);
                racunElektra.Dopis = await _context.Dopis.FirstOrDefaultAsync(o => o.Id == racunElektra.DopisId);

                if (racunElektra.Dopis != null)
                {
                    racunElektra.Dopis.Predmet = await _context.Predmet.FirstOrDefaultAsync(o => o.Id == racunElektra.Dopis.PredmetId);
                }
            }

            int totalRows = racunElektraList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                racunElektraList = await racunElektraList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue)
                             || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                             || (x.DatumPotvrde != null &&
                                 x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                             || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower())))
                    .ToDynamicListAsync<RacunElektra>();
            }

            int totalRowsAfterFiltering = racunElektraList.Count;
            racunElektraList = racunElektraList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            racunElektraList = racunElektraList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new
            {
                data = racunElektraList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        public async Task<IActionResult> GetListCreate()
        {
            GetDatatablesParamas();

            userId = GetUid();
            racunElektraList = await _context.RacunElektra.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToListAsync();

            int rbr = 1;
            foreach (RacunElektra e in racunElektraList)
            {

                e.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.UgovorniRacun == long.Parse(e.BrojRacuna.Substring(0, 10)));

                List<Racun> racunList = new();
                racunList.AddRange(_context.RacunElektra.Where(e => e.IsItTemp == null || false).ToList());
                e.Napomena = Racun.CheckIfExistsInPayed(e.BrojRacuna, racunList);
                racunList.Clear();

                racunList.AddRange(_context.RacunElektra.Where(e => e.IsItTemp == true && e.CreatedByUserId == GetUid()).ToList());
                if (e.Napomena is null)
                {
                    e.Napomena = Racun.CheckIfExists(e.BrojRacuna, racunList);
                }

                racunList.Clear();

                if (e.ElektraKupac != null)
                {
                    e.ElektraKupac.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.OdsId);
                    e.ElektraKupac.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.Ods.StanId);
                }
                else
                {
                    e.Napomena = "kupac ne postoji";
                }
                e.RedniBroj = rbr++;
            }

            // filter
            int totalRows = racunElektraList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                racunElektraList = await racunElektraList.Where(
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
                    .ToDynamicListAsync<RacunElektra>();
            }

            int totalRowsAfterFiltering = racunElektraList.Count;

            // sorting
            racunElektraList = racunElektraList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            racunElektraList = racunElektraList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = racunElektraList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public JsonResult GetPredmetiDataForFilter()
        {
            List<Racun> re = new();
            racunElektraList = _context.RacunElektra.ToList();
            re.AddRange(racunElektraList);
            return Json(predmet.GetPredmetiDataForFilter(re));
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

        public async Task<IActionResult> UpdateDbForInline(string id, string updatedColumn, string x)
        {
            int idNum = int.Parse(id);
            int updatedColumnNum = int.Parse(updatedColumn);
            RacunElektra racunToUpdate = await _context.RacunElektra.FirstAsync(e => e.Id == idNum);
            return Racun.UpdateDbForInline(updatedColumnNum, x, racunToUpdate, _context);
        }

        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId)
        {
            return new JsonResult(RacunElektra.AddNewTemp(brojRacuna, iznos, date, dopisId, GetUid(), _context));
        }

        public JsonResult SaveToDB(string _dopisid)
        {
            return RacunElektra.SaveToDB(_dopisid, GetUid(), _context);
        }

        public async Task<IActionResult> RemoveRow(string racunId)
        {
            int id = int.Parse(racunId);
            racunElektraList = _context.RacunElektra.Where(e => e.IsItTemp == true && e.CreatedByUserId == GetUid()).ToList();
            List<Racun> racunList = new();
            racunList.AddRange(racunElektraList);
            RacunElektra RacunToRemove = await _context.RacunElektra.FirstOrDefaultAsync(x => x.Id == id);
            _context.RacunElektra.Remove(RacunToRemove);
            return Racun.RemoveRow(_context);
        }

        public JsonResult RemoveAllFromDb()
        {
            return new JsonResult(RacunElektra.RemoveAllFromDb(GetUid(), _context));
        }
    }
}
