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
    public class RacuniElektraRateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacuniElektraRateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RacuniElektraRate
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RacunElektraRate.Include(r => r.Dopis).Include(r => r.ElektraKupac);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RacuniElektraRate/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraRate = await _context.RacunElektraRate
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraRate == null)
            {
                return NotFound();
            }

            return View(racunElektraRate);
        }

        // GET: RacuniElektraRate/Create
        public IActionResult Create()
        {
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id");
            return View();
        }

        // POST: RacuniElektraRate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,ElektraKupacId,Razdoblje,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunElektraRate racunElektraRate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racunElektraRate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektraRate.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektraRate.ElektraKupacId);
            return View(racunElektraRate);
        }

        // GET: RacuniElektraRate/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraRate = await _context.RacunElektraRate.FindAsync(id);
            if (racunElektraRate == null)
            {
                return NotFound();
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektraRate.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektraRate.ElektraKupacId);
            return View(racunElektraRate);
        }

        // POST: RacuniElektraRate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,ElektraKupacId,Razdoblje,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunElektraRate racunElektraRate)
        {
            if (id != racunElektraRate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racunElektraRate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunElektraRateExists(racunElektraRate.Id))
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
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektraRate.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektraRate.ElektraKupacId);
            return View(racunElektraRate);
        }

        // GET: RacuniElektraRate/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraRate = await _context.RacunElektraRate
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraRate == null)
            {
                return NotFound();
            }

            return View(racunElektraRate);
        }

        // POST: RacuniElektraRate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var racunElektraRate = await _context.RacunElektraRate.FindAsync(id);
            _context.RacunElektraRate.Remove(racunElektraRate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraRateExists(int id)
        {
            return _context.RacunElektraRate.Any(e => e.Id == id);
        }
    }
}
