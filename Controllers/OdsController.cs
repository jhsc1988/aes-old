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
    public class OdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ods
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ods.Include(o => o.Stan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ods = await _context.Ods
                .Include(o => o.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ods == null)
            {
                return NotFound();
            }

            return View(ods);
        }

        // GET: Ods/Create
        public IActionResult Create()
        {
            ViewBag.ocontext = _context.Ods.ToList();
            ViewBag.scontext = _context.Stan.ToList();

            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa");
            return View();
        }

        // POST: Ods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StanId,Omm,Napomena,VrijemeUnosa")] Ods ods)
        {
            if (ModelState.IsValid)
            {
                ods.VrijemeUnosa = DateTime.Now;
                _context.Add(ods);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", ods.StanId);
            return View(ods);
        }

        // GET: Ods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ods = await _context.Ods.FindAsync(id);
            if (ods == null)
            {
                return NotFound();
            }
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", ods.StanId);
            return View(ods);
        }

        // POST: Ods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StanId,Omm,Napomena,VrijemeUnosa")] Ods ods)
        {
            if (id != ods.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ods);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OdsExists(ods.Id))
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
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", ods.StanId);
            return View(ods);
        }

        // GET: Ods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ods = await _context.Ods
                .Include(o => o.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ods == null)
            {
                return NotFound();
            }

            return View(ods);
        }

        // POST: Ods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ods = await _context.Ods.FindAsync(id);
            _context.Ods.Remove(ods);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OdsExists(int id)
        {
            return _context.Ods.Any(e => e.Id == id);
        }

        // validation
        [HttpGet]
        public async Task<IActionResult>OmmValidation(int omm)
        {
            if (omm < 10000000 || omm > 99999999)
            {
                return Json($"Broj obračunskog mjernog mjesta nije ispravan");
            }

            var db = await _context.Ods.FirstOrDefaultAsync(x => x.Omm == omm);
            if (db != null)
            {
                return Json($"Obračunsko mjerno mjesto {omm} već postoji.");
            }
            return Json(true);
        }
    }
}
