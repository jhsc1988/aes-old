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
        private List<ElektraKupac> ElektraKupacList;
        private List<RacunElektra> RacunElektraList;
        private List<RacunElektraRate> RacunElektraRateList;
        private List<RacunElektraIzvrsenjeUsluge> RacunElektraIzvrsenjeList;

        /// <summary>
        /// datatables params
        /// </summary>
        private string start, length, searchValue, sortColumnName, sortDirection;

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

            ElektraKupac elektraKupac = await _context.ElektraKupac
                .Include(e => e.Ods)
                .Include(e => e.Ods.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            return elektraKupac == null ? NotFound() : View(elektraKupac);
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
                _ = _context.Add(elektraKupac);
                _ = await _context.SaveChangesAsync();
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

            ElektraKupac elektraKupac = await _context.ElektraKupac.FindAsync(id);
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
                    _ = _context.Update(elektraKupac);
                    _ = await _context.SaveChangesAsync();
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

            ElektraKupac elektraKupac = await _context.ElektraKupac
                .Include(e => e.Ods)
                .FirstOrDefaultAsync(m => m.Id == id);
            return elektraKupac == null ? NotFound() : View(elektraKupac);
        }

        // POST: ElektraKupci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ElektraKupac elektraKupac = await _context.ElektraKupac.FindAsync(id);
            _ = _context.ElektraKupac.Remove(elektraKupac);
            _ = await _context.SaveChangesAsync();
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
            if (ugovorniRacun is < 1000000000 or > 9999999999)
            {
                return Json($"Ugovorni račun nije ispravan");
            }

            // TODO: dodati uvjet za omm - vjerojatno mi treba i unique constraint
            // vidjeti i kod ostalih controllera
            ElektraKupac db = await _context.ElektraKupac.FirstOrDefaultAsync(x => x.UgovorniRacun == ugovorniRacun);
            if (db != null)
            {
                return Json($"Ugovorni račun {ugovorniRacun} već postoji."); // TODO: isti kupac se teoretski moze pojaviti na drugom omm
                // TODO: treba staviti nekakav alert da već postoji
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

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća listu kupaca Elektre u JSON obliku za server side processing</returns>
        [HttpPost]
        public async Task<IActionResult> GetList()
        {
            GetDatatablesParamas();

            ElektraKupacList = await _context.ElektraKupac.ToListAsync();

            foreach (ElektraKupac elektraKupac in ElektraKupacList)
            {
                elektraKupac.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == elektraKupac.OdsId); // kod mene je elektraKupac.OdsId -> Ods.Id (primarni ključ)
                elektraKupac.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == elektraKupac.Ods.StanId); // hoću podatke o stanu za svaki omm, pretražuje po PK
            }

            int totalRows = ElektraKupacList.Count;
            if (!string.IsNullOrEmpty(searchValue)) // filter
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

            ElektraKupacList = ElektraKupacList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            ElektraKupacList = ElektraKupacList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new { data = ElektraKupacList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }




        public async Task<IActionResult> GetRacuniForKupac(int param)
        {
            GetDatatablesParamas();

            RacunElektraList = await _context.RacunElektra.Where(e => e.ElektraKupac.Id == param).ToListAsync();

            foreach (RacunElektra p in RacunElektraList)
            {
                p.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(e => e.Id == p.ElektraKupacId);
            }

            foreach (RacunElektra e in RacunElektraList)
            {
                e.ElektraKupac.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.OdsId);
                e.ElektraKupac.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.Ods.StanId);
            }


            int totalRows = RacunElektraList.Count;
            if (!string.IsNullOrEmpty(searchValue)) // filter
            {
                RacunElektraList = await RacunElektraList.Where(
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

            int totalRowsAfterFiltering = RacunElektraList.Count;

            RacunElektraList = RacunElektraList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            RacunElektraList = RacunElektraList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new
            {
                data = RacunElektraList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }




        public async Task<IActionResult> GetRacuniRateForKupac(int param)
        {
            GetDatatablesParamas();

            RacunElektraRateList = await _context.RacunElektraRate.Where(e => e.ElektraKupac.Id == param).ToListAsync();

            foreach (RacunElektraRate p in RacunElektraRateList)
            {
                p.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(e => e.Id == p.ElektraKupacId);
            }

            foreach (RacunElektraRate e in RacunElektraRateList)
            {
                e.ElektraKupac.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.OdsId);
                e.ElektraKupac.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.Ods.StanId);
            }


            int totalRows = RacunElektraRateList.Count;
            if (!string.IsNullOrEmpty(searchValue)) // filter
            {
                RacunElektraRateList = await RacunElektraRateList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue)
                             || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                             || (x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                             || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower())))
                    .ToDynamicListAsync<RacunElektraRate>();
            }

            int totalRowsAfterFiltering = RacunElektraRateList.Count;

            RacunElektraRateList = RacunElektraRateList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            RacunElektraRateList = RacunElektraRateList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new
            {
                data = RacunElektraRateList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }


        public async Task<IActionResult> GetRacuniElektraIzvrsenjeForKupac(int param)
        {
            GetDatatablesParamas();

            RacunElektraIzvrsenjeList = await _context.RacunElektraIzvrsenjeUsluge.Where(e => e.ElektraKupac.Id == param).ToListAsync();

            foreach (RacunElektraIzvrsenjeUsluge p in RacunElektraIzvrsenjeList)
            {
                p.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(e => e.Id == p.ElektraKupacId);
            }

            foreach (RacunElektraIzvrsenjeUsluge e in RacunElektraIzvrsenjeList)
            {
                e.ElektraKupac.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.OdsId);
                e.ElektraKupac.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == e.ElektraKupac.Ods.StanId);
            }

            int totalRows = RacunElektraIzvrsenjeList.Count;
            if (!string.IsNullOrEmpty(searchValue)) // filter
            {
                RacunElektraIzvrsenjeList = await RacunElektraIzvrsenjeList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.DatumIzvrsenja.ToString("dd.MM.yyyy").Contains(searchValue)
                             || (x.Usluga != null && x.Usluga.ToLower().Contains(searchValue.ToLower()))
                             || x.Iznos.ToString().Contains(searchValue)
                             || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                             || (x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                             || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue)))
                    .ToDynamicListAsync<RacunElektraIzvrsenjeUsluge>();
            }

            int totalRowsAfterFiltering = RacunElektraIzvrsenjeList.Count;

            RacunElektraIzvrsenjeList = RacunElektraIzvrsenjeList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            RacunElektraIzvrsenjeList = RacunElektraIzvrsenjeList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

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
