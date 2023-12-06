using aes.Data;
using aes.Models.HEP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace aes.Controllers.HEP
{
    public class ObracunPotrosnjeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ObracunPotrosnjeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ObracunPotrosnje
        public async Task<IActionResult> Index()
        {
            Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ObracunPotrosnje, TarifnaStavka> applicationDbContext = _context.ObracunPotrosnje.Include(o => o.RacunElektra).Include(o => o.TarifnaStavka);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ObracunPotrosnje/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ObracunPotrosnje obracunPotrosnje = await _context.ObracunPotrosnje
                .Include(o => o.RacunElektra)
                .Include(o => o.TarifnaStavka)
                .FirstOrDefaultAsync(m => m.Id == id);
            return obracunPotrosnje == null ? NotFound() : View(obracunPotrosnje);
        }

        // GET: ObracunPotrosnje/Create
        public IActionResult Create()
        {
            ViewData["RacunElektraId"] = new SelectList(_context.RacunElektra, "Id", "BrojRacuna");
            ViewData["TarifnaStavkaId"] = new SelectList(_context.TarifnaStavka, "Id", "Id");
            return View();
        }

        // POST: ObracunPotrosnje/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RacunElektraId,BrojBrojila,TarifnaStavkaId,DatumOd,DatumDo,StanjeOd,StanjeDo")] ObracunPotrosnje obracunPotrosnje)
        {
            if (ModelState.IsValid)
            {
                _ = _context.Add(obracunPotrosnje);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RacunElektraId"] = new SelectList(_context.RacunElektra, "Id", "BrojRacuna", obracunPotrosnje.RacunElektraId);
            ViewData["TarifnaStavkaId"] = new SelectList(_context.TarifnaStavka, "Id", "Id", obracunPotrosnje.TarifnaStavkaId);
            return View(obracunPotrosnje);
        }

        // GET: ObracunPotrosnje/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ObracunPotrosnje obracunPotrosnje = await _context.ObracunPotrosnje.FindAsync(id);
            if (obracunPotrosnje == null)
            {
                return NotFound();
            }
            ViewData["RacunElektraId"] = new SelectList(_context.RacunElektra, "Id", "BrojRacuna", obracunPotrosnje.RacunElektraId);
            ViewData["TarifnaStavkaId"] = new SelectList(_context.TarifnaStavka, "Id", "Naziv", obracunPotrosnje.TarifnaStavkaId);
            return View(obracunPotrosnje);
        }

        // POST: ObracunPotrosnje/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RacunElektraId,BrojBrojila,TarifnaStavkaId,DatumOd,DatumDo,StanjeOd,StanjeDo")] ObracunPotrosnje obracunPotrosnje)
        {
            if (id != obracunPotrosnje.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = _context.Update(obracunPotrosnje);
                    _ = await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObracunPotrosnjeExists(obracunPotrosnje.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "RacuniElektra", new { id = obracunPotrosnje.RacunElektraId });
            }

            ViewData["RacunElektraId"] = new SelectList(_context.RacunElektra, "Id", "BrojRacuna", obracunPotrosnje.RacunElektraId);
            ViewData["TarifnaStavkaId"] = new SelectList(_context.TarifnaStavka, "Id", "Id", obracunPotrosnje.TarifnaStavkaId);

            return View(obracunPotrosnje);
        }

        // GET: ObracunPotrosnje/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ObracunPotrosnje obracunPotrosnje = await _context.ObracunPotrosnje
                .Include(o => o.RacunElektra)
                .Include(o => o.TarifnaStavka)
                .FirstOrDefaultAsync(m => m.Id == id);
            return obracunPotrosnje == null ? NotFound() : View(obracunPotrosnje);
        }

        // POST: ObracunPotrosnje/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ObracunPotrosnje obracunPotrosnje = await _context.ObracunPotrosnje.FindAsync(id);
            _ = _context.ObracunPotrosnje.Remove(obracunPotrosnje!);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObracunPotrosnjeExists(int id)
        {
            return _context.ObracunPotrosnje.Any(e => e.Id == id);
        }
    }
}
