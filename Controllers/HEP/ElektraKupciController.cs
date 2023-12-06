using aes.CommonDependecies.ICommonDependencies;
using aes.Models.HEP;
using aes.Models.Racuni.Elektra;
using aes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace aes.Controllers.HEP
{
    public class ElektraKupciController : Controller
    {
        private readonly ICommonDependencies _c;

        public ElektraKupciController(ICommonDependencies c)
        {
            _c = c;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: ElektraKupci/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ElektraKupac elektraKupac = await _c.UnitOfWork.ElektraKupac.FindExactById((int)id);

            return elektraKupac == null ? NotFound() : View(elektraKupac);
        }

        // GET: ElektraKupci/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["OdsId"] = new SelectList(await _c.UnitOfWork.Ods.GetAll(), "Id", "Id");
            return View();
        }

        // POST: ElektraKupci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,UgovorniRacun,OdsId,Napomena,VrijemeUnosa")] ElektraKupac elektraKupac)
        {
            if (ModelState.IsValid)
            {
                elektraKupac.VrijemeUnosa = DateTime.Now;
                await _c.UnitOfWork.ElektraKupac.Add(elektraKupac);
                _ = await _c.UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OdsId"] = new SelectList(await _c.UnitOfWork.Ods.GetAll(), "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // GET: ElektraKupci/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ElektraKupac elektraKupac = await _c.UnitOfWork.ElektraKupac.FindExactById((int)id);

            if (elektraKupac == null)
            {
                return NotFound();
            }

            try
            {
                ElektraKupacEdit elektraKupacEdit = new()
                {
                    ElektraKupac = elektraKupac,
                    EditingByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    EditTime = DateTime.Now,
                };

                await _c.UnitOfWork.ElektraKupacEdit.Add(elektraKupacEdit);
                _ = await _c.UnitOfWork.Complete();
            }
            catch (Exception)
            {
                // ignored
            }

            ViewData["OdsId"] = new SelectList(await _c.UnitOfWork.Ods.GetAll(), "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // POST: ElektraKupci/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UgovorniRacun,OdsId,Napomena,VrijemeUnosa")] ElektraKupac elektraKupac)
        {
            if (id != elektraKupac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = await _c.UnitOfWork.ElektraKupac.Update(elektraKupac);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ElektraKupacExists(elektraKupac.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                finally
                {
                    _c.UnitOfWork.ElektraKupacEdit.RemoveRange(await _c.UnitOfWork.ElektraKupacEdit.Find(e => e.EditingByUserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))));
                    _ = await _c.UnitOfWork.Complete();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["OdsId"] = new SelectList(await _c.UnitOfWork.Ods.GetAll(), "Id", "Id", elektraKupac.OdsId);
            return View(elektraKupac);
        }

        // GET: ElektraKupci/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ElektraKupac elektraKupac = await _c.UnitOfWork.ElektraKupac.FindExactById((int)id);

            return elektraKupac == null ? NotFound() : View(elektraKupac);
        }

        // POST: ElektraKupci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ElektraKupac elektraKupac = await _c.UnitOfWork.ElektraKupac.Get(id);
            _c.UnitOfWork.ElektraKupac.Remove(elektraKupac);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ElektraKupacExists(int id)
        {
            return await _c.UnitOfWork.ElektraKupac.Any(e => e.Id == id);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // validation

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UgovorniRacunValidation(long ugovorniRacun)
        {

            ElektraKupacEdit elektraKupacEdit = await _c.UnitOfWork.ElektraKupacEdit.GetLastElektraKupacEdit(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (ugovorniRacun is < 1000000000 or > 9999999999)
            {
                return Json($"Ugovorni račun nije ispravan");
            }

            if (elektraKupacEdit is not null && ugovorniRacun == elektraKupacEdit.ElektraKupac.UgovorniRacun)
            {
                return Json(true);
            }

            ElektraKupac db = await _c.UnitOfWork.ElektraKupac.FindExact(ugovorniRacun);
            return db != null ? Json($"Ugovorni račun {ugovorniRacun} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetList()
        {
            IEnumerable<ElektraKupac> list = await _c.UnitOfWork.ElektraKupac.GetAllCustomers();

            return new DatatablesService<ElektraKupac>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetElektraKupciForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetRacuniElektraForElektraKupac(int param)
        {
            IEnumerable<RacunElektra> list = await _c.UnitOfWork.RacuniElektra.GetRacuniForCustomer(param);

            return new DatatablesService<RacunElektra>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniElektraForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetRacuniElektraRateForElektraKupac(int param)
        {
            IEnumerable<RacunElektraRate> list = await _c.UnitOfWork.RacuniElektraRate.GetRacuniForCustomer(param);

            return new DatatablesService<RacunElektraRate>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniElektraRateForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetRacuniElektraIzvrsenjeUslugeForElektraKupac(int param)
        {
            IEnumerable<RacunElektraIzvrsenjeUsluge> list = await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.GetRacuniForCustomer(param);

            return new DatatablesService<RacunElektraIzvrsenjeUsluge>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacunElektraIzvrsenjeUslugeForDatatables);
        }
    }
}
