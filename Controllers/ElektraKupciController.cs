using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class ElektraKupciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ElektraKupciController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ElektraKupci
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.ElektraKupac.Include(e => e.Ods);
        //    return View(await applicationDbContext.ToListAsync());
        //}   
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: ElektraKupci/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elektraKupac = await _context.ElektraKupac
                .Include(e => e.Ods)
                .Include(e => e.Ods.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (elektraKupac == null)
            {
                return NotFound();
            }

            return View(elektraKupac);
        }

        // GET: ElektraKupci/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id");
            return View();
        }

        // POST: ElektraKupci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,UgovorniRacun,OdsId,Napomena,VrijemeUnosa")] ElektraKupac elektraKupac)
        {
            if (ModelState.IsValid)
            {
                elektraKupac.VrijemeUnosa = DateTime.Now;
                _context.Add(elektraKupac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // GET: ElektraKupci/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elektraKupac = await _context.ElektraKupac.FindAsync(id);
            elektraKupac.Ods = _context.Ods.FirstOrDefault(e => e.Id == elektraKupac.OdsId);
            elektraKupac.Ods.Stan = _context.Stan.FirstOrDefault(e => e.StanId == elektraKupac.Ods.StanId);
            if (elektraKupac == null)
            {
                return NotFound();
            }
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // POST: ElektraKupci/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UgovorniRacun,OdsId,Napomena,VrijemeUnosa")] ElektraKupac elektraKupac)
        {
            if (id != elektraKupac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(elektraKupac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElektraKupacExists(elektraKupac.Id))
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
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // GET: ElektraKupci/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elektraKupac = await _context.ElektraKupac
                .Include(e => e.Ods)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (elektraKupac == null)
            {
                return NotFound();
            }

            return View(elektraKupac);
        }

        // POST: ElektraKupci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var elektraKupac = await _context.ElektraKupac.FindAsync(id);
            _context.ElektraKupac.Remove(elektraKupac);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElektraKupacExists(int id)
        {
            return _context.ElektraKupac.Any(e => e.Id == id);
        }

        // validation
        [HttpGet]
        public async Task<IActionResult> UgovorniRacunValidation(long ugovorniRacun)
        {
            if (ugovorniRacun < 1000000000 || ugovorniRacun > 9999999999)
            {
                return Json($"Ugovorni račun nije ispravan");
            }

            // TODO: dodati uvjet za omm - vjerojatno mi treba i unique constraint
            // vidjeti i kod ostalih controllera
            var db = await _context.ElektraKupac.FirstOrDefaultAsync(x => x.UgovorniRacun == ugovorniRacun);
            if (db != null)
            {
                return Json($"Ugovorni račun {ugovorniRacun} već postoji."); // TODO: isti kupac se teoretski moze pojaviti na drugom omm
                // TODO: treba staviti nekakav alert da već postoji
            }
            return Json(true);
        }

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća listu kupaca Elektre u JSON obliku za server side processing</returns>
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
            List<ElektraKupac> ElektraKupacList = new List<ElektraKupac>();
            ElektraKupacList = await _context.ElektraKupac.ToListAsync<ElektraKupac>();

            // popunjava podatke za JSON da mogu vezane podatke pregledavati u datatables
            foreach (ElektraKupac elektraKupac in ElektraKupacList)
            {
                elektraKupac.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == elektraKupac.OdsId); // kod mene je elektraKupac.OdsId -> Ods.Id (primarni ključ)
                elektraKupac.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == elektraKupac.Ods.StanId); // hoću podatke o stanu za svaki omm, pretražuje po PK
            }

            // filter
            int totalRows = ElektraKupacList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                ElektraKupacList = await ElektraKupacList.
                    Where(
                    x => x.UgovorniRacun.ToString().Contains(searchValue)
                    || x.Ods.Omm.ToString().Contains(searchValue)
                    || x.Ods.Stan.StanId.ToString().Contains(searchValue)
                    || x.Ods.Stan.SifraObjekta.ToString().Contains(searchValue)
                    || (x.Ods.Stan.Adresa != null && x.Ods.Stan.Adresa.ToLower().Contains(searchValue.ToLower()))
                    || (x.Ods.Stan.Kat != null && x.Ods.Stan.Kat.ToLower().Contains(searchValue.ToLower()))
                    || (x.Ods.Stan.BrojSTana != null && x.Ods.Stan.BrojSTana.ToLower().Contains(searchValue.ToLower()))
                    || (x.Ods.Stan.Četvrt != null && x.Ods.Stan.Četvrt.ToLower().Contains(searchValue.ToLower()))
                    || x.Ods.Stan.Površina.ToString().Contains(searchValue)
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))).ToDynamicListAsync<ElektraKupac>();
            }
            int totalRowsAfterFiltering = ElektraKupacList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            ElektraKupacList = ElektraKupacList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            ElektraKupacList = ElektraKupacList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<ElektraKupac>();

            return Json(new { data = ElektraKupacList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }




        public async Task<IActionResult> GetRacuniForKupac(int param)
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead, ali proširujem scalability
            List<RacunElektra> RacunElektraList = new List<RacunElektra>();
            RacunElektraList = await _context.RacunElektra.Where(e => e.ElektraKupac.Id == param).ToListAsync();

            foreach (var p in RacunElektraList)
                p.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(e => e.Id == p.ElektraKupacId);

            // popunjava podatke za JSON da mogu vezane podatke pregledavati u datatables
            foreach (RacunElektra e in RacunElektraList)
            {
                e.ElektraKupac.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.OdsId); // kod mene je elektraKupac.OdsId -> Ods.Id (primarni ključ)
                e.ElektraKupac.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.Ods.StanId); // hoću podatke o stanu za svaki omm, pretražuje po PK
            }

            // filter
            int totalRows = RacunElektraList.Count;
            if (!string.IsNullOrEmpty(searchValue))
                RacunElektraList = await RacunElektraList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue)
                             || x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue)
                             || x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))
                    .ToDynamicListAsync<RacunElektra>();

            int totalRowsAfterFiltering = RacunElektraList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            RacunElektraList = RacunElektraList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            RacunElektraList = RacunElektraList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<RacunElektra>();

            return Json(new { data = RacunElektraList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }




        public async Task<IActionResult> GetRacuniRateForKupac(int param)
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead, ali proširujem scalability
            List<RacunElektraRate> RacunElektraRateList = new List<RacunElektraRate>();
            RacunElektraRateList = await _context.RacunElektraRate.Where(e => e.ElektraKupac.Id == param).ToListAsync();

            foreach (var p in RacunElektraRateList)
                p.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(e => e.Id == p.ElektraKupacId);

            // popunjava podatke za JSON da mogu vezane podatke pregledavati u datatables
            foreach (RacunElektraRate e in RacunElektraRateList)
            {
                e.ElektraKupac.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.OdsId); // kod mene je elektraKupac.OdsId -> Ods.Id (primarni ključ)
                e.ElektraKupac.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.Ods.StanId); // hoću podatke o stanu za svaki omm, pretražuje po PK
            }

            // filter
            int totalRows = RacunElektraRateList.Count;
            if (!string.IsNullOrEmpty(searchValue))
                RacunElektraRateList = await RacunElektraRateList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue)
                             || x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue)
                             || x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))
                    .ToDynamicListAsync<RacunElektraRate>();

            int totalRowsAfterFiltering = RacunElektraRateList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            RacunElektraRateList = RacunElektraRateList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            RacunElektraRateList = RacunElektraRateList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<RacunElektraRate>();

            return Json(new { data = RacunElektraRateList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }




        public async Task<IActionResult> GetRacuniElektraIzvrsenjeForKupac(int param)
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request
                .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead, ali proširujem scalability
            List<RacunElektraIzvrsenjeUsluge> RacunElektraIzvrsenjeList = new List<RacunElektraIzvrsenjeUsluge>();
            RacunElektraIzvrsenjeList = await _context.RacunElektraIzvrsenjeUsluge.Where(e => e.ElektraKupac.Id == param).ToListAsync();

            foreach (var p in RacunElektraIzvrsenjeList)
                p.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(e => e.Id == p.ElektraKupacId);

            // popunjava podatke za JSON da mogu vezane podatke pregledavati u datatables
            foreach (RacunElektraIzvrsenjeUsluge e in RacunElektraIzvrsenjeList)
            {
                e.ElektraKupac.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.OdsId); // kod mene je elektraKupac.OdsId -> Ods.Id (primarni ključ)
                e.ElektraKupac.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.Ods.StanId); // hoću podatke o stanu za svaki omm, pretražuje po PK
            }

            // filter
            var totalRows = RacunElektraIzvrsenjeList.Count;
            if (!string.IsNullOrEmpty(searchValue))
                RacunElektraIzvrsenjeList = await RacunElektraIzvrsenjeList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.DatumIzvrsenja.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Usluga != null && x.Usluga.ToLower().Contains(searchValue.ToLower())
                             || x.Iznos.ToString().Contains(searchValue)
                             || x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue)
                             || x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Napomena != null && x.Napomena.ToLower().Contains(searchValue))
                    .ToDynamicListAsync<RacunElektraIzvrsenjeUsluge>();

            var totalRowsAfterFiltering = RacunElektraIzvrsenjeList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            RacunElektraIzvrsenjeList = RacunElektraIzvrsenjeList.AsQueryable()
                .OrderBy(sortColumnName + " " + sortDirection)
                .ToList();

            // paging
            RacunElektraIzvrsenjeList =
                RacunElektraIzvrsenjeList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = RacunElektraIzvrsenjeList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }
    }
}
