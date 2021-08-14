using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class StanoviController : Controller, IStanController
    {
        private readonly ApplicationDbContext _context;
        private List<Stan> StanList;
        private List<RacunElektra> RacunElektraList;
        private List<RacunElektraRate> RacunElektraRateList;
        private List<RacunHolding> racuniHoldingList;
        private List<RacunElektraIzvrsenjeUsluge> RacuniElektraIzvrsenjeList;

        /// <summary>
        /// datatables params
        /// </summary>
        private string start, length, searchValue, sortColumnName, sortDirection;

        public StanoviController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stanovi
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Stan.ToListAsync());
        //}
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Stanovi/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Stan stan = await _context.Stan
                .FirstOrDefaultAsync(m => m.Id == id);
            return stan == null ? NotFound() : View(stan);
        }

        // GET: Stanovi/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stanovi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(
            [Bind(
                "Id,StanId,SifraObjekta,Vrsta,Adresa,Kat,BrojSTana,Naselje,Četvrt,Površina,StatusKorištenja,Korisnik,Vlasništvo,DioNekretnine,Sektor,Status,VrijemeUnosa")]
            Stan stan)
        {
            if (ModelState.IsValid)
            {
                stan.VrijemeUnosa = DateTime.Now;
                _ = _context.Add(stan);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(stan);
        }

        // GET: Stanovi/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Stan stan = await _context.Stan.FindAsync(id);
            return stan == null ? NotFound() : View(stan);
        }

        // POST: Stanovi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id,StanId,SifraObjekta,Vrsta,Adresa,Kat,BrojSTana,Naselje,Četvrt,Površina,StatusKorištenja,Korisnik,Vlasništvo,DioNekretnine,Sektor,Status,VrijemeUnosa")]
            Stan stan)
        {
            if (id != stan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = _context.Update(stan);
                    _ = await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StanExists(stan.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(stan);
        }

        // GET: Stanovi/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Stan stan = await _context.Stan
                .FirstOrDefaultAsync(m => m.Id == id);
            return stan == null ? NotFound() : View(stan);
        }

        // POST: Stanovi/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Stan stan = await _context.Stan.FindAsync(id);
            _ = _context.Stan.Remove(stan);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StanExists(int id)
        {
            return _context.Stan.Any(e => e.Id == id);
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
            StanList = _context.Stan.ToList();

            int totalRows = StanList.Count;
            if (!string.IsNullOrEmpty(searchValue)) // filter
            {
                StanList = await StanList.Where(
                        x => x.StanId.ToString().Contains(searchValue)
                             || x.SifraObjekta.ToString().Contains(searchValue)
                             || (x.Adresa != null && x.Adresa.ToLower().Contains(searchValue.ToLower()))
                             || (x.Kat != null && x.Kat.ToLower().Contains(searchValue.ToLower()))
                             || (x.BrojSTana != null && x.BrojSTana.ToLower().Contains(searchValue.ToLower()))
                             || (x.Četvrt != null && x.Četvrt.ToLower().Contains(searchValue.ToLower()))
                             || x.Površina.ToString().Contains(searchValue)
                             || (x.StatusKorištenja != null &&
                             x.StatusKorištenja.ToLower().Contains(searchValue.ToLower()))
                             || (x.Korisnik != null && x.Korisnik.ToLower().Contains(searchValue.ToLower()))
                             || (x.Vlasništvo != null && x.Vlasništvo.ToLower().Contains(searchValue.ToLower())))
                    .ToDynamicListAsync<Stan>();
            }

            int totalRowsAfterFiltering = StanList.Count;

            StanList = StanList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            StanList = StanList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new
            {
                data = StanList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        public async Task<IActionResult> GetListFiltered()
        {
            GetDatatablesParamas();

            StanList = await _context.Stan.Where(p => !_context.Ods.Any(o => o.StanId == p.Id)).ToListAsync();

            int totalRows = StanList.Count;
            if (!string.IsNullOrEmpty(searchValue)) // filter
            {
                StanList = await StanList.Where(
                        x => x.StanId.ToString().Contains(searchValue)
                             || x.SifraObjekta.ToString().Contains(searchValue)
                             || (x.Adresa != null && x.Adresa.ToLower().Contains(searchValue.ToLower()))
                             || (x.Kat != null && x.Kat.ToLower().Contains(searchValue.ToLower()))
                             || (x.BrojSTana != null && x.BrojSTana.ToLower().Contains(searchValue.ToLower()))
                             || (x.Četvrt != null && x.Četvrt.ToLower().Contains(searchValue.ToLower()))
                             || x.Površina.ToString().Contains(searchValue)
                             || (x.StatusKorištenja != null &&
                             x.StatusKorištenja.ToLower().Contains(searchValue.ToLower()))
                             || (x.Korisnik != null && x.Korisnik.ToLower().Contains(searchValue.ToLower()))
                             || (x.Vlasništvo != null && x.Vlasništvo.ToLower().Contains(searchValue.ToLower())))
                    .ToDynamicListAsync<Stan>();
            }

            int totalRowsAfterFiltering = StanList.Count;

            StanList = StanList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            StanList = StanList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new
            {
                data = StanList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // for Details page

        [HttpPost]
        public async Task<IActionResult> GetRacuniForStan(int param)
        {
            GetDatatablesParamas();

            RacunElektraList = await _context.RacunElektra
                .Include(e => e.ElektraKupac)
                .Include(e => e.ElektraKupac.Ods)
                .Include(e => e.ElektraKupac.Ods.Stan)
                .Where(e => e.ElektraKupac.Ods.Stan.Id == param)
                .ToListAsync();

            int totalRows = RacunElektraList.Count;
            if (!string.IsNullOrEmpty(searchValue)) // filter
            {
                RacunElektraList = await RacunElektraList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue)
                             || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                             || (x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                             || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower())))
                    .ToDynamicListAsync<RacunElektra>();
            }

            int totalRowsAfterFiltering = RacunElektraList.Count;

            RacunElektraList = RacunElektraList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            RacunElektraList = RacunElektraList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new
            {
                data = RacunElektraList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        public async Task<IActionResult> GetRacuniRateForStan(int param)
        {

            GetDatatablesParamas();
            RacunElektraRateList = await _context.RacunElektraRate
                .Include(e => e.ElektraKupac)
                .Include(e => e.ElektraKupac.Ods)
                .Include(e => e.ElektraKupac.Ods.Stan)
                .Where(e => e.ElektraKupac.Ods.Stan.Id == param)
                .ToListAsync();

            int totalRows = RacunElektraRateList.Count; // filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                RacunElektraRateList = await RacunElektraRateList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.Razdoblje.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue)
                             || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                             || (x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                             || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower())))
                    .ToDynamicListAsync<RacunElektraRate>();
            }

            int totalRowsAfterFiltering = RacunElektraRateList.Count;


            RacunElektraRateList = RacunElektraRateList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            RacunElektraRateList = RacunElektraRateList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new
            {
                data = RacunElektraRateList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        public async Task<IActionResult> GetHoldingRacuniForStan(int param)
        {
            GetDatatablesParamas();
            racuniHoldingList = await _context.RacunHolding
                .Include(e => e.Stan)
                .Where(e => e.StanId == param)
                .ToListAsync();

            int totalRows = racuniHoldingList.Count;
            if (!string.IsNullOrEmpty(searchValue)) // filter
            {
                racuniHoldingList = await racuniHoldingList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.Stan.SifraObjekta.ToString().Contains(searchValue)
                             || x.Stan.StanId.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue)
                             || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                             || (x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                             || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower())))
                    .ToDynamicListAsync<RacunHolding>();
            }

            int totalRowsAfterFiltering = racuniHoldingList.Count;

            racuniHoldingList = racuniHoldingList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            racuniHoldingList = racuniHoldingList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new
            {
                data = racuniHoldingList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        public async Task<IActionResult> GetRacuniElektraIzvrsenjeForStan(int param)
        {
            GetDatatablesParamas();
            RacuniElektraIzvrsenjeList = await _context.RacunElektraIzvrsenjeUsluge
                .Include(e => e.ElektraKupac)
                .Include(e => e.ElektraKupac.Ods)
                .Include(e => e.ElektraKupac.Ods.Stan)
                .Where(e => e.ElektraKupac.Ods.Stan.Id == param)
                .ToListAsync();

            int totalRows = RacuniElektraIzvrsenjeList.Count;
            if (!string.IsNullOrEmpty(searchValue)) // filter
            {
                RacuniElektraIzvrsenjeList = await RacuniElektraIzvrsenjeList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.DatumIzvrsenja.ToString("dd.MM.yyyy").Contains(searchValue)
                             || (x.Usluga != null && x.Usluga.ToLower().Contains(searchValue.ToLower()))
                             || x.Iznos.ToString().Contains(searchValue)
                             || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue))
                             || (x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue))
                             || (x.Napomena != null && x.Napomena.ToLower().Contains(searchValue)))
                    .ToDynamicListAsync<RacunElektraIzvrsenjeUsluge>();
            }

            int totalRowsAfterFiltering = RacuniElektraIzvrsenjeList.Count;

            RacuniElektraIzvrsenjeList = RacuniElektraIzvrsenjeList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList(); // sorting
            RacuniElektraIzvrsenjeList = RacuniElektraIzvrsenjeList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList(); // paging

            return Json(new
            {
                data = RacuniElektraIzvrsenjeList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }
    }
}