using aes.CommonDependecies;
using aes.Controllers.IControllers;
using aes.Models.Racuni;
using aes.Services;
using aes.Services.BillsServices.BillsElektraServices.BillsElektraAdvances.Is;
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

namespace aes.Controllers.BillsControllers.BillsElektraControllers
{
    public class BillsElektraAdvancesController : Controller, IBillsController
    {
        private readonly IBillsElektraAdvancesTempCreateService _billsElektraAdvancesTempCreateService;
        private readonly IBillsElektraAdvancesService _billsElektraAdvancesService;
        private readonly IBillsElektraAdvancesUploadService _billsElektraAdvancesUploadService;
        private readonly IBillsCommonDependecies _c;
        private readonly ILogger _logger;


        public BillsElektraAdvancesController(IBillsElektraAdvancesTempCreateService billsElektraAdvancesTempCreateService, IBillsElektraAdvancesService billsElektraAdvancesService,
            IBillsCommonDependecies c, IBillsElektraAdvancesUploadService billsElektraAdvancesUploadService, ILogger logger)
        {
            _c = c;
            _billsElektraAdvancesTempCreateService = billsElektraAdvancesTempCreateService;
            _billsElektraAdvancesService = billsElektraAdvancesService;
            _billsElektraAdvancesUploadService = billsElektraAdvancesUploadService;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: BillsElektraAdvances/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraRate racunElektraRate = await _c.UnitOfWork.BillsElektraAdvances.IncludeAll((int)id);

            return racunElektraRate == null ? NotFound() : View(racunElektraRate);
        }

        // GET: BillsElektraAdvances/Create
        // GET: BillsElektra/Create
        [Authorize]
        public IActionResult CreateAsync()
        {
            //List<RacunElektraRate> applicationDbContext = await _context.RacunElektraRate.ToListAsync();

            //return View(applicationDbContext);
            return View();
        }

        // POST: BillsElektraAdvances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,ElektraKupacId,Razdoblje,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa, Napomena")] RacunElektraRate racunElektraRate)
        {
            if (ModelState.IsValid)
            {
                racunElektraRate.VrijemeUnosa = DateTime.Now;
                _c.UnitOfWork.BillsElektraAdvances.Add(racunElektraRate);
                _ = await _c.UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(racunElektraRate);
        }

        // GET: BillsElektraAdvances/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraRate racunElektraRate = await _c.UnitOfWork.BillsElektraAdvances.IncludeAll((int)id);

            if (racunElektraRate == null)
            {
                return NotFound();
            }

            try
            {

                RacunElektraRateEdit racunElektraRateEdit = new()
                {
                    RacunElektraRate = racunElektraRate,
                    RacunElektraRateId = racunElektraRate.Id,
                    EditingByUserId = _c.Service.GetUid(User),
                    EditTime = DateTime.Now,
                };

                _c.UnitOfWork.BillsElektraAdvancesEdit.Add(racunElektraRateEdit);
                _ = await _c.UnitOfWork.Complete();
            }
            catch (Exception e)
            {

            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Letter.GetAll(), "Id", "Urbroj", racunElektraRate.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraCustomer.GetAll(), "Id", "Id", racunElektraRate.ElektraKupacId);
            return View(racunElektraRate);
        }

        // POST: BillsElektraAdvances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,ElektraKupacId,Razdoblje,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa,Napomena, IsItTemp, CreatedByUserId")] RacunElektraRate racunElektraRate)
        {
            if (id != racunElektraRate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = await _c.UnitOfWork.BillsElektraAdvances.Update(racunElektraRate);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunElektraRateExists(racunElektraRate.Id))
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
                    _c.UnitOfWork.BillsElektraAdvancesEdit.RemoveRange(await _c.UnitOfWork.BillsElektraAdvancesEdit.Find(e => e.EditingByUserId.Equals(_c.Service.GetUid(User))));
                    _ = await _c.UnitOfWork.Complete();
                }

                return racunElektraRate.IsItTemp == true ? RedirectToAction("Create") : RedirectToAction(nameof(Index));
            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Letter.GetAll(), "Id", "Urbroj", racunElektraRate.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraCustomer.GetAll(), "Id", "Id", racunElektraRate.ElektraKupacId);

            return View(racunElektraRate);
        }

        // GET: BillsElektraAdvances/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraRate racunElektraRate = await _c.UnitOfWork.BillsElektraAdvances.IncludeAll((int)id);

            return racunElektraRate == null ? NotFound() : View(racunElektraRate);
        }

        // POST: BillsElektraAdvances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunElektraRate racunElektraRate = await _c.UnitOfWork.BillsElektraAdvances.Get(id);
            _c.UnitOfWork.BillsElektraAdvances.Remove(racunElektraRate);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraRateExists(int id)
        {
            return _c.UnitOfWork.BillsElektraAdvances.Any(e => e.Id == id);
        }

        // validation
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {

            RacunElektraRateEdit racunElektraRateEdit = await _c.UnitOfWork.BillsElektraAdvancesEdit.GetLastBillElektraAdvancesEdit(_c.Service.GetUid(User));

            if (brojRacuna.Length is not 19
                || brojRacuna[10] is not '-'
                || brojRacuna[17] is not '-'
                || !int.TryParse(brojRacuna.AsSpan(11, 6), out _)
                || !int.TryParse(brojRacuna.AsSpan(18, 1), out _))
            {
                return Json($"Broj računa nije ispravan");
            }

            if (brojRacuna.Length >= 10 && !brojRacuna[..10].Equals(racunElektraRateEdit.RacunElektraRate.BrojRacuna[..10]))
            {
                return Json($"Ugovorni račun unutar broja računa ne smije se razlikovati");
            }

            RacunElektraRate db = await _c.UnitOfWork.BillsElektraAdvances.FindExact(x => x.BrojRacuna.Equals(brojRacuna));
            return (db != null && db.IsItTemp != true && !racunElektraRateEdit.RacunElektraRate.BrojRacuna.Equals(brojRacuna)) ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Upload

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            return await _billsElektraAdvancesUploadService.Upload(Request, _c.Service.GetUid(User));
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetLettersDataForFilter(int predmetId)
        {
            return Json(await _c.UnitOfWork.BillsElektraAdvances.GetLettersForPayedBillsElektraAdvances(predmetId));
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
            return Json(await _c.UnitOfWork.BillsElektra.GetCaseFilesForCreate());
        }

        [Authorize]
        [HttpPost]
        public async Task<string> GetCustomers()
        {
            return JsonConvert.SerializeObject(await _c.UnitOfWork.ElektraCustomer.GetAllCustomers());
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> UpdateDbForInline(string id, string updatedColumn, string x)
        {
            return await _c.BillsInlineEditorService.UpdateDbForInline<RacunElektraRate>(await _c.UnitOfWork.BillsElektraAdvances.FindExact(e => e.Id == int.Parse(id)), updatedColumn, x);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveToDB(string _dopisId)
        {
            if ((await _c.UnitOfWork.BillsElektraAdvances.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            return await _billsElektraAdvancesTempCreateService.CheckTempTableForBillsWithousElectraCustomer(_c.Service.GetUid(User)) != 0
                ? (new(new { success = false, Message = "U tablici postoje računi bez kupca!" }))
                : await _c.BillsTempEditorService.SaveToDb<RacunElektraRate>(await _c.UnitOfWork.BillsElektraAdvances.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))), _dopisId);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveRow(string racunId)
        {
            _c.UnitOfWork.BillsElektraAdvances.Remove(await _c.UnitOfWork.BillsElektraAdvances.FindExact(e => e.Id == int.Parse(racunId)));
            _ = await _c.UnitOfWork.Complete();

            IEnumerable<RacunElektraRate> list = await _c.UnitOfWork.BillsElektraAdvances.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User)));
            int rbr = 1;
            foreach (RacunElektraRate e in list)
            {
                e.RedniBroj = rbr++;
            }
            return await _c.Service.TrySave(true);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveAllFromDb()
        {
            if ((await _c.UnitOfWork.BillsElektraAdvances.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }
            _c.UnitOfWork.BillsElektraAdvances.RemoveRange(await _c.UnitOfWork.BillsElektraAdvances.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))));
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

            return await _billsElektraAdvancesTempCreateService.AddNewTemp(brojRacuna, iznos, date, _c.Service.GetUid(User));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCaseFilesDataForFilter()
        {
            return Json(_c.UnitOfWork.CaseFile.GetCaseFilefForAllPayedBills(await _c.UnitOfWork.BillsElektraAdvances.GetBillsElektraAdvancesWithLettersAndCaseFiles()));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList(bool isFilteredForIndex, string klasa, string urbroj)
        {
            IEnumerable<RacunElektraRate> list = isFilteredForIndex
                ? await _billsElektraAdvancesService.GetList(_billsElektraAdvancesService.ParseCaseFile(klasa), _billsElektraAdvancesService.ParseLetter(urbroj))
                : await _billsElektraAdvancesService.GetCreateBills(_c.Service.GetUid(User));

            return await new DatatablesService<RacunElektraRate>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniElektraRateForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RefreshCustomers()
        {
            return await _billsElektraAdvancesTempCreateService.RefreshCustomers(_c.Service.GetUid(User));
        }
    }
}
