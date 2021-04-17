using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aes.Data;
using aes.Models;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;

namespace aes.Controllers
{
    public class RacuniElektraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacuniElektraController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RacuniElektra
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.RacunElektra.Include(r => r.Dopis).Include(r => r.ElektraKupac);
        //    return View(await applicationDbContext.ToListAsync());
        //}        
        public IActionResult Index()
        {
            return View();
        }

        // GET: RacuniElektra/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektra = await _context.RacunElektra
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektra == null)
            {
                return NotFound();
            }

            return View(racunElektra);
        }

        // GET: RacuniElektra/Create
        public async Task<IActionResult> CreateAsync()
        {
            //ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            //ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id");

            List<RacunElektra> re = new List<RacunElektra>();

            var applicationDbContext = await _context.RacunElektra.ToListAsync();

            {
                ViewBag.Predmeti = new List<Predmet>();
                foreach (Predmet element in await _context.Predmet.ToListAsync())
                    ViewBag.Predmeti.Add(element);
                ViewBag.Predmeti = JsonConvert.SerializeObject(ViewBag.Predmeti);
            }

            {
                ViewBag.Dopisi = new List<Dopis>();
                foreach (Dopis element in await _context.Dopis.ToListAsync())
                    ViewBag.Dopisi.Add(element);
                ViewBag.Dopisi = JsonConvert.SerializeObject(ViewBag.Dopisi);
            }
            {
                ViewBag.Kupci = new List<ElektraKupac>();
                foreach (ElektraKupac element in await _context.ElektraKupac.ToListAsync())
                    ViewBag.Kupci.Add(element);
            }

            {
                foreach (ElektraKupac element in ViewBag.Kupci)
                {
                    element.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == element.OdsId);
                }
                foreach (ElektraKupac element in ViewBag.Kupci)
                    element.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == element.Ods.StanId);

                ViewBag.Kupci = JsonConvert.SerializeObject(ViewBag.Kupci);
            }
            return View(applicationDbContext);
        }

        // POST: RacuniElektra/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunElektra racunElektra)
        {
            // ModelState debbuger:
            // var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                racunElektra.VrijemeUnosa = DateTime.Now;
                _context.Add(racunElektra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektra.DopisId);
            //ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektra.ElektraKupacId);

            return View(racunElektra);
        }

        //public JsonResult GetPredmeti(int? predmetId, int? dopisId)
        //{
        //    ViewBag.Predmeti = new List<SelectListItem>();
        //    foreach (Predmet elementi in _context.Predmet.ToList())
        //        ViewBag.Predmeti.Add(new SelectListItem { Text = elementi.Klasa, Value = elementi.Id.ToString() });

        //}

        // GET: RacuniElektra/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektra = await _context.RacunElektra.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunElektra racunElektra)
        {
            if (id != racunElektra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racunElektra);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektra = await _context.RacunElektra
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektra == null)
            {
                return NotFound();
            }

            return View(racunElektra);
        }

        // POST: RacuniElektra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var racunElektra = await _context.RacunElektra.FindAsync(id);
            _context.RacunElektra.Remove(racunElektra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraExists(int id)
        {
            return _context.RacunElektra.Any(e => e.Id == id);
        }

        // validation
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {
            if (brojRacuna.Length < 19 || brojRacuna.Length > 19)
            {
                return Json($"Broj računa nije ispravan");
            }

            var db = await _context.RacunElektra.FirstOrDefaultAsync(x => x.BrojRacuna.Equals(brojRacuna));
            if (db != null)
            {
                return Json($"Račun {brojRacuna} već postoji.");
            }
            return Json(true);
        }

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća listu racuna Elektre u JSON obliku za server side processing</returns>
        [HttpPost]
        public async Task<IActionResult> GetList()
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead, ali proširujem scalability
            List<RacunElektra> RacunElektraList = new List<RacunElektra>();
            RacunElektraList = await _context.RacunElektra.ToListAsync<RacunElektra>();

            // popunjava podatke za JSON da mogu vezane podatke pregledavati u datatables
            foreach (RacunElektra racunElektra in RacunElektraList)
            {
                racunElektra.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.Id == racunElektra.ElektraKupacId);
            }
            
            // filter
            int totalRows = RacunElektraList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                RacunElektraList = await RacunElektraList.
                    Where(
                    x => x.BrojRacuna.Contains(searchValue)
                    || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                    || x.DatumIzdavanja.ToString("dd.MM.yyyy").Contains(searchValue)
                    || x.Iznos.ToString().Contains(searchValue)
                    || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                    || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))).ToDynamicListAsync<RacunElektra>();
                    // x.DatumPotvrde.Value mi treba jer metoda nullable objekta ne prima argument za funkciju ToString
                    // sortiranje radi normalno za datume, neovisno o formatu ToString
            }
            int totalRowsAfterFiltering = RacunElektraList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            RacunElektraList = RacunElektraList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            RacunElektraList = RacunElektraList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<RacunElektra>();

            return Json(new { data = RacunElektraList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public JsonResult SaveToDB([FromBody] List<RacunElektra> p)
        {

            RacunElektra racuni = new RacunElektra();

            {
                //Truncate Table to delete all old records.
                //entities.Database.ExecuteSqlCommand("TRUNCATE TABLE [Customers]");

                //Check for NULL.
                //if (racuniList == null)
                //{
                //    racuniList = new List<RacunElektra>();
                //}

                //Loop and insert records.
                //foreach (RacunElektra r in racuniList)
                //{
                //    _context.RacunElektra.Add(racuni);
                //}
                 _context.SaveChangesAsync();

                //int insertedRecords = re.SaveChanges();
                return Json("uspjes no spremljeno");
            }
        }











        // TODO: delete for production  !!!!
        // Area51
        [HttpGet]
        public async Task<IActionResult> GetListJSON()
        {
            List<RacunElektra> RacunElektraList = new List<RacunElektra>();
            RacunElektraList = await _context.RacunElektra.ToListAsync<RacunElektra>();

            var applicationDbContext = _context.RacunElektra.
                Include(r => r.Dopis).
                Include(r => r.ElektraKupac).
                Include(r => r.ElektraKupac.Ods).
                Include(r => r.ElektraKupac.Ods.Stan);

            foreach (RacunElektra racunElektra in RacunElektraList)
            {
                racunElektra.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.Id == racunElektra.ElektraKupacId);
            }
            return Json(applicationDbContext.ToList());
        }







    }
}
