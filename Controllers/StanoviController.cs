using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

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

        // unnecessary overhead

        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Stan.ToListAsync());
        //}
        public IActionResult Index()
        {
            return View();
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
                stan.VrijemeUnosa = DateTime.Now;
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

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća listu stanova u JSON obliku za server side processing</returns>
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
            List<Stan> StanList = new List<Stan>();
            StanList = await _context.Stan.ToListAsync<Stan>();

            // filter
            int totalRows = StanList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                // TODO: filter unnecessary ToLower remove, za sve kontrolere
                StanList = await StanList.
                    Where(
                    x => x.StanId.ToString().Contains(searchValue.ToLower())
                    || x.SifraObjekta.ToString().Contains(searchValue.ToLower())
                    || (x.Adresa != null && x.Adresa.ToLower().Contains(searchValue.ToLower()))
                    || (x.Kat != null && x.Kat.ToLower().Contains(searchValue.ToLower()))
                    || (x.BrojSTana != null && x.BrojSTana.ToLower().Contains(searchValue.ToLower()))
                    || (x.Četvrt != null && x.Četvrt.ToLower().Contains(searchValue.ToLower()))
                    || x.Površina.ToString().Contains(searchValue.ToLower())
                    || (x.StatusKorištenja != null && x.StatusKorištenja.ToLower().Contains(searchValue.ToLower()))
                    || (x.Korisnik != null && x.Korisnik.ToLower().Contains(searchValue.ToLower()))
                    || (x.Vlasništvo != null && x.Vlasništvo.ToLower().Contains(searchValue.ToLower()))).ToDynamicListAsync<Stan>();
            }
            int totalRowsAfterFiltering = StanList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            StanList = StanList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            StanList = StanList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<Stan>();

            return Json(new { data = StanList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća filtriranu listu stanova (za koje ne postoje omm HEP-ODS-a) u JSON obliku za server side processing</returns>
        public async Task<IActionResult> GetListFiltered()
        {
            // Server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead (povećavam latency), ali proširujem scalability
            List<Stan> StanList = new List<Stan>();
            StanList = await _context.Stan.Where(p => !_context.Ods.Any(o => o.StanId == p.Id)).ToListAsync<Stan>();

            // filter
            int totalRows = StanList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                // async/await - imam overhead (povećavam latency), ali proširujem scalability
                StanList = await StanList.
                    Where(
                    x => x.StanId.ToString().Contains(searchValue.ToLower())
                    || x.SifraObjekta.ToString().Contains(searchValue.ToLower())
                    || (x.Adresa != null && x.Adresa.ToLower().Contains(searchValue.ToLower()))
                    || (x.Kat != null && x.Kat.ToLower().Contains(searchValue.ToLower()))
                    || (x.BrojSTana != null && x.BrojSTana.ToLower().Contains(searchValue.ToLower()))
                    || (x.Četvrt != null && x.Četvrt.ToLower().Contains(searchValue.ToLower()))
                    || x.Površina.ToString().Contains(searchValue.ToLower())
                    || (x.StatusKorištenja != null && x.StatusKorištenja.ToLower().Contains(searchValue.ToLower()))
                    || (x.Korisnik != null && x.Korisnik.ToLower().Contains(searchValue.ToLower()))
                    || (x.Vlasništvo != null && x.Vlasništvo.ToLower().Contains(searchValue.ToLower()))).ToDynamicListAsync<Stan>();
            }
            int totalRowsAfterFiltering = StanList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            StanList = StanList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            StanList = StanList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<Stan>();

            return Json(new { data = StanList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        // TODO: delete for production  !!!!
        // Area 51 - testing facility
        [HttpGet]
        public async Task<IActionResult> GetListJSON()
        {

            List<Stan> StanList = new List<Stan>();
            StanList = await _context.Stan.ToListAsync<Stan>();
            return Json(StanList);
        }
    }
}
