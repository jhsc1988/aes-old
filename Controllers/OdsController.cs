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
        private readonly ApplicationDbContext _context;
        private List<Ods> OdsList;

        /// <summary>
        /// datatables params
        /// </summary>
        private string start, length, searchValue, sortColumnName, sortDirection;
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

        public void GetDatatablesParamas()
        {
            // server side parameters
            start = Request.Form["start"].FirstOrDefault();
            length = Request.Form["length"].FirstOrDefault();
            searchValue = Request.Form["search[value]"].FirstOrDefault();
            sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        }

        public async Task<IActionResult> GetList()
        {
            GetDatatablesParamas();
            OdsList = await _context.Ods
                .Include(e => e.Stan)
                .ToListAsync();

            int totalRows = OdsList.Count;
            if (!string.IsNullOrEmpty(searchValue)) // filter
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

            OdsList = OdsList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            OdsList = OdsList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new
            {
                data = OdsList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

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
