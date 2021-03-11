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
    public class UgovoriOPrijenosuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UgovoriOPrijenosuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UgovoriOPrijenosu
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UgovorOPrijenosu.Include(u => u.Dopis).Include(u => u.DopisDostave).Include(u => u.UgovorOKoristenju);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UgovoriOPrijenosu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ugovorOPrijenosu = await _context.UgovorOPrijenosu
                .Include(u => u.Dopis)
                .Include(u => u.DopisDostave)
                .Include(u => u.UgovorOKoristenju)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ugovorOPrijenosu == null)
            {
                return NotFound();
            }

            return View(ugovorOPrijenosu);
        }

        // GET: UgovoriOPrijenosu/Create
        public IActionResult Create()
        {
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            ViewData["DopisDostaveId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            ViewData["UgovorOKoristenjuId"] = new SelectList(_context.UgovorOKoristenju, "Id", "BrojUgovora");
            return View();
        }

        // POST: UgovoriOPrijenosu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BrojUgovora,UgovorOKoristenjuId,DatumPrijenosa,DatumPotpisa,Kupac,KupacOIB,DopisId,RbrUgovora,DopisDostaveId,RbrDostave,VrijemeUnosa")] UgovorOPrijenosu ugovorOPrijenosu)
        {
            if (ModelState.IsValid)
            {
                ugovorOPrijenosu.VrijemeUnosa = DateTime.Now;
                _context.Add(ugovorOPrijenosu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", ugovorOPrijenosu.DopisId);
            ViewData["DopisDostaveId"] = new SelectList(_context.Dopis, "Id", "Urbroj", ugovorOPrijenosu.DopisDostaveId);
            ViewData["UgovorOKoristenjuId"] = new SelectList(_context.UgovorOKoristenju, "Id", "BrojUgovora", ugovorOPrijenosu.UgovorOKoristenjuId);
            return View(ugovorOPrijenosu);
        }

        // GET: UgovoriOPrijenosu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ugovorOPrijenosu = await _context.UgovorOPrijenosu.FindAsync(id);
            if (ugovorOPrijenosu == null)
            {
                return NotFound();
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", ugovorOPrijenosu.DopisId);
            ViewData["DopisDostaveId"] = new SelectList(_context.Dopis, "Id", "Urbroj", ugovorOPrijenosu.DopisDostaveId);
            ViewData["UgovorOKoristenjuId"] = new SelectList(_context.UgovorOKoristenju, "Id", "BrojUgovora", ugovorOPrijenosu.UgovorOKoristenjuId);
            return View(ugovorOPrijenosu);
        }

        // POST: UgovoriOPrijenosu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojUgovora,UgovorOKoristenjuId,DatumPrijenosa,DatumPotpisa,Kupac,KupacOIB,DopisId,RbrUgovora,DopisDostaveId,RbrDostave,VrijemeUnosa")] UgovorOPrijenosu ugovorOPrijenosu)
        {
            if (id != ugovorOPrijenosu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ugovorOPrijenosu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UgovorOPrijenosuExists(ugovorOPrijenosu.Id))
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
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", ugovorOPrijenosu.DopisId);
            ViewData["DopisDostaveId"] = new SelectList(_context.Dopis, "Id", "Urbroj", ugovorOPrijenosu.DopisDostaveId);
            ViewData["UgovorOKoristenjuId"] = new SelectList(_context.UgovorOKoristenju, "Id", "BrojUgovora", ugovorOPrijenosu.UgovorOKoristenjuId);
            return View(ugovorOPrijenosu);
        }

        // GET: UgovoriOPrijenosu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ugovorOPrijenosu = await _context.UgovorOPrijenosu
                .Include(u => u.Dopis)
                .Include(u => u.DopisDostave)
                .Include(u => u.UgovorOKoristenju)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ugovorOPrijenosu == null)
            {
                return NotFound();
            }

            return View(ugovorOPrijenosu);
        }

        // POST: UgovoriOPrijenosu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ugovorOPrijenosu = await _context.UgovorOPrijenosu.FindAsync(id);
            _context.UgovorOPrijenosu.Remove(ugovorOPrijenosu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UgovorOPrijenosuExists(int id)
        {
            return _context.UgovorOPrijenosu.Any(e => e.Id == id);
        }

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća listu ugovora o prijenosu u JSON obliku za server side processing</returns>
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
            List<UgovorOPrijenosu> UgovorOPrijenosuList = new List<UgovorOPrijenosu>();
            UgovorOPrijenosuList = await _context.UgovorOPrijenosu.ToListAsync<UgovorOPrijenosu>();

            // TODO: ugovor prijenosa
            // popunjava podatke za JSON da mogu vezane podatke pregledavati u datatables
            foreach (UgovorOPrijenosu ugovorOPrijenosu in UgovorOPrijenosuList)
            {
                ugovorOPrijenosu.Dopis = await _context.Dopis.FirstOrDefaultAsync(o => o.Id == ugovorOPrijenosu.DopisId);
                ugovorOPrijenosu.DopisDostave = await _context.Dopis.FirstOrDefaultAsync(o => o.Id == ugovorOPrijenosu.DopisDostaveId);
            }

            // filter
            int totalRows = UgovorOPrijenosuList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                UgovorOPrijenosuList = await UgovorOPrijenosuList.
                    Where(
                    x => x.BrojUgovora.Contains(searchValue)
                    || x.DatumPrijenosa.ToString().Contains(searchValue)
                    || x.DatumPotpisa.ToString().Contains(searchValue)
                    || x.Kupac.ToLower().Contains(searchValue.ToLower())
                    || x.KupacOIB.ToString().Contains(searchValue)
                    || x.Dopis.Predmet.Klasa.ToString().Contains(searchValue)
                    || x.RbrUgovora.ToString().Contains(searchValue)
                    || x.DopisDostave.Predmet.Klasa.ToString().Contains(searchValue)
                    || x.RbrDostave.ToString().Contains(searchValue)).ToDynamicListAsync<UgovorOPrijenosu>();
                    // sortiranje radi normalno za datume, neovisno o formatu ToString
            }
            int totalRowsAfterFiltering = UgovorOPrijenosuList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            UgovorOPrijenosuList = UgovorOPrijenosuList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            UgovorOPrijenosuList = UgovorOPrijenosuList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<UgovorOPrijenosu>();

            return Json(new { data = UgovorOPrijenosuList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        // TODO: delete for production  !!!!
        // Area51
        [HttpGet]
        public async Task<IActionResult> GetListJSON()
        {
            List<UgovorOPrijenosu> UgovorOPrijenosuList = new List<UgovorOPrijenosu>();
            UgovorOPrijenosuList = await _context.UgovorOPrijenosu.ToListAsync<UgovorOPrijenosu>();

            // TODO: ugovor prijenosa
            foreach (UgovorOPrijenosu ugovorOPrijenosu in UgovorOPrijenosuList)
            {
                ugovorOPrijenosu.Dopis = await _context.Dopis.FirstOrDefaultAsync(o => o.Id == ugovorOPrijenosu.DopisId);
                ugovorOPrijenosu.DopisDostave = await _context.Dopis.FirstOrDefaultAsync(o => o.Id == ugovorOPrijenosu.DopisDostaveId);
            }
            return Json(UgovorOPrijenosuList);
        }
    }
}
