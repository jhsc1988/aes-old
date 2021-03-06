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

namespace aes.Controllers
{
    public class RacunElektraIzvrsenjaUslugesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacunElektraIzvrsenjaUslugesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RacunElektraIzvrsenjaUsluges
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RacunElektraIzvrsenjeUsluge.Include(r => r.Dopis).Include(r => r.ElektraKupac);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RacunElektraIzvrsenjaUsluges/Details/5
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



        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća listu računa izvršenja usluge Elektre u JSON obliku za server side processing</returns>
        [HttpPost]
        public async Task<IActionResult> GetList()
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead (povećavam latency), ali proširujem scalability
            List<RacunElektraIzvrsenjeUsluge> RacunElektraIzvrsenjeUslugeList = new List<RacunElektraIzvrsenjeUsluge>();
            RacunElektraIzvrsenjeUslugeList = await _context.RacunElektraIzvrsenjeUsluge.ToListAsync<RacunElektraIzvrsenjeUsluge>();

            // popunjava podatke za JSON da mogu vezane podatke pregledavati u datatables
            foreach (RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge in RacunElektraIzvrsenjeUslugeList)
            {
                racunElektraIzvrsenjeUsluge.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.Id == racunElektraIzvrsenjeUsluge.ElektraKupacId);
            }

            // filter
            // TODO: fali napomena
            int totalRows = RacunElektraIzvrsenjeUslugeList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                RacunElektraIzvrsenjeUslugeList = await RacunElektraIzvrsenjeUslugeList.
                    Where(
                    x => x.BrojRacuna.Contains(searchValue)
                    || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                    || x.DatumIzdavanja.ToString("dd.MM.yyyy").Contains(searchValue)
                    || x.DatumIzvrsenja.ToString("dd.MM.yyyy").Contains(searchValue)
                    || (x.Usluga != null && x.Usluga.ToLower().Contains(searchValue.ToLower()))
                    || x.Iznos.ToString().Contains(searchValue)
                    || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                    || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue))).ToDynamicListAsync<RacunElektraIzvrsenjeUsluge>();
                    // x.DatumPotvrde.Value mi treba jer metoda nullable objekta ne prima argument za funkciju ToString
                    // sortiranje radi normalno za datume, neovisno o formatu ToString
            }
            int totalRowsAfterFiltering = RacunElektraIzvrsenjeUslugeList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            RacunElektraIzvrsenjeUslugeList = RacunElektraIzvrsenjeUslugeList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            RacunElektraIzvrsenjeUslugeList = RacunElektraIzvrsenjeUslugeList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<RacunElektraIzvrsenjeUsluge>();

            return Json(new { data = RacunElektraIzvrsenjeUslugeList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        // TODO: delete for production  !!!!
        // Area51
        [HttpGet]
        public async Task<IActionResult> GetListJSON()
        {
            List<RacunElektraIzvrsenjeUsluge> RacunElektraIzvrsenjeUslugeList = new List<RacunElektraIzvrsenjeUsluge>();
            RacunElektraIzvrsenjeUslugeList = await _context.RacunElektraIzvrsenjeUsluge.ToListAsync<RacunElektraIzvrsenjeUsluge>();

            foreach (RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge in RacunElektraIzvrsenjeUslugeList)
            {
                racunElektraIzvrsenjeUsluge.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.Id == racunElektraIzvrsenjeUsluge.ElektraKupacId);
            }
            return Json(RacunElektraIzvrsenjeUslugeList);
        }
    }
}
