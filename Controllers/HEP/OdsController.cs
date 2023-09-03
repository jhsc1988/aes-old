using aes.CommonDependecies.ICommonDependencies;
using aes.Controllers.IControllers;
using aes.Models.HEP;
using aes.Services;
using aes.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace aes.Controllers.HEP
{
    public class OdsController : Controller, IOdsController
    {
        private readonly ICommonDependencies _c;
        private readonly IOdsService _odsService;
        public OdsController(IOdsService odsService, ICommonDependencies c)
        {
            _odsService = odsService;
            _c = c;
        }

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

            Ods ods = await _c.UnitOfWork.Ods.IncludeApartment(await _c.UnitOfWork.Ods.Get((int)id));
            return ods == null ? NotFound() : View(ods);
        }

        // GET: Ods/Create
        [Authorize]
        public IActionResult Create()
        {
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
                await _c.UnitOfWork.Ods.Add(ods);
                _ = await _c.UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
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

            Ods ods = await _c.UnitOfWork.Ods.IncludeApartment(await _c.UnitOfWork.Ods.Get((int)id));

            if (ods == null)
            {
                return NotFound();
            }

            try
            {
                OdsEdit odsEdit = new()
                {
                    Ods = ods,
                    EditingByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    EditTime = DateTime.Now,
                };

                await _c.UnitOfWork.OdsEdit.Add(odsEdit);
                _ = await _c.UnitOfWork.Complete();
            }
            catch (Exception)
            {
                // ignored
            }

            ViewData["StanId"] = new SelectList(await _c.UnitOfWork.Stan.GetAll(), "Id", "Adresa", ods.StanId);
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
                    _ = await _c.UnitOfWork.Ods.Update(ods);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await OdsExists(ods.Id))
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
                    _c.UnitOfWork.OdsEdit.RemoveRange(await _c.UnitOfWork.OdsEdit.Find(e => e.EditingByUserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))));
                    _ = await _c.UnitOfWork.Complete();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StanId"] = new SelectList(await _c.UnitOfWork.Stan.GetAll(), "Id", "Adresa", ods.StanId);
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

            Ods ods = await _c.UnitOfWork.Ods.IncludeApartment(await _c.UnitOfWork.Ods.Get((int)id));
            return ods == null ? NotFound() : View(ods);
        }

        // POST: Ods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Ods ods = await _c.UnitOfWork.Ods.Get(id);
            _c.UnitOfWork.Ods.Remove(ods);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OdsExists(int id)
        {
            return await _c.UnitOfWork.Ods.Any(e => e.Id == id);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // validation

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> OmmValidation(int omm)
        {
            OdsEdit odsEdit = await _c.UnitOfWork.OdsEdit.GetLastOdsEdit(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (omm is < 10000000 or > 99999999)
            {
                return Json($"Broj obračunskog mjernog mjesta nije ispravan");
            }

            if (odsEdit is not null && omm == odsEdit.Ods.Omm)
            {
                return Json(true);
            }

            Ods db = await _c.UnitOfWork.Ods.FindExact(x => x.Omm == omm);
            return db != null ? Json($"Obračunsko mjerno mjesto {omm} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetStanData(string sid)
        {
            return await _odsService.GetStanData(sid);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetStanDataForOmm(string odsId)
        {
            return await _odsService.GetStanDataForOmm(odsId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetList()
        {
            IEnumerable<Ods> list = await _c.UnitOfWork.Ods.GetAllOds();

            return new DatatablesService<Ods>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetStanoviOdsForDatatables);
        }
    }
}
