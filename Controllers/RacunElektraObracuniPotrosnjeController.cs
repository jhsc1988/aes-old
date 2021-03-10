﻿using System;
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
    public class RacunElektraObracuniPotrosnjeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacunElektraObracuniPotrosnjeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RacunElektraObracuniPotrosnje
        public async Task<IActionResult> Index()
        {
            return View(await _context.RacunElektraObracunPotrosnje.ToListAsync());
        }

        // GET: RacunElektraObracuniPotrosnje/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraObracunPotrosnje = await _context.RacunElektraObracunPotrosnje
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraObracunPotrosnje == null)
            {
                return NotFound();
            }

            return View(racunElektraObracunPotrosnje);
        }

        // GET: RacunElektraObracuniPotrosnje/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RacunElektraObracuniPotrosnje/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DatumObracuna,brojilo,BrojRacuna,RVT,RNT,VrijemeUnosa")] RacunElektraObracunPotrosnje racunElektraObracunPotrosnje)
        {
            if (ModelState.IsValid)
            {
                racunElektraObracunPotrosnje.VrijemeUnosa = DateTime.Now;
                _context.Add(racunElektraObracunPotrosnje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(racunElektraObracunPotrosnje);
        }

        // GET: RacunElektraObracuniPotrosnje/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraObracunPotrosnje = await _context.RacunElektraObracunPotrosnje.FindAsync(id);
            if (racunElektraObracunPotrosnje == null)
            {
                return NotFound();
            }
            return View(racunElektraObracunPotrosnje);
        }

        // POST: RacunElektraObracuniPotrosnje/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DatumObracuna,brojilo,BrojRacuna,RVT,RNT,VrijemeUnosa")] RacunElektraObracunPotrosnje racunElektraObracunPotrosnje)
        {
            if (id != racunElektraObracunPotrosnje.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racunElektraObracunPotrosnje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunElektraObracunPotrosnjeExists(racunElektraObracunPotrosnje.Id))
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
            return View(racunElektraObracunPotrosnje);
        }

        // GET: RacunElektraObracuniPotrosnje/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racunElektraObracunPotrosnje = await _context.RacunElektraObracunPotrosnje
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racunElektraObracunPotrosnje == null)
            {
                return NotFound();
            }

            return View(racunElektraObracunPotrosnje);
        }

        // POST: RacunElektraObracuniPotrosnje/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var racunElektraObracunPotrosnje = await _context.RacunElektraObracunPotrosnje.FindAsync(id);
            _context.RacunElektraObracunPotrosnje.Remove(racunElektraObracunPotrosnje);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraObracunPotrosnjeExists(int id)
        {
            return _context.RacunElektraObracunPotrosnje.Any(e => e.Id == id);
        }

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća listu racuna Holdinga u JSON obliku za server side processing</returns>
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
            List<RacunElektraObracunPotrosnje> RacunElektraObracunPotrosnjeList = new List<RacunElektraObracunPotrosnje>();
            RacunElektraObracunPotrosnjeList = await _context.RacunElektraObracunPotrosnje.ToListAsync<RacunElektraObracunPotrosnje>();

            // popunjava podatke za JSON da mogu vezane podatke pregledavati u datatables
            foreach (RacunElektraObracunPotrosnje racunElektraObracunPotrosnje in RacunElektraObracunPotrosnjeList)
            {
                racunElektraObracunPotrosnje.RacunElektra = await _context.RacunElektra.FirstOrDefaultAsync(o => o.Id == racunElektraObracunPotrosnje.RacunElektraId); // kod mene je racunElektraObracunPotrosnje.RacunElektraId -> RacunElektra.Id (primarni ključ)
            }


            // filter
            int totalRows = RacunElektraObracunPotrosnjeList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                RacunElektraObracunPotrosnjeList = await RacunElektraObracunPotrosnjeList.
                    Where(
                    x => x.DatumObracuna.ToString().Contains(searchValue)
                    || x.brojilo.ToString().Contains(searchValue)
                    || x.RacunElektra.BrojRacuna.ToString().Contains(searchValue)
                    || x.RVT.ToString().Contains(searchValue)
                    || x.RNT.ToString().Contains(searchValue)).ToDynamicListAsync<RacunElektraObracunPotrosnje>();
                    // sortiranje radi normalno za datume, neovisno o formatu ToString
            }
            int totalRowsAfterFiltering = RacunElektraObracunPotrosnjeList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            RacunElektraObracunPotrosnjeList = RacunElektraObracunPotrosnjeList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            RacunElektraObracunPotrosnjeList = RacunElektraObracunPotrosnjeList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList<RacunElektraObracunPotrosnje>();

            return Json(new { data = RacunElektraObracunPotrosnjeList, draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()), recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering });
        }

        // TODO: delete for production  !!!!
        // Area51
        [HttpGet]
        public async Task<IActionResult> GetListJSON()
        {
            List<RacunElektraObracunPotrosnje> RacunElektraObracunPotrosnjeList = new List<RacunElektraObracunPotrosnje>();
            RacunElektraObracunPotrosnjeList = await _context.RacunElektraObracunPotrosnje.ToListAsync<RacunElektraObracunPotrosnje>();

            foreach (RacunElektraObracunPotrosnje racunElektraObracunPotrosnje in RacunElektraObracunPotrosnjeList)
            {
                racunElektraObracunPotrosnje.RacunElektra = await _context.RacunElektra.FirstOrDefaultAsync(o => o.Id == racunElektraObracunPotrosnje.RacunElektraId); // kod mene je racunElektraObracunPotrosnje.RacunElektraId -> RacunElektra.Id (primarni ključ)
            }
            return Json(RacunElektraObracunPotrosnjeList);
        }
    }
}