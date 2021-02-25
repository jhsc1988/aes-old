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
    public class RacuniHoldingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacuniHoldingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RacuniHolding
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RacunHolding.Include(r => r.Dopis).Include(r => r.Stan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RacuniHolding/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunHolding = await _context.RacunHolding
                .Include(r => r.Dopis)
                .Include(r => r.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunHolding == null)
            {
                return NotFound();
            }

            return View(racunHolding);
        }

        // GET: RacuniHolding/Create
        public IActionResult Create()
        {
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa");
            return View();
        }

        // POST: RacuniHolding/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,StanId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunHolding racunHolding)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racunHolding);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", racunHolding.StanId);
            return View(racunHolding);
        }

        // GET: RacuniHolding/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunHolding = await _context.RacunHolding.FindAsync(id);
            if (racunHolding == null)
            {
                return NotFound();
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", racunHolding.StanId);
            return View(racunHolding);
        }

        // POST: RacuniHolding/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,StanId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunHolding racunHolding)
        {
            if (id != racunHolding.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racunHolding);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunHoldingExists(racunHolding.Id))
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
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", racunHolding.StanId);
            return View(racunHolding);
        }

        // GET: RacuniHolding/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunHolding = await _context.RacunHolding
                .Include(r => r.Dopis)
                .Include(r => r.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunHolding == null)
            {
                return NotFound();
            }

            return View(racunHolding);
        }

        // POST: RacuniHolding/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var racunHolding = await _context.RacunHolding.FindAsync(id);
            _context.RacunHolding.Remove(racunHolding);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunHoldingExists(int id)
        {
            return _context.RacunHolding.Any(e => e.Id == id);
        }
    }
}
