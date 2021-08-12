using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class PredmetiController : Controller, IPredmetController
    {
        private readonly ApplicationDbContext _context;
        private List<Predmet> PredmetList;

        /// <summary>
        /// datatables params
        /// </summary>
        private string start, length, searchValue, sortColumnName, sortDirection;

        public PredmetiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Predmeti
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Predmet.ToListAsync());
        //}     
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Predmeti/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Predmet predmet = await _context.Predmet
                .FirstOrDefaultAsync(m => m.Id == id);
            return predmet == null ? NotFound() : View(predmet);
        }

        // GET: Predmeti/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Predmeti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Klasa,Naziv,VrijemeUnosa")] Predmet predmet)
        {
            if (ModelState.IsValid)
            {
                predmet.VrijemeUnosa = DateTime.Now;
                _ = _context.Add(predmet);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(predmet);
        }

        // GET: Predmeti/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Predmet predmet = await _context.Predmet.FindAsync(id);
            return predmet == null ? NotFound() : View(predmet);
        }

        // POST: Predmeti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Klasa,Naziv,VrijemeUnosa")] Predmet predmet)
        {
            if (id != predmet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = _context.Update(predmet);
                    _ = await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PredmetExists(predmet.Id))
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
            return View(predmet);
        }

        // GET: Predmeti/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Predmet predmet = await _context.Predmet
                .FirstOrDefaultAsync(m => m.Id == id);
            return predmet == null ? NotFound() : View(predmet);
        }

        // POST: Predmeti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Predmet predmet = await _context.Predmet.FindAsync(id);
            _ = _context.Predmet.Remove(predmet);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PredmetExists(int id)
        {
            return _context.Predmet.Any(e => e.Id == id);
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


        [HttpPost]
        public async Task<IActionResult> GetList()
        {
            GetDatatablesParamas();

            PredmetList = await _context.Predmet.ToListAsync();

            int totalRows = PredmetList.Count;
            if (!string.IsNullOrEmpty(searchValue)) // filter
            {
                PredmetList = await PredmetList.
                    Where(
                    x => x.Klasa.Contains(searchValue)
                    || x.Naziv.Contains(searchValue)).ToDynamicListAsync<Predmet>();
            }
            int totalRowsAfterFiltering = PredmetList.Count;

            PredmetList = PredmetList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            PredmetList = PredmetList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new
            {
                data = PredmetList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        public JsonResult SaveToDB(string klasa, string naziv)
        {
            Predmet pTemp = new();
            pTemp.Klasa = klasa;
            pTemp.Naziv = naziv;
            _ = _context.Predmet.Add(pTemp);
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
