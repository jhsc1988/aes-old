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
    public class ElektraKupciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ElektraKupciController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ElektraKupci
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ElektraKupac.Include(e => e.Ods);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ElektraKupci/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: ElektraKupci/Create
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
        public async Task<IActionResult> Create([Bind("Id,UgovorniRacun,OdsId,Napomena,VrijemeUnosa")] ElektraKupac elektraKupac)
        {
            if (ModelState.IsValid)
            {
                _context.Add(elektraKupac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // GET: ElektraKupci/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elektraKupac = await _context.ElektraKupac.FindAsync(id);
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

        // ajax server-side processing
        [HttpPost]
        public IActionResult GetList()
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            List<ElektraKupac> ElektraKupacList = new List<ElektraKupac>();
            ElektraKupacList = _context.ElektraKupac.ToList<ElektraKupac>();

            // need for json
            foreach (ElektraKupac elektraKupac in ElektraKupacList)
            {
                elektraKupac.Ods = _context.Ods.FirstOrDefault(o => o.Omm == elektraKupac.Ods.Omm);
            }

            // filter
            int totalRows = ElektraKupacList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                ElektraKupacList = ElektraKupacList.
                    Where(
                    x => x.UgovorniRacun.ToString().Contains(searchValue.ToLower())
                    || x.Ods.Omm.ToString().Contains(searchValue.ToLower())
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))).ToList<ElektraKupac>();
            }
            int totalRowsAfterFiltering = ElektraKupacList.Count;

            // sorting
            ElektraKupacList = ElektraKupacList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            ElektraKupacList = ElektraKupacList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<ElektraKupac>();

            return Json(new { data = ElektraKupacList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }
    }
}
