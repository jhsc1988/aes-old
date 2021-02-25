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
    public class RacunElektraObracuniPotrosnjeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacunElektraObracuniPotrosnjeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RacunElektraObracuniPotrosnje
        public async Task<IActionResult> Index()
        {
            return View(await _context.RacunElektraObracunPotrosnje.ToListAsync());
        }

        // GET: RacunElektraObracuniPotrosnje/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraObracunPotrosnje = await _context.RacunElektraObracunPotrosnje
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraObracunPotrosnje == null)
            {
                return NotFound();
            }

            return View(racunElektraObracunPotrosnje);
        }

        // GET: RacunElektraObracuniPotrosnje/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RacunElektraObracuniPotrosnje/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DatumObracuna,brojilo,BrojRacuna,RVT,RNT,VrijemeUnosa")] RacunElektraObracunPotrosnje racunElektraObracunPotrosnje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racunElektraObracunPotrosnje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(racunElektraObracunPotrosnje);
        }

        // GET: RacunElektraObracuniPotrosnje/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraObracunPotrosnje = await _context.RacunElektraObracunPotrosnje.FindAsync(id);
            if (racunElektraObracunPotrosnje == null)
            {
                return NotFound();
            }
            return View(racunElektraObracunPotrosnje);
        }

        // POST: RacunElektraObracuniPotrosnje/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DatumObracuna,brojilo,BrojRacuna,RVT,RNT,VrijemeUnosa")] RacunElektraObracunPotrosnje racunElektraObracunPotrosnje)
        {
            if (id != racunElektraObracunPotrosnje.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racunElektraObracunPotrosnje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunElektraObracunPotrosnjeExists(racunElektraObracunPotrosnje.Id))
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
            return View(racunElektraObracunPotrosnje);
        }

        // GET: RacunElektraObracuniPotrosnje/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraObracunPotrosnje = await _context.RacunElektraObracunPotrosnje
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraObracunPotrosnje == null)
            {
                return NotFound();
            }

            return View(racunElektraObracunPotrosnje);
        }

        // POST: RacunElektraObracuniPotrosnje/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var racunElektraObracunPotrosnje = await _context.RacunElektraObracunPotrosnje.FindAsync(id);
            _context.RacunElektraObracunPotrosnje.Remove(racunElektraObracunPotrosnje);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraObracunPotrosnjeExists(int id)
        {
            return _context.RacunElektraObracunPotrosnje.Any(e => e.Id == id);
        }
    }
}
