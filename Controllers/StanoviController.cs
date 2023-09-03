using aes.CommonDependecies.ICommonDependencies;
using aes.Controllers.IControllers;
using aes.Models;
using aes.Models.Racuni.Elektra;
using aes.Models.Racuni.Holding;
using aes.Services;
using aes.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aes.Controllers
{
    public class StanoviController : Controller, IStanoviController
    {
        private readonly ICommonDependencies _c;
        private readonly IStanUploadService _stanUploadService;

        public StanoviController(ICommonDependencies c, IStanUploadService stanUploadService)
        {
            _c = c;
            _stanUploadService = stanUploadService;
        }

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

            Stan stan = await _c.UnitOfWork.Stan.FindExact(m => m.Id == id);
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
                await _c.UnitOfWork.Stan.Add(stan);
                _ = await _c.UnitOfWork.Complete();
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

            Stan stan = await _c.UnitOfWork.Stan.Get((int)id);
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
                    _ = await _c.UnitOfWork.Stan.Update(stan);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await StanExists(stan.Id))
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

            Stan stan = await _c.UnitOfWork.Stan.FindExact(m => m.Id == id);
            return stan == null ? NotFound() : View(stan);
        }

        // POST: Stanovi/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Stan stan = await _c.UnitOfWork.Stan.Get(id);
            _c.UnitOfWork.Stan.Remove(stan);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> StanExists(int id)
        {
            return await _c.UnitOfWork.Stan.Any(e => e.Id == id);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Upload

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            return await _stanUploadService.Upload(Request, User.Identity?.Name);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Main Datatables

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList()
        {
            IEnumerable<Stan> list = await _c.UnitOfWork.Stan.GetStanovi();

            return new DatatablesService<Stan>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetStanoviForDatatables);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Stanovi wihout ods-omm (ODS Create page)

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetListFiltered()
        {
            IEnumerable<Stan> list = await _c.UnitOfWork.Stan.GetStanoviWithoutOdsOmm();

            return new DatatablesService<Stan>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetStanoviForDatatables);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Details page - Racuni for Stan

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetRacuniElektraForStan(int param)
        {
            IEnumerable<RacunElektra> list = await _c.UnitOfWork.Ods.GetRacuniForOmm<RacunElektra>(param);

            return new DatatablesService<RacunElektra>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniElektraForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetRacuniElektraRateForStan(int param)
        {
            IEnumerable<RacunElektraRate> list = await _c.UnitOfWork.Ods.GetRacuniForOmm<RacunElektraRate>(param);

            return new DatatablesService<RacunElektraRate>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniElektraRateForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetRacuniElektraIzvrsenjeUslugeForStan(int param)
        {
            IEnumerable<RacunElektraIzvrsenjeUsluge> list =
                await _c.UnitOfWork.Ods.GetRacuniForOmm<RacunElektraIzvrsenjeUsluge>(param);

            return new DatatablesService<RacunElektraIzvrsenjeUsluge>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacunElektraIzvrsenjeUslugeForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetRacuniHoldingForStan(int param)
        {
            IEnumerable<RacunHolding> list = await _c.UnitOfWork.RacuniHolding.GetRacuniForStan(param);

            return new DatatablesService<RacunHolding>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniHoldingForDatatables);
        }
    }
}