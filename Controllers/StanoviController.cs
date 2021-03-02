using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aes.Data;
using aes.Models;
using System.Linq.Dynamic.Core;

namespace aes.Controllers
{
    public class StanoviController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StanoviController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stanovi
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stan.ToListAsync());
        }

        // GET: Stanovi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stan = await _context.Stan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stan == null)
            {
                return NotFound();
            }

            return View(stan);
        }

        // GET: Stanovi/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stanovi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StanId,SifraObjekta,Vrsta,Adresa,Kat,BrojSTana,Naselje,Četvrt,Površina,StatusKorištenja,Korisnik,Vlasništvo,DioNekretnine,Sektor,Status,VrijemeUnosa")] Stan stan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stan);
        }

        // GET: Stanovi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stan = await _context.Stan.FindAsync(id);
            if (stan == null)
            {
                return NotFound();
            }
            return View(stan);
        }

        // POST: Stanovi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StanId,SifraObjekta,Vrsta,Adresa,Kat,BrojSTana,Naselje,Četvrt,Površina,StatusKorištenja,Korisnik,Vlasništvo,DioNekretnine,Sektor,Status,VrijemeUnosa")] Stan stan)
        {
            if (id != stan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StanExists(stan.Id))
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
            return View(stan);
        }

        // GET: Stanovi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stan = await _context.Stan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stan == null)
            {
                return NotFound();
            }

            return View(stan);
        }

        // POST: Stanovi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stan = await _context.Stan.FindAsync(id);
            _context.Stan.Remove(stan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StanExists(int id)
        {
            return _context.Stan.Any(e => e.Id == id);
        }

        // ajax server-side processing
        [HttpPost]
        public IActionResult GetList()
        {
            // Server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();


            List<Stan> StanList = new List<Stan>();
            StanList = _context.Stan.ToList<Stan>();

            // filter
            int totalRows = StanList.Count;
            if (!string.IsNullOrEmpty(searchValue))  
            {
                StanList = StanList.
                    Where(
                    x => x.Id.ToString().Contains(searchValue.ToLower())
                    || x.SifraObjekta.ToString().Contains(searchValue.ToLower())
                    || (x.Adresa != null && x.Adresa.ToLower().Contains(searchValue.ToLower()))
                    || (x.Kat != null && x.Kat.ToLower().Contains(searchValue.ToLower()))
                    || (x.BrojSTana != null && x.BrojSTana.ToLower().Contains(searchValue.ToLower()))
                    || (x.Četvrt != null && x.Četvrt.ToLower().Contains(searchValue.ToLower()))
                    || x.Površina.ToString().Contains(searchValue.ToLower())
                    || (x.StatusKorištenja != null && x.StatusKorištenja.ToLower().Contains(searchValue.ToLower()))
                    || (x.Korisnik != null && x.Korisnik.ToLower().Contains(searchValue.ToLower()))
                    || (x.Vlasništvo != null && x.Vlasništvo.ToLower().Contains(searchValue.ToLower()))).ToList<Stan>();
            }
            int totalRowsAfterFiltering = StanList.Count;

            // sorting
            StanList = StanList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            StanList = StanList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<Stan>();

            return Json(new{ data = StanList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }
    }
}
