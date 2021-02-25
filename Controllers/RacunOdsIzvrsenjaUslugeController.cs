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
    public class RacunOdsIzvrsenjaUslugeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacunOdsIzvrsenjaUslugeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RacunOdsIzvrsenjaUsluge
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RacunOdsIzvrsenjaUsluge.Include(r => r.Dopis).Include(r => r.OdsKupac);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RacunOdsIzvrsenjaUsluge/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunOdsIzvrsenjaUsluge = await _context.RacunOdsIzvrsenjaUsluge
                .Include(r => r.Dopis)
                .Include(r => r.OdsKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunOdsIzvrsenjaUsluge == null)
            {
                return NotFound();
            }

            return View(racunOdsIzvrsenjaUsluge);
        }

        // GET: RacunOdsIzvrsenjaUsluge/Create
        public IActionResult Create()
        {
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            ViewData["OdsKupacId"] = new SelectList(_context.OdsKupac, "Id", "Id");
            return View();
        }

        // POST: RacunOdsIzvrsenjaUsluge/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,OdsKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunOdsIzvrsenjaUsluge racunOdsIzvrsenjaUsluge)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racunOdsIzvrsenjaUsluge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunOdsIzvrsenjaUsluge.DopisId);
            ViewData["OdsKupacId"] = new SelectList(_context.OdsKupac, "Id", "Id", racunOdsIzvrsenjaUsluge.OdsKupacId);
            return View(racunOdsIzvrsenjaUsluge);
        }

        // GET: RacunOdsIzvrsenjaUsluge/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunOdsIzvrsenjaUsluge = await _context.RacunOdsIzvrsenjaUsluge.FindAsync(id);
            if (racunOdsIzvrsenjaUsluge == null)
            {
                return NotFound();
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunOdsIzvrsenjaUsluge.DopisId);
            ViewData["OdsKupacId"] = new SelectList(_context.OdsKupac, "Id", "Id", racunOdsIzvrsenjaUsluge.OdsKupacId);
            return View(racunOdsIzvrsenjaUsluge);
        }

        // POST: RacunOdsIzvrsenjaUsluge/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,OdsKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunOdsIzvrsenjaUsluge racunOdsIzvrsenjaUsluge)
        {
            if (id != racunOdsIzvrsenjaUsluge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racunOdsIzvrsenjaUsluge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunOdsIzvrsenjaUslugeExists(racunOdsIzvrsenjaUsluge.Id))
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
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunOdsIzvrsenjaUsluge.DopisId);
            ViewData["OdsKupacId"] = new SelectList(_context.OdsKupac, "Id", "Id", racunOdsIzvrsenjaUsluge.OdsKupacId);
            return View(racunOdsIzvrsenjaUsluge);
        }

        // GET: RacunOdsIzvrsenjaUsluge/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunOdsIzvrsenjaUsluge = await _context.RacunOdsIzvrsenjaUsluge
                .Include(r => r.Dopis)
                .Include(r => r.OdsKupac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunOdsIzvrsenjaUsluge == null)
            {
                return NotFound();
            }

            return View(racunOdsIzvrsenjaUsluge);
        }

        // POST: RacunOdsIzvrsenjaUsluge/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var racunOdsIzvrsenjaUsluge = await _context.RacunOdsIzvrsenjaUsluge.FindAsync(id);
            _context.RacunOdsIzvrsenjaUsluge.Remove(racunOdsIzvrsenjaUsluge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunOdsIzvrsenjaUslugeExists(int id)
        {
            return _context.RacunOdsIzvrsenjaUsluge.Any(e => e.Id == id);
        }
    }
}
