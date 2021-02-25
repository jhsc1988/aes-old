using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aes.Data;
using aes.Models;

namespace aes.Controllers
{
    public class DopisiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DopisiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dopisi
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dopis.Include(d => d.Predmet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Dopisi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dopis = await _context.Dopis
                .Include(d => d.Predmet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dopis == null)
            {
                return NotFound();
            }

            return View(dopis);
        }

        // GET: Dopisi/Create
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
        public async Task<IActionResult> Create([Bind("Id,PredmetId,Urbroj,Datum,VrijemeUnosa")] Dopis dopis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dopis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PredmetId"] = new SelectList(_context.Predmet, "Id", "Klasa", dopis.PredmetId);
            return View(dopis);
        }

        // GET: Dopisi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dopis = await _context.Dopis.FindAsync(id);
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
                    _context.Update(dopis);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dopis = await _context.Dopis
                .Include(d => d.Predmet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dopis == null)
            {
                return NotFound();
            }

            return View(dopis);
        }

        // POST: Dopisi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dopis = await _context.Dopis.FindAsync(id);
            _context.Dopis.Remove(dopis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DopisExists(int id)
        {
            return _context.Dopis.Any(e => e.Id == id);
        }
    }
}
