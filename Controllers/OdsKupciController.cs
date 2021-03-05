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
    public class OdsKupciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OdsKupciController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OdsKupci
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OdsKupac.Include(o => o.Ods);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OdsKupci/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odsKupac = await _context.OdsKupac
                .Include(o => o.Ods)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (odsKupac == null)
            {
                return NotFound();
            }

            return View(odsKupac);
        }

        // GET: OdsKupci/Create
        public IActionResult Create()
        {
            //ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id");
            return View();
        }

        // POST: OdsKupci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SifraKupca,OdsId,Napomena,VrijemeUnosa")] OdsKupac odsKupac)
        {
            if (ModelState.IsValid)
            {
                odsKupac.VrijemeUnosa = DateTime.Now;
                _context.Add(odsKupac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", odsKupac.OdsId);
            return View(odsKupac);
        }

        // GET: OdsKupci/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odsKupac = await _context.OdsKupac.FindAsync(id);
            if (odsKupac == null)
            {
                return NotFound();
            }
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", odsKupac.OdsId);
            return View(odsKupac);
        }

        // POST: OdsKupci/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SifraKupca,OdsId,Napomena,VrijemeUnosa")] OdsKupac odsKupac)
        {
            if (id != odsKupac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(odsKupac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OdsKupacExists(odsKupac.Id))
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
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", odsKupac.OdsId);
            return View(odsKupac);
        }

        // GET: OdsKupci/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odsKupac = await _context.OdsKupac
                .Include(o => o.Ods)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (odsKupac == null)
            {
                return NotFound();
            }

            return View(odsKupac);
        }

        // POST: OdsKupci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var odsKupac = await _context.OdsKupac.FindAsync(id);
            _context.OdsKupac.Remove(odsKupac);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OdsKupacExists(int id)
        {
            return _context.OdsKupac.Any(e => e.Id == id);
        }

        // validation
        [HttpGet]
        public async Task<IActionResult> SifraKupcaValidation(int sifraKupca)
        {
            if (sifraKupca < 10000000 || sifraKupca > 99999999)
            {
                return Json($"Šifra kupca nije ispravna");
            }

            // TODO: dodati uvjet za omm - vjerojatno mi treba i unique constraint
            // vidjeti i kod ostalih controllera
            var db = await _context.OdsKupac.FirstOrDefaultAsync(x => x.SifraKupca == sifraKupca);
            if (db != null)
            {
                return Json($"Šifra kupca {sifraKupca} već postoji."); // TODO: isti kupac se teoretski moze pojaviti na drugom omm
            }
            return Json(true);
        }

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća listu ODS kupaca u JSON obliku za server side processing</returns>
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
            List<OdsKupac> OdsKupacList = new List<OdsKupac>();
            OdsKupacList = await _context.OdsKupac.ToListAsync<OdsKupac>();

            // popunjava podatke za JSON da mogu vezane podatke pregledavati u datatables
            foreach (OdsKupac odsKupac in OdsKupacList)
            {
                odsKupac.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == odsKupac.OdsId); // kod mene je odsKupac.OdsId -> Ods.Id (primarni ključ)
                odsKupac.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == odsKupac.Ods.StanId); // hoću podatke o stanu za svaki omm, pretražuje po PK
            }

            // filter
            int totalRows = OdsKupacList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                OdsKupacList = await OdsKupacList.
                    Where(
                    x => x.SifraKupca.ToString().Contains(searchValue.ToLower())
                    || x.Ods.Omm.ToString().Contains(searchValue.ToLower())
                    || x.Ods.Stan.StanId.ToString().Contains(searchValue.ToLower())
                    || x.Ods.Stan.SifraObjekta.ToString().Contains(searchValue.ToLower())
                    || (x.Ods.Stan.Adresa != null && x.Ods.Stan.Adresa.ToLower().Contains(searchValue.ToLower()))
                    || (x.Ods.Stan.Kat != null && x.Ods.Stan.Kat.ToLower().Contains(searchValue.ToLower()))
                    || (x.Ods.Stan.BrojSTana != null && x.Ods.Stan.BrojSTana.ToLower().Contains(searchValue.ToLower()))
                    || (x.Ods.Stan.Četvrt != null && x.Ods.Stan.Četvrt.ToLower().Contains(searchValue.ToLower()))
                    || x.Ods.Stan.Površina.ToString().Contains(searchValue.ToLower())
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))).ToDynamicListAsync<OdsKupac>();
            }
            int totalRowsAfterFiltering = OdsKupacList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            OdsKupacList = OdsKupacList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            OdsKupacList = OdsKupacList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<OdsKupac>();

            return Json(new { data = OdsKupacList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }
    }
}
