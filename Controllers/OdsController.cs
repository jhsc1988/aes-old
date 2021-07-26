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
    public class OdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OdsController(ApplicationDbContext context)
        {
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
                _context.Add(ods);
                await _context.SaveChangesAsync();
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
        [Authorize]
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
        [Authorize]
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
        public async Task<IActionResult> GetList()
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead (povećavam latency), ali proširujem scalability
            List<Ods> OdsList = new List<Ods>();
            OdsList = await _context.Ods.ToListAsync<Ods>();

            foreach (Ods ods in OdsList)
            {
                ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == ods.StanId); // kod mene je ods.StanId -> Stan.Id (primarni ključ)
            }

            // filter
            int totalRows = OdsList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                OdsList = await OdsList.
                    Where(
                    x => x.Omm.ToString().Contains(searchValue)
                    || x.Stan.StanId.ToString().Contains(searchValue)
                    || x.Stan.SifraObjekta.ToString().Contains(searchValue)
                    || (x.Stan.Adresa != null && x.Stan.Adresa.ToLower().Contains(searchValue.ToLower()))
                    || (x.Stan.Kat != null && x.Stan.Kat.ToLower().Contains(searchValue.ToLower()))
                    || (x.Stan.BrojSTana != null && x.Stan.BrojSTana.ToLower().Contains(searchValue.ToLower()))
                    || (x.Stan.Četvrt != null && x.Stan.Četvrt.ToLower().Contains(searchValue.ToLower()))
                    || x.Stan.Površina.ToString().Contains(searchValue)
                    || (x.Stan.StatusKorištenja != null && x.Stan.StatusKorištenja.ToLower().Contains(searchValue.ToLower()))
                    || (x.Stan.Korisnik != null && x.Stan.Korisnik.ToLower().Contains(searchValue.ToLower()))
                    || (x.Stan.Vlasništvo != null && x.Stan.Vlasništvo.ToLower().Contains(searchValue.ToLower()))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))).ToDynamicListAsync<Ods>();
            }
            int totalRowsAfterFiltering = OdsList.Count;

            // sorting
            OdsList = OdsList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            OdsList = OdsList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<Ods>();

            return Json(new { data = OdsList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        // TODO: delete for production  !!!!
        // Area 51
        [HttpGet]
        public async Task<IActionResult> GetListJSON()
        {

            List<Ods> OdsList = new List<Ods>();
            OdsList = await _context.Ods.ToListAsync<Ods>();

            foreach (Ods ods in OdsList)
            {
                ods.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == ods.StanId);
            }
            return Json(OdsList);
        }

        // ************************************ get kupci for notification builder ************************************ //

        public JsonResult GetStanData(string sid)
        {
            int idInt;
            if (sid != null)
            {
                idInt = int.Parse(sid);
            }
            else
            {
                return Json(new { success = false, Message = "Greška, prazan id" });
            }

            Stan st = new();
            List<Stan> stList = _context.Stan.ToList();

            st = stList.FirstOrDefault(o => o.Id == idInt);
            return Json(st);

        }

    }
}
