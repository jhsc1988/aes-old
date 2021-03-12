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
    public class UgovoriOKoristenjuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UgovoriOKoristenjuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UgovoriOKoristenju
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.UgovorOKoristenju.Include(u => u.Dopis).Include(u => u.DopisDostave).Include(u => u.Ods);
        //    return View(await applicationDbContext.ToListAsync());
        //}        
       
        public IActionResult Index()
        {
            return View();
        }

        // GET: UgovoriOKoristenju/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ugovorOKoristenju = await _context.UgovorOKoristenju
                .Include(u => u.Dopis)
                .Include(u => u.DopisDostave)
                .Include(u => u.Ods)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ugovorOKoristenju == null)
            {
                return NotFound();
            }

            return View(ugovorOKoristenju);
        }

        // GET: UgovoriOKoristenju/Create
        public IActionResult Create()
        {
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            ViewData["DopisDostaveId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id");
            return View();
        }

        // POST: UgovoriOKoristenju/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BrojUgovora,OdsId,DatumPotpisaHEP,DatumPotpisaGZ,DopisId,RbrUgovora,DopisDostaveId,RbrDostave,VrijemeUnosa")] UgovorOKoristenju ugovorOKoristenju)
        {
            if (ModelState.IsValid)
            {
                ugovorOKoristenju.VrijemeUnosa = DateTime.Now;
                _context.Add(ugovorOKoristenju);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", ugovorOKoristenju.DopisId);
            ViewData["DopisDostaveId"] = new SelectList(_context.Dopis, "Id", "Urbroj", ugovorOKoristenju.DopisDostaveId);
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", ugovorOKoristenju.OdsId);
            return View(ugovorOKoristenju);
        }

        // GET: UgovoriOKoristenju/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ugovorOKoristenju = await _context.UgovorOKoristenju.FindAsync(id);
            if (ugovorOKoristenju == null)
            {
                return NotFound();
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", ugovorOKoristenju.DopisId);
            ViewData["DopisDostaveId"] = new SelectList(_context.Dopis, "Id", "Urbroj", ugovorOKoristenju.DopisDostaveId);
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", ugovorOKoristenju.OdsId);
            return View(ugovorOKoristenju);
        }

        // POST: UgovoriOKoristenju/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojUgovora,OdsId,DatumPotpisaHEP,DatumPotpisaGZ,DopisId,RbrUgovora,DopisDostaveId,RbrDostave,VrijemeUnosa")] UgovorOKoristenju ugovorOKoristenju)
        {
            if (id != ugovorOKoristenju.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ugovorOKoristenju);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UgovorOKoristenjuExists(ugovorOKoristenju.Id))
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
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", ugovorOKoristenju.DopisId);
            ViewData["DopisDostaveId"] = new SelectList(_context.Dopis, "Id", "Urbroj", ugovorOKoristenju.DopisDostaveId);
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", ugovorOKoristenju.OdsId);
            return View(ugovorOKoristenju);
        }

        // GET: UgovoriOKoristenju/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ugovorOKoristenju = await _context.UgovorOKoristenju
                .Include(u => u.Dopis)
                .Include(u => u.DopisDostave)
                .Include(u => u.Ods)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ugovorOKoristenju == null)
            {
                return NotFound();
            }

            return View(ugovorOKoristenju);
        }

        // POST: UgovoriOKoristenju/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ugovorOKoristenju = await _context.UgovorOKoristenju.FindAsync(id);
            _context.UgovorOKoristenju.Remove(ugovorOKoristenju);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UgovorOKoristenjuExists(int id)
        {
            return _context.UgovorOKoristenju.Any(e => e.Id == id);
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

            // async/await - imam overhead, ali proširujem scalability
            List<UgovorOKoristenju> UgovorOKoristenjuList = new List<UgovorOKoristenju>();
            UgovorOKoristenjuList = await _context.UgovorOKoristenju.ToListAsync<UgovorOKoristenju>();

            // popunjava podatke za JSON da mogu vezane podatke pregledavati u datatables
            foreach (UgovorOKoristenju ugovorOKoristenju in UgovorOKoristenjuList)
            {
                ugovorOKoristenju.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == ugovorOKoristenju.OdsId); // kod mene je ugovorOKoristenju.OdsId -> Ods.Id (primarni ključ)
                ugovorOKoristenju.Dopis = await _context.Dopis.FirstOrDefaultAsync(o => o.Id == ugovorOKoristenju.DopisId);
                ugovorOKoristenju.DopisDostave = await _context.Dopis.FirstOrDefaultAsync(o => o.Id == ugovorOKoristenju.DopisDostaveId);
            }

            // filter
            int totalRows = UgovorOKoristenjuList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                UgovorOKoristenjuList = await UgovorOKoristenjuList.
                    Where(
                    x => x.BrojUgovora.Contains(searchValue)
                    || x.Ods.Omm.ToString().Contains(searchValue)
                    || x.DatumPotpisaHEP.ToString().Contains(searchValue)
                    || x.DatumPotpisaGZ.ToString().Contains(searchValue)
                    || x.Dopis.Predmet.Klasa.ToString().Contains(searchValue)
                    || x.RbrUgovora.ToString().Contains(searchValue)                    
                    || x.DopisDostave.Predmet.Klasa.ToString().Contains(searchValue)
                    || x.RbrDostave.ToString().Contains(searchValue)).ToDynamicListAsync<UgovorOKoristenju>();
                    // sortiranje radi normalno za datume, neovisno o formatu ToString
            }
            int totalRowsAfterFiltering = UgovorOKoristenjuList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            UgovorOKoristenjuList = UgovorOKoristenjuList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            UgovorOKoristenjuList = UgovorOKoristenjuList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<UgovorOKoristenju>();

            return Json(new { data = UgovorOKoristenjuList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        // TODO: delete for production  !!!!
        // Area51
        [HttpGet]
        public async Task<IActionResult> GetListJSON()
        {
            List<UgovorOKoristenju> UgovorOKoristenjuList = new List<UgovorOKoristenju>();
            UgovorOKoristenjuList = await _context.UgovorOKoristenju.ToListAsync<UgovorOKoristenju>();

            foreach (UgovorOKoristenju ugovorOKoristenju in UgovorOKoristenjuList)
            {
                ugovorOKoristenju.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == ugovorOKoristenju.OdsId); // kod mene je ugovorOKoristenju.OdsId -> Ods.Id (primarni ključ)
                ugovorOKoristenju.Dopis = await _context.Dopis.FirstOrDefaultAsync(o => o.Id == ugovorOKoristenju.DopisId);
                ugovorOKoristenju.DopisDostave = await _context.Dopis.FirstOrDefaultAsync(o => o.Id == ugovorOKoristenju.DopisDostaveId);
            }
            return Json(UgovorOKoristenjuList);
        }
    }
}
