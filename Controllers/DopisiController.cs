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
    public class DopisiController : Controller, IDopisiController
    {
        private readonly ApplicationDbContext _context;
        private List<Dopis> DopisList;

        /// <summary>
        /// datatables params
        /// </summary>
        private string start, length, searchValue, sortColumnName, sortDirection;

        public DopisiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dopisi
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Dopis.Include(d => d.Predmet);
        //    return View(await applicationDbContext.ToListAsync());
        //}       
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Dopisi/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dopis dopis = await _context.Dopis
                .Include(d => d.Predmet)
                .FirstOrDefaultAsync(m => m.Id == id);
            return dopis == null ? NotFound() : View(dopis);
        }

        // GET: Dopisi/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["PredmetId"] = new SelectList(_context.Predmet, "Id", "Klasa");
            return View();
        }

        // POST: Dopisi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,PredmetId,Urbroj,Datum,VrijemeUnosa")] Dopis dopis)
        {
            if (ModelState.IsValid)
            {
                dopis.VrijemeUnosa = DateTime.Now;
                _ = _context.Add(dopis);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PredmetId"] = new SelectList(_context.Predmet, "Id", "Klasa", dopis.PredmetId);
            return View(dopis);
        }

        // GET: Dopisi/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dopis dopis = await _context.Dopis.FindAsync(id);
            dopis.Predmet = _context.Predmet.FirstOrDefault(e => e.Id == dopis.PredmetId);
            if (dopis == null)
            {
                return NotFound();
            }
            ViewData["PredmetId"] = new SelectList(_context.Predmet, "Id", "Klasa", dopis.PredmetId);
            return View(dopis);
        }

        // POST: Dopisi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PredmetId,Urbroj,Datum,VrijemeUnosa")] Dopis dopis)
        {
            if (id != dopis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dopis.Predmet = _context.Predmet.FirstOrDefault(e => e.Id == dopis.PredmetId);
                    _ = _context.Update(dopis);
                    _ = await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DopisExists(dopis.Id))
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
            ViewData["PredmetId"] = new SelectList(_context.Predmet, "Id", "Klasa", dopis.PredmetId);
            return View(dopis);
        }

        // GET: Dopisi/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dopis dopis = await _context.Dopis
                .Include(d => d.Predmet)
                .FirstOrDefaultAsync(m => m.Id == id);
            return dopis == null ? NotFound() : View(dopis);
        }

        // POST: Dopisi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Dopis dopis = await _context.Dopis.FindAsync(id);
            _ = _context.Dopis.Remove(dopis);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DopisExists(int id)
        {
            return _context.Dopis.Any(e => e.Id == id);
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

        public async Task<IActionResult> GetList(int predmetId)
        {
            GetDatatablesParamas();

            DopisList = await _context.Dopis.Where(e => e.PredmetId == predmetId).ToListAsync();

            foreach (Dopis dopis in DopisList)
            {
                dopis.Predmet = await _context.Predmet.FirstOrDefaultAsync(o => o.Id == dopis.PredmetId); // kod mene je dopis.PredmetId -> Predmet.Id (primarni ključ)
            }

            int totalRows = DopisList.Count;
            if (!string.IsNullOrEmpty(searchValue)) // filter
            {
                DopisList = await DopisList.
                    Where(
                    x => x.Predmet.Klasa.Contains(searchValue)
                    || x.Predmet.Naziv.ToLower().Contains(searchValue.ToLower())
                    || x.Datum.ToString().Contains(searchValue)
                    || x.Urbroj.Contains(searchValue)).ToDynamicListAsync<Dopis>();
            }

            int totalRowsAfterFiltering = DopisList.Count;

            DopisList = DopisList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            DopisList = DopisList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new
            {
                data = DopisList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        public JsonResult SaveToDB(string predmetId, string urbroj, string datumDopisa)
        {
            Dopis dTemp = new();
            dTemp.PredmetId = int.Parse(predmetId);
            dTemp.Urbroj = urbroj;
            dTemp.Datum = DateTime.Parse(datumDopisa);
            _ = _context.Dopis.Add(dTemp);
            return TrySave();
        }

        public JsonResult TrySave()
        {
            try
            {
                _ = _context.SaveChanges();
                return new(new { success = true, Message = "Spremljeno" });
            }
            catch (DbUpdateException)
            {
                return new(new { success = false, Message = "Greška" });
            }
        }
    }
}
