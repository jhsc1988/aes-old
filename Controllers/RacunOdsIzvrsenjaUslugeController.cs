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
using Microsoft.AspNetCore.Authorization;

namespace aes.Controllers
{
    public class RacunOdsIzvrsenjaUslugeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacunOdsIzvrsenjaUslugeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RacunOdsIzvrsenjaUsluge
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.RacunOdsIzvrsenjaUsluge.Include(r => r.Dopis).Include(r => r.OdsKupac);
        //    return View(await applicationDbContext.ToListAsync());
        //}        
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: RacunOdsIzvrsenjaUsluge/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunOdsIzvrsenjaUsluge = await _context.RacunOdsIzvrsenjaUsluge
                .Include(r => r.Dopis)
                .Include(r => r.OdsKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunOdsIzvrsenjaUsluge == null)
            {
                return NotFound();
            }

            return View(racunOdsIzvrsenjaUsluge);
        }

        // GET: RacunOdsIzvrsenjaUsluge/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            ViewData["OdsKupacId"] = new SelectList(_context.OdsKupac, "Id", "Id");
            return View();
        }

        // POST: RacunOdsIzvrsenjaUsluge/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,OdsKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunOdsIzvrsenjaUsluge racunOdsIzvrsenjaUsluge)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racunOdsIzvrsenjaUsluge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunOdsIzvrsenjaUsluge.DopisId);
            ViewData["OdsKupacId"] = new SelectList(_context.OdsKupac, "Id", "Id", racunOdsIzvrsenjaUsluge.OdsKupacId);
            return View(racunOdsIzvrsenjaUsluge);
        }

        // GET: RacunOdsIzvrsenjaUsluge/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunOdsIzvrsenjaUsluge = await _context.RacunOdsIzvrsenjaUsluge.FindAsync(id);
            if (racunOdsIzvrsenjaUsluge == null)
            {
                return NotFound();
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunOdsIzvrsenjaUsluge.DopisId);
            ViewData["OdsKupacId"] = new SelectList(_context.OdsKupac, "Id", "Id", racunOdsIzvrsenjaUsluge.OdsKupacId);
            return View(racunOdsIzvrsenjaUsluge);
        }

        // POST: RacunOdsIzvrsenjaUsluge/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,OdsKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunOdsIzvrsenjaUsluge racunOdsIzvrsenjaUsluge)
        {
            if (id != racunOdsIzvrsenjaUsluge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racunOdsIzvrsenjaUsluge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunOdsIzvrsenjaUslugeExists(racunOdsIzvrsenjaUsluge.Id))
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
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunOdsIzvrsenjaUsluge.DopisId);
            ViewData["OdsKupacId"] = new SelectList(_context.OdsKupac, "Id", "Id", racunOdsIzvrsenjaUsluge.OdsKupacId);
            return View(racunOdsIzvrsenjaUsluge);
        }

        // GET: RacunOdsIzvrsenjaUsluge/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunOdsIzvrsenjaUsluge = await _context.RacunOdsIzvrsenjaUsluge
                .Include(r => r.Dopis)
                .Include(r => r.OdsKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunOdsIzvrsenjaUsluge == null)
            {
                return NotFound();
            }

            return View(racunOdsIzvrsenjaUsluge);
        }

        // POST: RacunOdsIzvrsenjaUsluge/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var racunOdsIzvrsenjaUsluge = await _context.RacunOdsIzvrsenjaUsluge.FindAsync(id);
            _context.RacunOdsIzvrsenjaUsluge.Remove(racunOdsIzvrsenjaUsluge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunOdsIzvrsenjaUslugeExists(int id)
        {
            return _context.RacunOdsIzvrsenjaUsluge.Any(e => e.Id == id);
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
        /// <returns>Vraća listu racuna ODS - izvrsenje usluge u JSON obliku za server side processing</returns>
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
            List<RacunOdsIzvrsenjaUsluge> RacunOdsIzvrsenjaUslugeList = new List<RacunOdsIzvrsenjaUsluge>();
            RacunOdsIzvrsenjaUslugeList = await _context.RacunOdsIzvrsenjaUsluge.ToListAsync<RacunOdsIzvrsenjaUsluge>();

            // popunjava podatke za JSON da mogu vezane podatke pregledavati u datatables
            foreach (RacunOdsIzvrsenjaUsluge racunOdsIzvrsenjaUsluge in RacunOdsIzvrsenjaUslugeList)
            {
                racunOdsIzvrsenjaUsluge.OdsKupac = await _context.OdsKupac.FirstOrDefaultAsync(o => o.Id == racunOdsIzvrsenjaUsluge.OdsKupacId);
            }

            // filter
            int totalRows = RacunOdsIzvrsenjaUslugeList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                RacunOdsIzvrsenjaUslugeList = await RacunOdsIzvrsenjaUslugeList.
                    Where(
                    x => x.BrojRacuna.Contains(searchValue)
                    || x.OdsKupac.SifraKupca.ToString().Contains(searchValue)
                    || x.DatumIzdavanja.ToString("dd.MM.yyyy").Contains(searchValue)
                    || x.DatumIzvrsenja.ToString("dd.MM.yyyy").Contains(searchValue)
                    || (x.Usluga != null && x.Usluga.ToLower().Contains(searchValue.ToLower()))
                    || x.Iznos.ToString().Contains(searchValue)
                    || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                    || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue))).ToDynamicListAsync<RacunOdsIzvrsenjaUsluge>();
                    // x.DatumPotvrde.Value mi treba jer metoda nullable objekta ne prima argument za funkciju ToString
                    // sortiranje radi normalno za datume, neovisno o formatu ToString
            }
            int totalRowsAfterFiltering = RacunOdsIzvrsenjaUslugeList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            RacunOdsIzvrsenjaUslugeList = RacunOdsIzvrsenjaUslugeList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            RacunOdsIzvrsenjaUslugeList = RacunOdsIzvrsenjaUslugeList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<RacunOdsIzvrsenjaUsluge>();

            return Json(new { data = RacunOdsIzvrsenjaUslugeList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        // TODO: delete for production  !!!!
        // Area51
        [HttpGet]
        public async Task<IActionResult> GetListJSON()
        {
            List<RacunOdsIzvrsenjaUsluge> RacunOdsIzvrsenjaUslugeList = new List<RacunOdsIzvrsenjaUsluge>();
            RacunOdsIzvrsenjaUslugeList = await _context.RacunOdsIzvrsenjaUsluge.ToListAsync<RacunOdsIzvrsenjaUsluge>();

            foreach (RacunOdsIzvrsenjaUsluge racunOdsIzvrsenjaUsluge in RacunOdsIzvrsenjaUslugeList)
            {
                racunOdsIzvrsenjaUsluge.OdsKupac = await _context.OdsKupac.FirstOrDefaultAsync(o => o.Id == racunOdsIzvrsenjaUsluge.OdsKupacId);
            }
            return Json(RacunOdsIzvrsenjaUslugeList);
        }
    }

}
