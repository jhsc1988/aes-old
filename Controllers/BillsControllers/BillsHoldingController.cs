using aes.CommonDependecies;
using aes.Controllers.IControllers;
using aes.Models.Racuni;
using aes.Services;
using aes.Services.BillsServices.BillsHoldingService.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Controllers.BillsControllers
{
    public class BillsHoldingController : Controller, IBillsController
    {

        private readonly IBillsHoldingService _racunHoldingService;
        private readonly IBillsHoldingTempCreateService _billsHoldingTempCreateService;
        private readonly IBillsHoldingUploadService _billsHoldingUploadService;
        private readonly IBillsCommonDependecies _c;
        private readonly ILogger _logger;


        public BillsHoldingController(IBillsHoldingService racunHoldingService, IBillsHoldingTempCreateService billsHoldingTempCreateService,
            IBillsCommonDependecies c, IBillsHoldingUploadService billsHoldingUploadService, ILogger logger)
        {
            _c = c;
            _racunHoldingService = racunHoldingService;
            _billsHoldingTempCreateService = billsHoldingTempCreateService;
            _billsHoldingUploadService = billsHoldingUploadService;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: BillsHolding/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RacunHolding racunHolding = await _c.UnitOfWork.BillsHolding
                .IncludeAll((int)id);

            return racunHolding == null ? NotFound() : View(racunHolding);
        }

        // GET: BillsHolding/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Letter.GetAll(), "Id", "Urbroj");
            ViewData["StanId"] = new SelectList(await _c.UnitOfWork.Apartment.GetAll(), "Id", "Adresa");
            return View();
        }

        // POST: BillsHolding/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,StanId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunHolding racunHolding)
        {
            if (ModelState.IsValid)
            {
                racunHolding.VrijemeUnosa = DateTime.Now;
                _c.UnitOfWork.BillsHolding.Add(racunHolding);
                _ = _c.UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Letter.GetAll(), "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(await _c.UnitOfWork.Apartment.GetAll(), "Id", "Adresa", racunHolding.StanId);
            return View(racunHolding);
        }

        // GET: BillsHolding/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunHolding racunHolding = await _c.UnitOfWork.BillsHolding
                .IncludeAll((int)id);

            if (racunHolding == null)
            {
                return NotFound();
            }

            try
            {

                RacunHoldingEdit racunHoldingEdit = new()
                {
                    RacunHolding = racunHolding,
                    RacunHoldingId = racunHolding.Id,
                    EditingByUserId = _c.Service.GetUid(User),
                    EditTime = DateTime.Now,
                };

                _c.UnitOfWork.BillsHoldingEdit.Add(racunHoldingEdit);
                _ = await _c.UnitOfWork.Complete();
            }
            catch (Exception)
            {

            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Letter.GetAll(), "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(await _c.UnitOfWork.Apartment.GetAll(), "Id", "Adresa", racunHolding.StanId);
            return View(racunHolding);
        }

        // POST: BillsHolding/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,StanId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa,Napomena, IsItTemp, CreatedByUserId")] RacunHolding racunHolding)
        {
            if (id != racunHolding.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = await _c.UnitOfWork.BillsHolding.Update(racunHolding);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunHoldingExists(racunHolding.Id))
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
                    _c.UnitOfWork.BillsHoldingEdit.RemoveRange(await _c.UnitOfWork.BillsHoldingEdit.Find(e => e.EditingByUserId.Equals(_c.Service.GetUid(User))));
                    _ = await _c.UnitOfWork.Complete();
                }

                return racunHolding.IsItTemp == true ? RedirectToAction("Create") : RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Letter.GetAll(), "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(await _c.UnitOfWork.Apartment.GetAll(), "Id", "Adresa", racunHolding.StanId);

            return View(racunHolding);
        }

        // GET: BillsHolding/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunHolding racunHolding = await _c.UnitOfWork.BillsHolding
                .IncludeAll((int)id);

            return racunHolding == null ? NotFound() : View(racunHolding);
        }

        // POST: BillsHolding/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunHolding racunHolding = await _c.UnitOfWork.BillsHolding.Get(id);
            _c.UnitOfWork.BillsHolding.Remove(racunHolding);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunHoldingExists(int id)
        {
            return _c.UnitOfWork.BillsHolding.Any(e => e.Id == id);
        }

        // validation
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {
            RacunHoldingEdit racunHoldingEdit = await _c.UnitOfWork.BillsHoldingEdit.GetLastBillHoldingEdit(_c.Service.GetUid(User));

            if (brojRacuna.Length is not 20
                || brojRacuna[8] is not '-'
                || brojRacuna[18] is not '-'
                || !int.TryParse(brojRacuna.AsSpan(9, 9), out _)
                || !int.TryParse(brojRacuna.AsSpan(19, 1), out _))
            {
                return Json($"Broj računa nije ispravan");
            }

            if (brojRacuna.Length >= 8 && !brojRacuna[..8].Equals(racunHoldingEdit.RacunHolding.BrojRacuna[..8]))
            {
                return Json($"Šifra objekta unutar broja računa ne smije se razlikovati");
            }

            RacunHolding db = await _c.UnitOfWork.BillsHolding.FindExact(x => x.BrojRacuna.Equals(brojRacuna));
            return (db != null && db.IsItTemp != true && !racunHoldingEdit.RacunHolding.BrojRacuna.Equals(brojRacuna)) ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Upload

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            return await _billsHoldingUploadService.Upload(Request, _c.Service.GetUid(User));
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetLettersDataForFilter(int predmetId)
        {
            return Json(await _c.UnitOfWork.BillsHolding.GetLettersForPayedBills(predmetId));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetLettersDataForCreate(int predmetId)
        {
            return Json(await _c.UnitOfWork.Letter.GetOnlyEmptyLetters(predmetId));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCaseFilesForCreate()
        {
            return Json(await _c.UnitOfWork.BillsHolding.GetCaseFilesForCreate());
        }

        [Authorize]
        [HttpPost]
        public async Task<string> GetCustomers()
        {
            return JsonConvert.SerializeObject(await _c.UnitOfWork.Apartment.GetApartments());
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> UpdateDbForInline(string id, string updatedColumn, string x)
        {
            return await _c.BillsInlineEditorService.UpdateDbForInline<RacunHolding>(await _c.UnitOfWork.BillsHolding.FindExact(e => e.Id == int.Parse(id)), updatedColumn, x);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveToDB(string _dopisId)
        {
            return (await _c.UnitOfWork.BillsHolding.TempList(_c.Service.GetUid(User))).Count() is 0
                ? (new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                }))
                : await _billsHoldingTempCreateService.CheckTempTableForBillsWithouCustomer(_c.Service.GetUid(User)) != 0
                ? (new(new { success = false, Message = "U tablici postoje računi bez kupca!" }))
                : await _c.BillsTempEditorService.SaveToDb<RacunHolding>(await _c.UnitOfWork.BillsHolding.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))), _dopisId);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveRow(string racunId)
        {

            _c.UnitOfWork.BillsHolding.Remove(await _c.UnitOfWork.BillsHolding.FindExact(e => e.Id == int.Parse(racunId)));
            _ = await _c.UnitOfWork.Complete();

            IEnumerable<RacunHolding> bills = await _c.UnitOfWork.BillsHolding.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User)));
            int rbr = 1;
            foreach (RacunHolding e in bills)
            {
                e.RedniBroj = rbr++;
            }
            return await _c.Service.TrySave(true);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveAllFromDb()
        {
            if ((await _c.UnitOfWork.BillsHolding.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            _c.UnitOfWork.BillsHolding.RemoveRange(await _c.UnitOfWork.BillsHolding.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))));
            return await _c.Service.TrySave(true);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string date)
        {
            string _loggerTemplate = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + ", " + "User: " + User.Identity.Name + ", " + "msg: ";

            if (brojRacuna is null)
            {
                string message = "brojRacuna ne može biti prazan";

                _logger.Information(_loggerTemplate + message);

                return new(new
                {
                    success = false,
                    message
                });
            }

            return await _billsHoldingTempCreateService.AddNewTemp(brojRacuna, iznos, date, _c.Service.GetUid(User));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCaseFilesDataForFilter()
        {
            return Json(_c.UnitOfWork.CaseFile.GetCaseFilefForAllPayedBills(await _c.UnitOfWork.BillsHolding.GetBillsHoldingWithLettersAndCaseFiles()));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList(bool isFilteredForIndex, string klasa, string urbroj)
        {
            IEnumerable<RacunHolding> list = isFilteredForIndex
                ? await _racunHoldingService.GetList(_racunHoldingService.ParseCaseFile(klasa), _racunHoldingService.ParseLetter(urbroj))
                : await _racunHoldingService.GetCreateBills(_c.Service.GetUid(User));

            return new DatatablesService<RacunHolding>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniHoldingForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RefreshCustomers()
        {
            return await _billsHoldingTempCreateService.RefreshCustomers(_c.Service.GetUid(User));
        }
    }
}
