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
    public class StanoviController : Controller
    {
        private readonly ApplicationDbContext _context;

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
                _context.Add(stan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(stan);
        }

        // GET: Stanovi/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var stan = await _context.Stan.FindAsync(id);
            if (stan == null) return NotFound();

            return View(stan);
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
            if (id != stan.Id) return NotFound();

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
                        return NotFound();
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
            if (id == null) return NotFound();

            var stan = await _context.Stan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stan == null) return NotFound();

            return View(stan);
        }

        // POST: Stanovi/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
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
        ///     Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća listu stanova u JSON obliku za server side processing</returns>
        [HttpPost]
        public async Task<IActionResult> GetList()
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request
                .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead, ali proširujem scalability
            var StanList = new List<Stan>();
            StanList = await _context.Stan.ToListAsync();

            // filter
            var totalRows = StanList.Count;
            if (!string.IsNullOrEmpty(searchValue))
                StanList = await StanList.Where(
                        x => x.StanId.ToString().Contains(searchValue)
                             || x.SifraObjekta.ToString().Contains(searchValue)
                             || x.Adresa != null && x.Adresa.ToLower().Contains(searchValue.ToLower())
                             || x.Kat != null && x.Kat.ToLower().Contains(searchValue.ToLower())
                             || x.BrojSTana != null && x.BrojSTana.ToLower().Contains(searchValue.ToLower())
                             || x.Četvrt != null && x.Četvrt.ToLower().Contains(searchValue.ToLower())
                             || x.Površina.ToString().Contains(searchValue)
                             || x.StatusKorištenja != null &&
                             x.StatusKorištenja.ToLower().Contains(searchValue.ToLower())
                             || x.Korisnik != null && x.Korisnik.ToLower().Contains(searchValue.ToLower())
                             || x.Vlasništvo != null && x.Vlasništvo.ToLower().Contains(searchValue.ToLower()))
                    .ToDynamicListAsync<Stan>();

            var totalRowsAfterFiltering = StanList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting

            StanList = StanList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            StanList = StanList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = StanList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        /// <summary>
        ///     Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća filtriranu listu stanova (za koje ne postoje omm HEP-ODS-a) u JSON obliku za server side processing</returns>
        public async Task<IActionResult> GetListFiltered()
        {
            // Server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request
                .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead, ali proširujem scalability
            var StanList = new List<Stan>();
            StanList = await _context.Stan.Where(p => !_context.Ods.Any(o => o.StanId == p.Id)).ToListAsync();

            // filter
            var totalRows = StanList.Count;
            if (!string.IsNullOrEmpty(searchValue))
                // async/await - imam overhead, ali proširujem scalability
                StanList = await StanList.Where(
                        x => x.StanId.ToString().Contains(searchValue)
                             || x.SifraObjekta.ToString().Contains(searchValue)
                             || x.Adresa != null && x.Adresa.ToLower().Contains(searchValue.ToLower())
                             || x.Kat != null && x.Kat.ToLower().Contains(searchValue.ToLower())
                             || x.BrojSTana != null && x.BrojSTana.ToLower().Contains(searchValue.ToLower())
                             || x.Četvrt != null && x.Četvrt.ToLower().Contains(searchValue.ToLower())
                             || x.Površina.ToString().Contains(searchValue)
                             || x.StatusKorištenja != null &&
                             x.StatusKorištenja.ToLower().Contains(searchValue.ToLower())
                             || x.Korisnik != null && x.Korisnik.ToLower().Contains(searchValue.ToLower())
                             || x.Vlasništvo != null && x.Vlasništvo.ToLower().Contains(searchValue.ToLower()))
                    .ToDynamicListAsync<Stan>();

            var totalRowsAfterFiltering = StanList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            StanList = StanList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            StanList = StanList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = StanList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        // TODO: delete for production  !!!!
        // Area 51
        [HttpGet]
        public async Task<IActionResult> GetListJSON()
        {
            var StanList = new List<Stan>();
            StanList = await _context.Stan.ToListAsync();
            return Json(StanList);
        }

        [HttpPost]
        public async Task<IActionResult> GetRacuniForStan(int stanid)
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request
                .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead, ali proširujem scalability
            var RacunElektraList = new List<RacunElektra>();
            RacunElektraList =
                await _context.RacunElektra.Where(p => p.ElektraKupac.Ods.Stan.Id == stanid).ToListAsync();

            foreach (var re in RacunElektraList)
                re.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.Id == re.ElektraKupacId);

            // filter
            var totalRows = RacunElektraList.Count;
            if (!string.IsNullOrEmpty(searchValue))
                RacunElektraList = await RacunElektraList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue)
                             || x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue)
                             || x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))
                    .ToDynamicListAsync<RacunElektra>();

            var totalRowsAfterFiltering = RacunElektraList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            RacunElektraList = RacunElektraList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

            // paging
            RacunElektraList = RacunElektraList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = RacunElektraList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }


        // TODO: delete for production
        [HttpGet]
        public async Task<IActionResult> GetRacuniForStanJSON()
        {
            var RacunElektraList = new List<RacunElektra>();
            RacunElektraList =
                await _context.RacunElektra.ToListAsync();

            foreach (var re in RacunElektraList)
                re.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.Id == re.ElektraKupacId);

            return Json(RacunElektraList);
        }


        [HttpPost]
        public async Task<IActionResult> GetRacuniRateForStan(int stanid)
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request
                .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead, ali proširujem scalability
            var RacunElektraRateList =
                await _context.RacunElektraRate.Where(p => p.ElektraKupac.Ods.Stan.Id == stanid).ToListAsync();

            foreach (var rer in RacunElektraRateList)
                rer.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.Id == rer.ElektraKupacId);

            // filter
            var totalRows = RacunElektraRateList.Count;
            if (!string.IsNullOrEmpty(searchValue))
                RacunElektraRateList = await RacunElektraRateList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.Razdoblje.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue)
                             || x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue)
                             || x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))
                    .ToDynamicListAsync<RacunElektraRate>();

            var totalRowsAfterFiltering = RacunElektraRateList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            RacunElektraRateList = RacunElektraRateList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection)
                .ToList();

            // paging
            RacunElektraRateList =
                RacunElektraRateList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = RacunElektraRateList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetHoldingRacuniForStan(int stanid)
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request
                .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            var racuniHoldingList =
                await _context.RacunHolding.Where(p => p.StanId == stanid).ToListAsync();

            foreach (var rh in racuniHoldingList)
                rh.Stan = await _context.Stan.FirstOrDefaultAsync(o => o.Id == rh.StanId);

            // filter
            var totalRows = racuniHoldingList.Count;
            if (!string.IsNullOrEmpty(searchValue))
                racuniHoldingList = await racuniHoldingList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.Stan.SifraObjekta.ToString().Contains(searchValue)
                             || x.Stan.StanId.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Iznos.ToString().Contains(searchValue)
                             || x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue)
                             || x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Napomena != null && x.Napomena.ToLower().Contains(searchValue.ToLower()))
                    .ToDynamicListAsync<RacunHolding>();

            var totalRowsAfterFiltering = racuniHoldingList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            racuniHoldingList = racuniHoldingList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection)
                .ToList();

            // paging
            racuniHoldingList =
                racuniHoldingList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = racuniHoldingList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }


        [HttpPost]
        public async Task<IActionResult> GetRacuniOdsIzvrsenjeForStan(int stanid)
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request
                .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead, ali proširujem scalability
            var RacuniOdsIzvrsenjeList =
                await _context.RacunOdsIzvrsenjaUsluge.Where(p => p.OdsKupac.Ods.Stan.Id == stanid).ToListAsync();

            foreach (var roiu in RacuniOdsIzvrsenjeList)
                roiu.OdsKupac = await _context.OdsKupac.FirstOrDefaultAsync(o => o.Id == roiu.OdsKupacId);

            // filter
            var totalRows = RacuniOdsIzvrsenjeList.Count;
            if (!string.IsNullOrEmpty(searchValue))
                RacuniOdsIzvrsenjeList = await RacuniOdsIzvrsenjeList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.OdsKupac.SifraKupca.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.DatumIzvrsenja.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Usluga != null && x.Usluga.ToLower().Contains(searchValue.ToLower())
                             || x.Iznos.ToString().Contains(searchValue)
                             || x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue)
                             || x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Napomena != null && x.Napomena.ToLower().Contains(searchValue))
                    .ToDynamicListAsync<RacunOdsIzvrsenjaUsluge>();

            var totalRowsAfterFiltering = RacuniOdsIzvrsenjeList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            RacuniOdsIzvrsenjeList = RacuniOdsIzvrsenjeList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection)
                .ToList();

            // paging
            RacuniOdsIzvrsenjeList =
                RacuniOdsIzvrsenjeList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = RacuniOdsIzvrsenjeList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }


        [HttpPost]
        public async Task<IActionResult> GetRacuniElektraIzvrsenjeForStan(int stanid)
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request
                .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead, ali proširujem scalability
            var RacuniElektraIzvrsenjeList =
                await _context.RacunElektraIzvrsenjeUsluge.Where(p => p.ElektraKupac.Ods.Stan.Id == stanid)
                    .ToListAsync();

            foreach (var reiu in RacuniElektraIzvrsenjeList)
                reiu.ElektraKupac = await _context.ElektraKupac.FirstOrDefaultAsync(o => o.Id == reiu.ElektraKupacId);

            // filter
            var totalRows = RacuniElektraIzvrsenjeList.Count;
            if (!string.IsNullOrEmpty(searchValue))
                RacuniElektraIzvrsenjeList = await RacuniElektraIzvrsenjeList.Where(
                        x => x.BrojRacuna.Contains(searchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(searchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.DatumIzvrsenja.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Usluga != null && x.Usluga.ToLower().Contains(searchValue.ToLower())
                             || x.Iznos.ToString().Contains(searchValue)
                             || x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(searchValue)
                             || x.DatumPotvrde != null &&
                             x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(searchValue)
                             || x.Napomena != null && x.Napomena.ToLower().Contains(searchValue))
                    .ToDynamicListAsync<RacunElektraIzvrsenjeUsluge>();

            var totalRowsAfterFiltering = RacuniElektraIzvrsenjeList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            RacuniElektraIzvrsenjeList = RacuniElektraIzvrsenjeList.AsQueryable()
                .OrderBy(sortColumnName + " " + sortDirection)
                .ToList();

            // paging
            RacuniElektraIzvrsenjeList =
                RacuniElektraIzvrsenjeList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = RacuniElektraIzvrsenjeList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }


        [HttpPost]
        public async Task<IActionResult> GetUgovoriOKoristenjuForStan(int stanid)
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request
                .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead, ali proširujem scalability
            var UgovoriOKoristenjuList =
                await _context.UgovorOKoristenju.Where(p => p.Ods.Stan.Id == stanid).ToListAsync();

            foreach (var uok in UgovoriOKoristenjuList)
                uok.Ods = await _context.Ods.FirstOrDefaultAsync(o => o.Id == uok.OdsId);

            foreach (var uok in UgovoriOKoristenjuList)
                uok.Dopis = await _context.Dopis.FirstOrDefaultAsync(o => o.Id == uok.DopisId);

            foreach (var uok in UgovoriOKoristenjuList)
                uok.Dopis.Predmet = await _context.Predmet.FirstOrDefaultAsync(o => o.Id == uok.Dopis.PredmetId);

            // filter
            var totalRows = UgovoriOKoristenjuList.Count;
            if (!string.IsNullOrEmpty(searchValue))
                UgovoriOKoristenjuList = await UgovoriOKoristenjuList.Where(
                    x => x.BrojUgovora.Contains(searchValue)
                         || x.Ods.Omm.ToString().Contains(searchValue)
                         || x.DatumPotpisaHEP.ToString().Contains(searchValue)
                         || x.DatumPotpisaGZ.ToString().Contains(searchValue)
                         || x.Dopis.Predmet.Klasa.ToString().Contains(searchValue)
                         || x.RbrUgovora.ToString().Contains(searchValue)
                         || x.DopisDostave.Predmet.Klasa.ToString().Contains(searchValue)
                         || x.RbrDostave.ToString().Contains(searchValue)).ToDynamicListAsync<UgovorOKoristenju>();

            var totalRowsAfterFiltering = UgovoriOKoristenjuList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            UgovoriOKoristenjuList = UgovoriOKoristenjuList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection)
                .ToList();

            // paging
            UgovoriOKoristenjuList =
                UgovoriOKoristenjuList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = UgovoriOKoristenjuList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }


        [HttpPost]
        public async Task<IActionResult> GetUgovoriOPrijenosuForStan(int stanid)
        {
            // server side parameters
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnName = Request
                .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // async/await - imam overhead, ali proširujem scalability
            var UgovoriOPrijenosuList =
                await _context.UgovorOPrijenosu.Where(p => p.UgovorOKoristenju.Ods.Stan.Id == stanid).ToListAsync();

            foreach (var uok in UgovoriOPrijenosuList)
                uok.UgovorOKoristenju =
                    await _context.UgovorOKoristenju.FirstOrDefaultAsync(o => o.Id == uok.UgovorOKoristenjuId);

            foreach (var uok in UgovoriOPrijenosuList)
                uok.Dopis = await _context.Dopis.FirstOrDefaultAsync(o => o.Id == uok.DopisId);

            foreach (var uok in UgovoriOPrijenosuList)
                uok.Dopis.Predmet = await _context.Predmet.FirstOrDefaultAsync(o => o.Id == uok.Dopis.PredmetId);

            // filter
            var totalRows = UgovoriOPrijenosuList.Count;
            if (!string.IsNullOrEmpty(searchValue))
                UgovoriOPrijenosuList = await UgovoriOPrijenosuList.Where(
                    x => x.BrojUgovora.Contains(searchValue)
                         || x.DatumPrijenosa.ToString().Contains(searchValue)
                         || x.UgovorOKoristenju.BrojUgovora.Contains(searchValue)
                         || x.DatumPotpisa.ToString().Contains(searchValue)
                         || x.Kupac.ToLower().Contains(searchValue.ToLower())
                         || x.KupacOIB.ToString().Contains(searchValue)
                         || x.Dopis.Predmet.Klasa.ToString().Contains(searchValue)
                         || x.RbrUgovora.ToString().Contains(searchValue)
                         || x.DopisDostave.Predmet.Klasa.ToString().Contains(searchValue)
                         || x.RbrDostave.ToString().Contains(searchValue)).ToDynamicListAsync<UgovorOPrijenosu>();

            var totalRowsAfterFiltering = UgovoriOPrijenosuList.Count;

            // trebam System.Linq.Dynamic.Core;
            // sorting
            UgovoriOPrijenosuList = UgovoriOPrijenosuList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection)
                .ToList();

            // paging
            UgovoriOPrijenosuList =
                UgovoriOPrijenosuList.Skip(Convert.ToInt32(start)).Take(Convert.ToInt32(length)).ToList();

            return Json(new
            {
                data = UgovoriOPrijenosuList,
                draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }
    }
}