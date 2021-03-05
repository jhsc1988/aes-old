using aes.Data;
using aes.Models;
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
    public class OdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ods

        // unnecessary overhead

        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Ods.Include(o => o.Stan);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        public IActionResult Index()
        {
            return View();
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
            // ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa");
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
            //ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", ods.StanId);
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
        public async Task<IActionResult> OmmValidation(int omm)
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

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća listu obračunskih mjernih mjesta za HEP - ODS u JSON obliku za server side processing</returns>
        [HttpPost]
        public IActionResult GetList()
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            List<Ods> OdsList = new List<Ods>();
            OdsList = _context.Ods.ToList<Ods>();


            foreach (Ods ods in OdsList)
            {
                ods.Stan = _context.Stan.FirstOrDefault(o => o.Id == ods.StanId); // kod mene je ods.StanId -> Stan.Id (primarni ključ)
            }

            // filter
            int totalRows = OdsList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                OdsList = OdsList.
                    Where(
                    x => x.Omm.ToString().Contains(searchValue.ToLower())
                    || x.Stan.StanId.ToString().Contains(searchValue.ToLower())
                    || x.Stan.SifraObjekta.ToString().Contains(searchValue.ToLower())
                    || (x.Stan.Adresa != null && x.Stan.Adresa.ToLower().Contains(searchValue.ToLower()))
                    || (x.Stan.Kat != null && x.Stan.Kat.ToLower().Contains(searchValue.ToLower()))
                    || (x.Stan.BrojSTana != null && x.Stan.BrojSTana.ToLower().Contains(searchValue.ToLower()))
                    || (x.Stan.Četvrt != null && x.Stan.Četvrt.ToLower().Contains(searchValue.ToLower()))
                    || x.Stan.Površina.ToString().Contains(searchValue.ToLower())
                    || (x.Stan.StatusKorištenja != null && x.Stan.StatusKorištenja.ToLower().Contains(searchValue.ToLower()))
                    || (x.Stan.Korisnik != null && x.Stan.Korisnik.ToLower().Contains(searchValue.ToLower()))
                    || (x.Stan.Vlasništvo != null && x.Stan.Vlasništvo.ToLower().Contains(searchValue.ToLower()))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))).ToList<Ods>();
            }
            int totalRowsAfterFiltering = OdsList.Count;

            // sorting
            OdsList = OdsList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            OdsList = OdsList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<Ods>();

            return Json(new { data = OdsList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }
    }
}
