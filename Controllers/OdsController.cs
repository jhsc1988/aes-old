using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class OdsController : Controller, IOdsController
    {
        private readonly IDatatablesGenerator _datatablesGenerator;
        private readonly IOdsWorkshop _odsWorkshop;
        private readonly ApplicationDbContext _context;
        private DatatablesParams Params;
        private List<Ods> OdsList;

        public OdsController(ApplicationDbContext context, IDatatablesGenerator datatablesParamsGeneratorcs, IOdsWorkshop odsWorkshop)
        {
            _datatablesGenerator = datatablesParamsGeneratorcs;
            _odsWorkshop = odsWorkshop;
            _context = context;
        }

        // GET: Ods
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Ods.Include(o => o.Stan);
        //    return View(await applicationDbContext.ToListAsync());
        //}
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Ods/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ods ods = await _context.Ods
                .Include(o => o.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            return ods == null ? NotFound() : View(ods);
        }

        // GET: Ods/Create
        [Authorize]
        public IActionResult Create()
        {
            // ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa");
            return View();
        }

        // POST: Ods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,StanId,Omm,Napomena,VrijemeUnosa")] Ods ods)
        {
            if (ModelState.IsValid)
            {
                ods.VrijemeUnosa = DateTime.Now;
                _ = _context.Add(ods);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", ods.StanId);
            return View(ods);
        }

        // GET: Ods/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ods ods = await _context.Ods.FindAsync(id);
            ods.Stan = _context.Stan.FirstOrDefault(e => e.StanId == ods.StanId);
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
        [Authorize]
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
                    _ = _context.Update(ods);
                    _ = await _context.SaveChangesAsync();
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
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ods ods = await _context.Ods
                .Include(o => o.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            return ods == null ? NotFound() : View(ods);
        }

        // POST: Ods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Ods ods = await _context.Ods.FindAsync(id);
            _ = _context.Ods.Remove(ods);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OdsExists(int id)
        {
            return _context.Ods.Any(e => e.Id == id);
        }

        // validation
        [HttpGet]
        public async Task<IActionResult> OmmValidation(int omm)
        {
            if (omm is < 10000000 or > 99999999)
            {
                return Json($"Broj obračunskog mjernog mjesta nije ispravan");
            }
            Ods db = await _context.Ods.FirstOrDefaultAsync(x => x.Omm == omm);
            return db != null ? Json($"Obračunsko mjerno mjesto {omm} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<IActionResult> GetList()
        {
            Params = _datatablesGenerator.GetParams(Request);
            OdsList = await _context.Ods
                .Include(e => e.Stan)
                .ToListAsync();

            int totalRows = OdsList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue)) // filter
            {
                OdsList = _odsWorkshop.GetStanoviForDatatables(Params, OdsList);
            }
            int totalRowsAfterFiltering = OdsList.Count;
            return _datatablesGenerator.SortingPaging(OdsList, Params, Request, totalRows, totalRowsAfterFiltering);
        }

        public JsonResult GetStanData(string sid)
        {
            return _odsWorkshop.GetStanData(sid, _context);
        }
    }
}
