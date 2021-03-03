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
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id");
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
                _context.Add(odsKupac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", odsKupac.OdsId);
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

            List<OdsKupac> OdsKupacList = new List<OdsKupac>();
            OdsKupacList = _context.OdsKupac.ToList<OdsKupac>();

            // need for JSON
            foreach (OdsKupac odsKupac in OdsKupacList)
            {
                odsKupac.Ods = _context.Ods.FirstOrDefault(o => o.Omm == odsKupac.Ods.Omm); // kod mene je ods.StanId -> Stan.Id
            }

            // filter
            int totalRows = OdsKupacList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                OdsKupacList = OdsKupacList.
                    Where(
                    x => x.SifraKupca.ToString().Contains(searchValue.ToLower())
                    || x.Ods.Omm.ToString().Contains(searchValue.ToLower())
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))).ToList<OdsKupac>();
            }
            int totalRowsAfterFiltering = OdsKupacList.Count;

            // sorting
            OdsKupacList = OdsKupacList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            OdsKupacList = OdsKupacList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<OdsKupac>();

            return Json(new { data = OdsKupacList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }
    }
}
