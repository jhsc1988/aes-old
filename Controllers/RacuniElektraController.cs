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
    public class RacuniElektraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacuniElektraController(ApplicationDbContext context)
        {
            _context = context;
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


        [HttpPost]
        public string GetPredmeti()
        {
            List<Predmet> p = new();
            foreach (Predmet element in _context.Predmet.ToList())
            {
                p.Add(element);
            }

            return JsonConvert.SerializeObject(p);
        }

        public string GetDopisi()
        {
            List<Dopis> d = new();
            foreach (Dopis element in _context.Dopis.ToList())
            {
                d.Add(element);
            }

            return JsonConvert.SerializeObject(d);
        }

        public string GetKupci()
        {
            List<ElektraKupac> ek = new();

            foreach (ElektraKupac element in _context.ElektraKupac.ToList())
            {
                ek.Add(element);
            }

            foreach (ElektraKupac element in ek)
            {
                element.Ods = _context.Ods.FirstOrDefault(o => o.Id == element.OdsId);
                element.Ods.Stan = _context.Stan.FirstOrDefault(o => o.Id == element.Ods.StanId);
            }

            return JsonConvert.SerializeObject(ek);
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

        // ************************************ validation ************************************ //

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

        // ************************************ get params from DataTables ************************************ //

        private string start, length, searchValue, sortColumnName, sortDirection;

        /// <summary>
        /// Gets params from Datatables which was requested by Datatables AJAX POST method
        /// </summary>
        public void GetDatatablesParamas()
        {
            // server side parameters
            start = Request.Form["start"].FirstOrDefault();
            length = Request.Form["length"].FirstOrDefault();
            searchValue = Request.Form["search[value]"].FirstOrDefault();
            sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        }

        // ************************************ get list ************************************ //
        // ********************************************************************************** //

        [HttpPost]
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

            if (predmetIdAsInt != 0 && dopisIdAsInt == 0)
            {
                racunElektraList = await _context.RacunElektra.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt).ToListAsync();
            }

            if (predmetIdAsInt != 0 && dopisIdAsInt != 0)
            {
                racunElektraList = await _context.RacunElektra.Where(
                    x => x.Dopis.Predmet.Id == predmetIdAsInt
                         && x.Dopis.Id == dopisIdAsInt).ToListAsync();
            }

            foreach (RacunElektra racunElektra in racunElektraList)
            {
                racunElektra.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.Id == racunElektra.ElektraKupacId);
                racunElektra.Dopis = await _context.Dopis.FirstOrDefaultAsync(o => o.Id == racunElektra.DopisId);
                racunElektra.Dopis.Predmet = await _context.Predmet.FirstOrDefaultAsync(o => o.Id == racunElektra.Dopis.PredmetId);
            }

            int totalRows = racunElektraList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                racunElektraList = await racunElektraList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.ToString("dd.MM.yyyy").Contains(searchValue)
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

        // ************************************ predmeti/dopisi filter ************************************ //

        private List<Predmet> _predmetiForDopisiForFIlter;

        public JsonResult GetPredmetiDataForFilter()
        {
            List<RacunElektra> racunElektraList = _context.RacunElektra.ToList();
            foreach (RacunElektra element in racunElektraList)
            {
                element.Dopis = _context.Dopis.FirstOrDefault(x => element.DopisId == x.Id);
                element.Dopis.Predmet = _context.Predmet.FirstOrDefault(x => element.Dopis.PredmetId == x.Id);
            }

            List<Predmet> predmetList = racunElektraList.Select(element => element.Dopis.Predmet).Distinct().ToList();
            _predmetiForDopisiForFIlter = predmetList;
            return Json(predmetList);
        }

        public JsonResult GetDopisiDataForFilter(int predmetId)
        {
            List<Dopis> dopisList = _context.Dopis.ToList();
            List<Dopis> dopisForFilterList = dopisList.Where(element => element.PredmetId == predmetId).ToList();
            return Json(dopisForFilterList);
        }

        // ************************************ Inline edit update db ************************************ //

        public async Task<IActionResult> UpdateDbForInline(string id, string klasa, DateTime? datum, string napomena)
        {
            int idInt = int.Parse(id);
            RacunElektra racunToUpdate = await _context.RacunElektra.FirstAsync(x => x.Id == idInt);

            if (racunToUpdate.KlasaPlacanja == null && datum != null)
            {
                return Json(new { success = false, Message = "Ne mogu evidentirati datum potvrde bez klase plaćanja!" });
            }

            if (klasa is null && datum is null && napomena is null)
            {
                racunToUpdate.KlasaPlacanja = null;
                racunToUpdate.DatumPotvrde = null;
            }

            else if (datum is null && napomena is null)
            {
                racunToUpdate.KlasaPlacanja = klasa;
            }

            if (datum is not null)
            {
                racunToUpdate.DatumPotvrde = datum;
            }

            if (napomena is not null)
            {
                racunToUpdate.Napomena = napomena;
            }

            try
            {
                _ = await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Json(new { success = false, Message = "Greška baze podataka!" });
            }

            return Json(new { success = true, Message = "Evidentirano" });
        }

        // ************************************ get list for create ************************************ //
        // ********************************************************************************************* //

        [HttpPost]
        public async Task<IActionResult> GetListCreate()
        {

            GetDatatablesParamas();
            //RemoveAllFromDb(); // TODO: for testing - delete

            List<RacunElektraTemp> RacunElektraTempList = await _context.RacunElektraTemp.ToListAsync();

            int rbr = 1;
            foreach (RacunElektraTemp element in RacunElektraTempList)
            {
                element.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.UgovorniRacun == long.Parse(element.BrojRacuna.Substring(0, 10)));

                if (element.ElektraKupac != null)
                {
                    element.ElektraKupac.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == element.ElektraKupac.OdsId);
                    element.ElektraKupac.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == element.ElektraKupac.Ods.StanId);
                }
                element.RedniBroj = rbr++;
            }

            // filter
            int totalRows = RacunElektraTempList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                RacunElektraTempList = await RacunElektraTempList.Where(
                        x => x.RedniBroj.ToString().Contains(searchValue)
                             || x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.Ods.Stan.StanId.ToString().Contains(searchValue)
                             || x.ElektraKupac.Ods.Stan.Adresa.Contains(searchValue)
                             || (x.ElektraKupac.Ods.Stan.Korisnik != null &&
                             x.ElektraKupac.Ods.Stan.Korisnik.Contains(searchValue))
                             || (x.ElektraKupac.Ods.Stan.Status != null &&
                             x.ElektraKupac.Ods.Stan.Status.Contains(searchValue))
                             || (x.ElektraKupac.Ods.Stan.Vlasništvo != null &&
                             x.ElektraKupac.Ods.Stan.Vlasništvo.Contains(searchValue))
                             || x.DatumIzdavanja.ToString().Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue))
                    .ToDynamicListAsync<RacunElektraTemp>();
            }

            int totalRowsAfterFiltering = RacunElektraTempList.Count;

            // sorting
            RacunElektraTempList = RacunElektraTempList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            RacunElektraTempList = RacunElektraTempList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = RacunElektraTempList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        // ************************************ Save to db for Create ************************************ //
        [HttpPost]
        public JsonResult SaveToDB(string _dopisid)
        {
            int dop = int.Parse(_dopisid);

            if (dop == 0)
            {
                return Json(new { success = false, Message = "Nije odabran dopis!" });
            }

            List<RacunElektraTemp> RacunElektraTempList = _context.RacunElektraTemp.ToList();

            foreach (RacunElektraTemp e in RacunElektraTempList)
            {
                e.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(e.BrojRacuna.Substring(0, 10)));

                if (e.ElektraKupac == null)
                {
                    return Json(new { success = false, Message = "U tablici postoje računi koji se ne odnose na mjerno mjesto!" });
                }

                RacunElektra re = new()
                {
                    RedniBroj = e.RedniBroj,
                    BrojRacuna = e.BrojRacuna,
                    DatumIzdavanja = (DateTime)e.DatumIzdavanja,
                    Iznos = (double)e.Iznos,
                    DopisId = dop,
                    ElektraKupacId = e.ElektraKupac.Id,
                };
                _ = _context.RacunElektra.Add(re);
            }

            try
            {
                _ = _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return Json(new { success = false, Message = "Greška prilikom ažuriranja u bazu!" });
            }
            _ = RemoveAllFromDb(); // brisem iz temp tablice
            return Json(new { success = true, Message = "Računi su uspješno spremljeni" });
        }

        // ************************************ add new for create ************************************ //

        public async Task<IActionResult> AddNewTemp(string brojRacuna, string iznos, string date, string __guid)
        {
            double _iznos = double.Parse(iznos);
            long ugovorniRacun = long.Parse(brojRacuna[..10]); // range notation

            List<RacunElektraTemp> RacunElektraTempList = await _context.RacunElektraTemp.ToListAsync();

            List<RacunElektra> racuniTemp = new();

            ClaimsPrincipal currentUser = User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

            RacunElektraTemp re = new()
            {
                BrojRacuna = brojRacuna,
                Iznos = _iznos,
                DatumIzdavanja = DateTime.Parse(date),
                Guid = Guid.Parse(__guid),
                UserId = userId,
            };

            RacunElektraTempList.Add(re);

            int rbr = 1;
            foreach (RacunElektraTemp e in RacunElektraTempList)
            {
                e.RedniBroj = rbr++;
            }

            _ = _context.RacunElektraTemp.Add(re);
            _ = _context.SaveChanges();

            return Json(new { success = true, Message = "success" });
        }

        // ************************************ remove row ************************************ //

        public async Task<IActionResult> RemoveRow(string racunId)
        {
            int id = int.Parse(racunId);

            List<RacunElektraTemp> RacunElektraTempList = await _context.RacunElektraTemp.ToListAsync();
            RacunElektraTemp RacunToRemove = _context.RacunElektraTemp.FirstOrDefault(x => x.Id == id);
            _ = _context.RacunElektraTemp.Remove(RacunToRemove);
            _ = _context.SaveChanges();

            return Json(new { success = true, Message = "obrisano" });
        }

        public JsonResult GetGUID()
        {
            Guid guid = Guid.NewGuid();
            return Json(new { success = true, Message = guid.ToString() });

        }

        // ************************************ check if brojRacuna Exists in table ************************************ //

        public JsonResult CheckIfExists(string brojRacuna)
        {
            int t = _context.RacunElektraTemp.Where(x => x.BrojRacuna.Equals(brojRacuna)).Count();
            return t is >= 2
                ? Json(new { success = true, Message = "true" })
                : Json(new { success = false, Message = "false" });
        }

        // ************************************ check if brojRacuna Exists in payed ************************************ //

        public JsonResult CheckIfExistsInPayed(string brojRacuna)
        {
            int t = _context.RacunElektra.Where(x => x.BrojRacuna.Equals(brojRacuna)).Count();
            return t is >= 1
                ? Json(new { success = true, })
                : Json(new { success = false, });
        }

        // ************************************ remove from db  for create ************************************ //

        public JsonResult RemoveAllFromDb()
        {
            _context.RacunElektraTemp.RemoveRange(_context.RacunElektraTemp);
            _ = _context.SaveChanges();
            return Json(new { success = true, Message = "Uspješno obrisano" });
        }

    }
}