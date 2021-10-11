using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class DopisiController : Controller, IDopisiController
    {
        private readonly IDatatablesGenerator _datatablesGenerator;
        private readonly IDopisWorkshop _dopisWorkshop;
        private readonly ApplicationDbContext _context;

        public DopisiController(ApplicationDbContext context, IDatatablesGenerator datatablesGenerator,
            IDopisWorkshop dopisWorkshop)
        {
            _dopisWorkshop = dopisWorkshop;
            _datatablesGenerator = datatablesGenerator;
            _context = context;
        }

        // GET: Dopisi
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Dopis.Include(d => d.Predmet);
        //    return View(await applicationDbContext.ToListAsync());
        //}       
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Dopisi/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dopis dopis = await _context.Dopis
                .Include(d => d.Predmet)
                .FirstOrDefaultAsync(m => m.Id == id);
            return dopis == null ? NotFound() : View(dopis);
        }

        // GET: Dopisi/Create
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,PredmetId,Urbroj,Datum,VrijemeUnosa")] Dopis dopis)
        {
            if (ModelState.IsValid)
            {
                dopis.VrijemeUnosa = DateTime.Now;
                _ = _context.Add(dopis);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PredmetId"] = new SelectList(_context.Predmet, "Id", "Klasa", dopis.PredmetId);
            return View(dopis);
        }

        // GET: Dopisi/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dopis dopis = await _context.Dopis.FindAsync(id);
            dopis.Predmet = _context.Predmet.FirstOrDefault(e => e.Id == dopis.PredmetId);
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
        [Authorize]
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
                    dopis.Predmet = _context.Predmet.FirstOrDefault(e => e.Id == dopis.PredmetId);
                    _ = _context.Update(dopis);
                    _ = await _context.SaveChangesAsync();
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
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dopis dopis = await _context.Dopis
                .Include(d => d.Predmet)
                .FirstOrDefaultAsync(m => m.Id == id);
            return dopis == null ? NotFound() : View(dopis);
        }

        // POST: Dopisi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Dopis dopis = await _context.Dopis.FindAsync(id);
            _ = _context.Dopis.Remove(dopis);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DopisExists(int id)
        {
            return _context.Dopis.Any(e => e.Id == id);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public JsonResult GetList(int predmetId) => _dopisWorkshop.GetList(_datatablesGenerator, _context, Request, predmetId);
        public JsonResult SaveToDB(string predmetId, string urbroj, string datumDopisa) => _dopisWorkshop.SaveToDB(predmetId, urbroj, datumDopisa, _context);

        //public JsonResult TrySave() => _dopisWorkshop.TrySave(_context, false);
    }
}
