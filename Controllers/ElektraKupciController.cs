using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class ElektraKupciController : Controller
    {
        private readonly IDatatablesGenerator _datatablesGenerator;
        private readonly IRacunWorkshop _racunWorkshop;
        private readonly IElektraKupacWorkshop _elektraKupacWorkshop;
        private readonly IRacunElektraWorkshop _racunElektraWorkshop;
        private readonly IRacunElektraRateWorkshop _racunElektraRateWorkshop;
        private readonly IRacunElektraIzvrsenjeUslugeWorkshop _racunElektraIzvrsenjeUslugeWorkshop;
        private readonly ApplicationDbContext _context;

        public ElektraKupciController(ApplicationDbContext context, IDatatablesGenerator datatablesParamsGeneratorcs,
            IElektraKupacWorkshop elektraKupacWorkshop, IRacunElektraWorkshop racunElektraWorkshop, IRacunElektraRateWorkshop racunElektraRateWorkshop,
            IRacunElektraIzvrsenjeUslugeWorkshop racunElektraIzvrsenjeUslugeWorkshop, IRacunWorkshop racunWorkshop)
        {
            _context = context;
            _racunWorkshop = racunWorkshop;
            _racunElektraWorkshop = racunElektraWorkshop;
            _racunElektraRateWorkshop = racunElektraRateWorkshop;
            _racunElektraIzvrsenjeUslugeWorkshop = racunElektraIzvrsenjeUslugeWorkshop;
            _datatablesGenerator = datatablesParamsGeneratorcs;
            _elektraKupacWorkshop = elektraKupacWorkshop;
        }

        // GET: ElektraKupci
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.ElektraKupac.Include(e => e.Ods);
        //    return View(await applicationDbContext.ToListAsync());
        //}   
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: ElektraKupci/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ElektraKupac elektraKupac = await _context.ElektraKupac
                .Include(e => e.Ods)
                .Include(e => e.Ods.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            return elektraKupac == null ? NotFound() : View(elektraKupac);
        }

        // GET: ElektraKupci/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id");
            return View();
        }

        // POST: ElektraKupci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,UgovorniRacun,OdsId,Napomena,VrijemeUnosa")] ElektraKupac elektraKupac)
        {
            if (ModelState.IsValid)
            {
                elektraKupac.VrijemeUnosa = DateTime.Now;
                _ = _context.Add(elektraKupac);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // GET: ElektraKupci/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ElektraKupac elektraKupac = await _context.ElektraKupac.FindAsync(id);
            elektraKupac.Ods = _context.Ods.FirstOrDefault(e => e.Id == elektraKupac.OdsId);
            elektraKupac.Ods.Stan = _context.Stan.FirstOrDefault(e => e.StanId == elektraKupac.Ods.StanId);
            if (elektraKupac == null)
            {
                return NotFound();
            }
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // POST: ElektraKupci/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UgovorniRacun,OdsId,Napomena,VrijemeUnosa")] ElektraKupac elektraKupac)
        {
            if (id != elektraKupac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = _context.Update(elektraKupac);
                    _ = await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElektraKupacExists(elektraKupac.Id))
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
            ViewData["OdsId"] = new SelectList(_context.Ods, "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // GET: ElektraKupci/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ElektraKupac elektraKupac = await _context.ElektraKupac
                .Include(e => e.Ods)
                .FirstOrDefaultAsync(m => m.Id == id);
            return elektraKupac == null ? NotFound() : View(elektraKupac);
        }

        // POST: ElektraKupci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ElektraKupac elektraKupac = await _context.ElektraKupac.FindAsync(id);
            _ = _context.ElektraKupac.Remove(elektraKupac);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElektraKupacExists(int id)
        {
            return _context.ElektraKupac.Any(e => e.Id == id);
        }

        // validation
        [HttpGet]
        public async Task<IActionResult> UgovorniRacunValidation(long ugovorniRacun)
        {
            if (ugovorniRacun is < 1000000000 or > 9999999999)
            {
                return Json($"Ugovorni račun nije ispravan");
            }

            // TODO: dodati uvjet za omm - vjerojatno mi treba i unique constraint
            // vidjeti i kod ostalih controllera
            ElektraKupac db = await _context.ElektraKupac.FirstOrDefaultAsync(x => x.UgovorniRacun == ugovorniRacun);
            if (db != null)
            {
                return Json($"Ugovorni račun {ugovorniRacun} već postoji."); // TODO: isti kupac se teoretski moze pojaviti na drugom omm
                // TODO: treba staviti nekakav alert da već postoji
            }
            return Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<IActionResult> GetList()
            => await _elektraKupacWorkshop.GetList(_datatablesGenerator, _context, Request, _elektraKupacWorkshop);

        public JsonResult GetRacuniForKupac(int param)
            => _elektraKupacWorkshop.GetRacuniForKupac(param, _datatablesGenerator, Request, _racunWorkshop, _context, _racunElektraWorkshop);

        public JsonResult GetRacuniRateForKupac(int param)
            => _elektraKupacWorkshop.GetRacuniRateForKupac(param, _datatablesGenerator, Request, _racunWorkshop, _context, _racunElektraRateWorkshop);

        public JsonResult GetRacuniElektraIzvrsenjeForKupac(int param)
            => _elektraKupacWorkshop.GetRacuniElektraIzvrsenjeForKupac(param, _datatablesGenerator, Request, _racunWorkshop, _context, _racunElektraIzvrsenjeUslugeWorkshop);
    }
}
