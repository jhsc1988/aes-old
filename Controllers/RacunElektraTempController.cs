using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class RacunElektraTempController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacunElektraTempController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RacunElektraTemp
        public async Task<IActionResult> Index()
        {
            return View(await _context.RacunElektraTemp.ToListAsync());
        }

        // GET: RacunElektraTemp/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraTemp = await _context.RacunElektraTemp
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraTemp == null)
            {
                return NotFound();
            }

            return View(racunElektraTemp);
        }

        // GET: RacunElektraTemp/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RacunElektraTemp/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,guid,RedniBroj,BrojRacuna,DatumIzdavanja,Iznos,DopisId")] RacunElektraTemp racunElektraTemp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racunElektraTemp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(racunElektraTemp);
        }

        // GET: RacunElektraTemp/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraTemp = await _context.RacunElektraTemp.FindAsync(id);
            if (racunElektraTemp == null)
            {
                return NotFound();
            }
            return View(racunElektraTemp);
        }

        // POST: RacunElektraTemp/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,guid,RedniBroj,BrojRacuna,DatumIzdavanja,Iznos,DopisId")] RacunElektraTemp racunElektraTemp)
        {
            if (id != racunElektraTemp.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racunElektraTemp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunElektraTempExists(racunElektraTemp.Id))
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
            return View(racunElektraTemp);
        }

        // GET: RacunElektraTemp/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraTemp = await _context.RacunElektraTemp
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraTemp == null)
            {
                return NotFound();
            }

            return View(racunElektraTemp);
        }

        // POST: RacunElektraTemp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var racunElektraTemp = await _context.RacunElektraTemp.FindAsync(id);
            _context.RacunElektraTemp.Remove(racunElektraTemp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraTempExists(int id)
        {
            return _context.RacunElektraTemp.Any(e => e.Id == id);
        }
    }
}
