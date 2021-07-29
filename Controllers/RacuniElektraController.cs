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
        private readonly Predmet predmet;
        private readonly Dopis dopis;
        private readonly List<Predmet> predmetList;
        private readonly List<ElektraKupac> elektraKupacList;
        private List<RacunElektra> racunElektraList;
        private readonly ClaimsPrincipal currentUser;
        private readonly string userId;
        public RacuniElektraController(ApplicationDbContext context)
        {
            _context = context;
            predmet = new(_context);
            dopis = new(_context);
            racunElektraList = _context.RacunElektra.ToList();
            elektraKupacList = _context.ElektraKupac.ToList();
            predmetList = _context.Predmet.ToList();

            currentUser = User;
            userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

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

        /// <summary>
        /// Datatables parameters
        /// </summary>
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

        /// <summary>
        /// Gets list of all Racuni
        /// </summary>
        /// <param name="klasa"></param>
        /// <param name="urbroj"></param>
        /// <returns>async Task<IActionResult> (JSON)</returns>
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

        /// <summary>
        /// Gets predmeti for filtered data
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetPredmetiDataForFilter()
        {
            List<Racun> re = new();
            re.AddRange(racunElektraList);
            return Json(predmet.GetPredmetiDataForFilter(re));
        }

        /// <summary>
        /// Gets dopisi for predmet for filtered data
        /// </summary>
        /// <param name="predmetId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDopisiDataForFilter(int predmetId)
        {
            return Json(dopis.GetDopisiDataForFilter(predmetId));
        }

        /// <summary>
        /// Gets list of Kupci for notification (info) builder
        /// </summary>
        /// <returns>string</returns>
        public string GetKupci()
        {
            return JsonConvert.SerializeObject(elektraKupacList);
        }

        /// <summary>
        /// Columns in Index - used for inline editor
        /// </summary>
        private enum Columns
        {
            racun = 1, datumIzdavanja = 2, iznos = 3, klasa = 4, datumPotvrde = 5, napomena = 6
        }

        /// <summary>
        /// Db update on inline edit
        /// </summary>
        /// <param name="id">Id of Racun</param>
        /// <param name="updatedColumn">Column which was updated</param>
        /// <param name="x">Changed text variable</param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateDbForInline(string id, string updatedColumn, string x)
        {

            int idNum = int.Parse(id);
            int updatedColumnNum = int.Parse(updatedColumn);
            RacunElektra racunToUpdate = await _context.RacunElektra.FirstAsync(x => x.Id == idNum);
            Columns column = (Columns)updatedColumnNum;

            switch (column)
            {
                case Columns.racun:
                    if (x.Length < 10)
                    {
                        return Json(new { success = false, Message = "Broj računa nije ispravan!" });
                    }
                    if (!x.Substring(0, 10).Equals(racunToUpdate.BrojRacuna.Substring(0, 10)))
                    {
                        return Json(new { success = false, Message = "Pogrešan broj računa - ugovorni računi ne smije se razlikovati!" });
                    }
                    racunToUpdate.BrojRacuna = x;
                    break;

                case Columns.datumIzdavanja:
                    racunToUpdate.DatumIzdavanja = DateTime.Parse(x);
                    break;

                case Columns.iznos:
                    racunToUpdate.Iznos = double.Parse(x);
                    break;

                case Columns.klasa:
                    racunToUpdate.KlasaPlacanja = x;
                    if (racunToUpdate.KlasaPlacanja is null && racunToUpdate.DatumPotvrde is not null)
                    {
                        racunToUpdate.DatumPotvrde = null;
                    }
                    break;

                case Columns.datumPotvrde:
                    if (racunToUpdate.KlasaPlacanja is null)
                    {
                        return Json(new { success = false, Message = "Ne mogu evidentirati datum potvrde bez klase plaćanja!" });
                    }
                    else
                    {
                        racunToUpdate.DatumPotvrde = DateTime.Parse(x);
                    }
                    break;

                case Columns.napomena:
                    racunToUpdate.Napomena = x;
                    break;

                default:
                    break;
            }

            try
            {
                _ = _context.SaveChanges();
                return Json(new { success = true, Message = "Spremljeno" });
            }

            catch (DbUpdateException)
            {
                return Json(new { success = false, Message = "Greška!" });
            }

        }

        /// <summary>
        /// Gets RacunElektraTemp list for Create Datatables
        /// </summary>
        /// <returns>async Task<IActionResult> (JSON)</returns>
        public async Task<IActionResult> GetListCreate()
        {
            GetDatatablesParamas();

            racunElektraList = await _context.RacunElektra.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToListAsync();

            int rbr = 1;
            foreach (RacunElektra e in racunElektraList)
            {
                e.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.UgovorniRacun == long.Parse(e.BrojRacuna.Substring(0, 10)));

                if (_context.RacunElektra.Any(o => o.BrojRacuna == e.BrojRacuna))
                {
                    e.Napomena = "račun već plaćen";
                }

                if (_context.RacunElektra.Where(o => o.BrojRacuna == e.BrojRacuna).Count() >= 2)
                {
                    e.Napomena = "dupli račun";
                }

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

        /// <summary>
        /// Gets list of predmeti for dropdown on Create page
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPredmetiCreate()
        {
            return Json(predmetList);
        }

        /// <summary>
        /// Gets list of dopisi for predmet for dropdown on Create page
        /// </summary>
        /// <param name="predmetId">Id of Predmet</param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDopisiCreate(int predmetId)
        {
            return Json(dopis.GetDopisiDataForFilter(predmetId));
        }

        /// <summary>
        /// Moves from RacunElektraTemp to RacuniElektra table
        /// </summary>
        /// <param name="_dopisid">Id of Dopis</param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveToDB(string _dopisid)
        {
            int dop = int.Parse(_dopisid);

            if (dop == 0)
            {
                return Json(new { success = false, Message = "Nije odabran dopis!" });
            }

            racunElektraList = _context.RacunElektra.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToList();


            foreach (RacunElektra e in racunElektraList)
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
                    DatumIzdavanja = e.DatumIzdavanja,
                    Iznos = (double)e.Iznos,
                    DopisId = dop,
                    ElektraKupacId = e.ElektraKupac.Id,
                    IsItTemp = null,
                };
                _ = _context.RacunElektra.Add(re);
            }
            try
            {
                _ = _context.SaveChanges();
                _ = RemoveAllFromDb(); // remove temp flag
                return Json(new { success = true, Message = "Spremljeno" });

            }
            catch (DbUpdateException)
            {
                return Json(new { success = true, Message = "Greška" });
            }
        }

        /// <summary>
        /// Adds new row to RacunElektraTemp
        /// </summary>
        /// <param name="brojRacuna">Broj računa</param>
        /// <param name="iznos">Iznos računa</param>
        /// <param name="date">Datum izdavanja</param>
        /// <param name="__guid">Guid // TODO: use UserID instead</param>
        /// <returns></returns>
        public async Task<IActionResult> AddNewTemp(string brojRacuna, string iznos, string date, string dopisId)
        {
            double _iznos;
            int _dopisId = int.Parse(dopisId);

            if (brojRacuna == null)
            {
                return Json(new { success = false, Message = "Broj računa je obavezan" });
            }
            if (date == null)
            {
                return Json(new { success = false, Message = "Datum izdavanja je obavezan" });
            }

            if (iznos != null)
            {
                _iznos = double.Parse(iznos);
                if (_iznos <= 0)
                {
                    return Json(new { success = false, Message = "Iznos mora biti veći od 0 kn" });
                }
            }
            else
            {
                return Json(new { success = false, Message = "Iznos je obavezan" });
            }

            racunElektraList = await _context.RacunElektra.ToListAsync();

            // TODO: ElektraKupacId
            RacunElektra re = new()
            {
                BrojRacuna = brojRacuna,
                Iznos = _iznos,
                DatumIzdavanja = DateTime.Parse(date),
                DopisId = _dopisId,
                CreatedByUserId = userId,
                IsItTemp = true,
            };
            
            re.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(re.BrojRacuna.Substring(0, 10)));
            racunElektraList.Add(re);

            int rbr = 1;
            foreach (RacunElektra e in racunElektraList)
            {
                e.RedniBroj = rbr++;
            }

            _ = _context.RacunElektra.Add(re);

            try
            {
                _ = _context.SaveChanges();
                return Json(new { success = true, Message = "Spremljeno" });

            }
            catch (DbUpdateException)
            {
                return Json(new { success = true, Message = "Greška" });
            }
        }

        /// <summary>
        /// Remove Row from RacunElektraTemp
        /// </summary>
        /// <param name="racunId">Id of RacunElektra</param>
        /// <returns>async Task<IActionResult></returns>
        public async Task<IActionResult> RemoveRow(string racunId)
        {
            int id = int.Parse(racunId);

            racunElektraList = await _context.RacunElektra.ToListAsync();
            RacunElektra RacunToRemove = _context.RacunElektra.FirstOrDefault(x => x.Id == id);
            _ = _context.RacunElektra.Remove(RacunToRemove);
            _ = RemoveAllFromDb(); // brisem iz temp tablice

            try
            {
                _ = _context.SaveChanges();
                return Json(new { success = true, Message = "Spremljeno" });

            }
            catch (DbUpdateException)
            {
                return Json(new { success = true, Message = "Greška" });
            }
        }

        /// <summary>
        /// TODO: remove, use UserID instead
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetGUID()
        {
            Guid guid = Guid.NewGuid();
            return Json(new { success = true, Message = guid.ToString() });

        }

        /// <summary>
        /// Checks if brojRacuna exists in ElektraRacuniTemp.
        /// </summary>
        /// <param name="brojRacuna">Broj računa</param>
        /// <returns>JsonResult</returns>
        public JsonResult CheckIfExists(string brojRacuna)
        {
            List<Racun> racunList = new();

            racunElektraList = _context.RacunElektra.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToList();
            racunList.AddRange(racunElektraList);
            return Racun.CheckIfExists(brojRacuna, racunList) ? Json(new { success = true, }) : Json(new { success = false, });
        }

        /// <summary>
        /// Checks if brojRacuna exists in already payed
        /// </summary>
        /// <param name="brojRacuna">Broj računa</param>
        /// <returns>JsonResult</returns>
        public JsonResult CheckIfExistsInPayed(string brojRacuna)
        {
            List<Racun> racunList = new();
            racunList.AddRange(racunElektraList);
            return Racun.CheckIfExistsInPayed(brojRacuna, racunList) ? Json(new { success = true, }) : Json(new { success = false, });
        }

        /// <summary>
        /// Used for Emptying RacunElektraTemp
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult RemoveAllFromDb()
        {

            racunElektraList = _context.RacunElektra.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToList();
            _context.RacunElektra.RemoveRange(racunElektraList);

            try
            {
                _ = _context.SaveChanges();
                return Json(new { success = true, Message = "Obrisano" });

            }
            catch (DbUpdateException)
            {
                return Json(new { success = true, Message = "Greška" });
            }
        }

    }
}
