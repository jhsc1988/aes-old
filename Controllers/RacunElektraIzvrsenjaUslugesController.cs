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
    public class RacunElektraIzvrsenjaUslugesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacunElektraIzvrsenjaUslugesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RacunElektraIzvrsenjaUsluges
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RacunElektraIzvrsenjeUsluge.Include(r => r.Dopis).Include(r => r.ElektraKupac);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RacunElektraIzvrsenjaUsluges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraIzvrsenjeUsluge = await _context.RacunElektraIzvrsenjeUsluge
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraIzvrsenjeUsluge == null)
            {
                return NotFound();
            }

            return View(racunElektraIzvrsenjeUsluge);
        }

        // GET: RacunElektraIzvrsenjaUsluges/Create
        public IActionResult Create()
        {
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id");
            return View();
        }

        // POST: RacunElektraIzvrsenjaUsluges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racunElektraIzvrsenjeUsluge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektraIzvrsenjeUsluge.ElektraKupacId);
            return View(racunElektraIzvrsenjeUsluge);
        }

        // GET: RacunElektraIzvrsenjaUsluges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraIzvrsenjeUsluge = await _context.RacunElektraIzvrsenjeUsluge.FindAsync(id);
            if (racunElektraIzvrsenjeUsluge == null)
            {
                return NotFound();
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektraIzvrsenjeUsluge.ElektraKupacId);
            return View(racunElektraIzvrsenjeUsluge);
        }

        // POST: RacunElektraIzvrsenjaUsluges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge)
        {
            if (id != racunElektraIzvrsenjeUsluge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racunElektraIzvrsenjeUsluge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunElektraIzvrsenjeUslugeExists(racunElektraIzvrsenjeUsluge.Id))
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
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(_context.ElektraKupac, "Id", "Id", racunElektraIzvrsenjeUsluge.ElektraKupacId);
            return View(racunElektraIzvrsenjeUsluge);
        }

        // GET: RacunElektraIzvrsenjaUsluges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraIzvrsenjeUsluge = await _context.RacunElektraIzvrsenjeUsluge
                .Include(r => r.Dopis)
                .Include(r => r.ElektraKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraIzvrsenjeUsluge == null)
            {
                return NotFound();
            }

            return View(racunElektraIzvrsenjeUsluge);
        }

        // POST: RacunElektraIzvrsenjaUsluges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var racunElektraIzvrsenjeUsluge = await _context.RacunElektraIzvrsenjeUsluge.FindAsync(id);
            _context.RacunElektraIzvrsenjeUsluge.Remove(racunElektraIzvrsenjeUsluge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraIzvrsenjeUslugeExists(int id)
        {
            return _context.RacunElektraIzvrsenjeUsluge.Any(e => e.Id == id);
        }
    }
}
