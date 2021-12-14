using aes.CommonDependecies;
using aes.Controllers.IControllers;
using aes.Models.Racuni;
using aes.Services;
using aes.Services.BillsServices.BillsElektraServices.BillsElektraServices.Is;
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
    public class BillsElektraServicesController : Controller, IBillsController
    {
        private readonly IBillsElektraServicesTempCreateService _billsElektraServicesTempCreateService;
        private readonly IBillsElektraServicesService _billsElektraServicesService;
        private readonly IBillsCommonDependecies _c;
        private readonly ILogger _logger;

        public BillsElektraServicesController(IBillsElektraServicesTempCreateService billsElektraServicesTempCreateService, IBillsElektraServicesService billsElektraServicesService,
            IBillsCommonDependecies c, ILogger logger)
        {
            _c = c;
            _billsElektraServicesTempCreateService = billsElektraServicesTempCreateService;
            _billsElektraServicesService = billsElektraServicesService;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: BillsElektraServices/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge = await _c.UnitOfWork.BillsElektraServices.IncludeAll((int)id);

            return racunElektraIzvrsenjeUsluge == null ? NotFound() : View(racunElektraIzvrsenjeUsluge);
        }

        // GET: BillsElektraServices/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Letter.GetAll(), "Id", "Urbroj");
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraCustomer.GetAll(), "Id", "Id");
            return View();
        }

        // POST: BillsElektraServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa, Napomena")] RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge)
        {
            if (ModelState.IsValid)
            {
                racunElektraIzvrsenjeUsluge.VrijemeUnosa = DateTime.Now;
                _c.UnitOfWork.BillsElektraServices.Add(racunElektraIzvrsenjeUsluge);
                _ = await _c.UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Letter.GetAll(), "Id", "Urbroj", racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraCustomer.GetAll(), "Id", "Id", racunElektraIzvrsenjeUsluge.ElektraKupacId);

            return View(racunElektraIzvrsenjeUsluge);
        }

        // GET: BillsElektraServices/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge = await _c.UnitOfWork.BillsElektraServices.IncludeAll((int)id);

            if (racunElektraIzvrsenjeUsluge == null)
            {
                return NotFound();
            }

            try
            {

                RacunElektraIzvrsenjeUslugeEdit racunElektraIzvrsenjeUslugeEdit = new()
                {
                    RacunElektraIzvrsenjeUsluge = racunElektraIzvrsenjeUsluge,
                    RacunElektraIzvrsenjeUslugeId = racunElektraIzvrsenjeUsluge.Id,
                    EditingByUserId = _c.Service.GetUid(User),
                    EditTime = DateTime.Now,
                };

                _c.UnitOfWork.BillsElektraServicesEdit.Add(racunElektraIzvrsenjeUslugeEdit);
                _ = await _c.UnitOfWork.Complete();
            }
            catch (Exception e)
            {

            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Letter.GetAll(), "Id", "Urbroj", racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraCustomer.GetAll(), "Id", "Id", racunElektraIzvrsenjeUsluge.ElektraKupacId);
            return View(racunElektraIzvrsenjeUsluge);
        }

        // POST: BillsElektraServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa,Napomena, IsItTemp, CreatedByUserId")] RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge)
        {
            if (id != racunElektraIzvrsenjeUsluge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = await _c.UnitOfWork.BillsElektraServices.Update(racunElektraIzvrsenjeUsluge);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunElektraIzvrsenjeUslugeExists(racunElektraIzvrsenjeUsluge.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return racunElektraIzvrsenjeUsluge.IsItTemp == true ? RedirectToAction("Create") : RedirectToAction(nameof(Index));
            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Letter.GetAll(), "Id", "Urbroj", racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraCustomer.GetAll(), "Id", "Id", racunElektraIzvrsenjeUsluge.ElektraKupacId);
            return View(racunElektraIzvrsenjeUsluge);
        }

        // GET: BillsElektraServices/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge = await _c.UnitOfWork.BillsElektraServices.IncludeAll((int)id);

            return racunElektraIzvrsenjeUsluge == null ? NotFound() : View(racunElektraIzvrsenjeUsluge);
        }

        // POST: BillsElektraServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge = await _c.UnitOfWork.BillsElektraServices.Get(id);
            _c.UnitOfWork.BillsElektraServices.Remove(racunElektraIzvrsenjeUsluge);
            _ = await _c.UnitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraIzvrsenjeUslugeExists(int id)
        {
            return _c.UnitOfWork.BillsElektraServices.Any(e => e.Id == id);
        }

        // validation
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {
            RacunElektraIzvrsenjeUslugeEdit racunElektraUslugeEdit = await _c.UnitOfWork.BillsElektraServicesEdit.GetLastBillElektraServiceEdit(_c.Service.GetUid(User));

            if (brojRacuna.Length is not 19
                || brojRacuna[10] is not '-'
                || brojRacuna[17] is not '-'
                || !int.TryParse(brojRacuna.AsSpan(11, 6), out _)
                || !int.TryParse(brojRacuna.AsSpan(18, 1), out _))
            {
                return Json($"Broj računa nije ispravan");
            }

            if (brojRacuna.Length >= 10 && !brojRacuna[..10].Equals(racunElektraUslugeEdit.RacunElektraIzvrsenjeUsluge.BrojRacuna[..10]))
            {
                return Json($"Ugovorni račun unutar broja računa ne smije se razlikovati");
            }

            RacunElektraIzvrsenjeUsluge db = await _c.UnitOfWork.BillsElektraServices.FindExact(x => x.BrojRacuna.Equals(brojRacuna));
            return (db != null && db.IsItTemp != true && !racunElektraUslugeEdit.RacunElektraIzvrsenjeUsluge.BrojRacuna.Equals(brojRacuna)) ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetLettersDataForFilter(int predmetId)
        {
            return Json(await _c.UnitOfWork.BillsElektraServices.GetLettersForPayedBillsElektraServices(predmetId));
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
            return await _c.BillsInlineEditorService.UpdateDbForInline<RacunElektraIzvrsenjeUsluge>(await _c.UnitOfWork.BillsElektraServices.FindExact(e => e.Id == int.Parse(id)), updatedColumn, x);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveToDB(string _dopisId)
        {
            if ((await _c.UnitOfWork.BillsElektraServices.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            return await _billsElektraServicesTempCreateService.CheckTempTableForBillsWithousElectraCustomer(_c.Service.GetUid(User)) != 0
                ? (new(new { success = false, Message = "U tablici postoje računi bez kupca!" }))
                : await _c.BillsTempEditorService.SaveToDb<RacunElektraIzvrsenjeUsluge>(await _c.UnitOfWork.BillsElektraServices
                .Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))), _dopisId);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveRow(string racunId)
        {
            _c.UnitOfWork.BillsElektraServices.Remove(await _c.UnitOfWork.BillsElektraServices.FindExact(e => e.Id == int.Parse(racunId)));
            _ = await _c.UnitOfWork.Complete();

            IEnumerable<RacunElektraIzvrsenjeUsluge> list = await _c.UnitOfWork.BillsElektraServices.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User)));
            int rbr = 1;
            foreach (RacunElektraIzvrsenjeUsluge e in list)
            {
                e.RedniBroj = rbr++;
            }
            return await _c.Service.TrySave(true);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveAllFromDb()
        {
            if ((await _c.UnitOfWork.BillsElektraServices.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            _c.UnitOfWork.BillsElektraServices.RemoveRange(await _c.UnitOfWork.BillsElektraServices.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))));
            return await _c.Service.TrySave(true);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string date, string datumIzvrsenja, string usluga, string dopisId)
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

            return await _billsElektraServicesTempCreateService.AddNewTemp(brojRacuna, iznos, date, datumIzvrsenja, usluga, dopisId, _c.Service.GetUid(User));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCaseFilesDataForFilter()
        {
            return Json(_c.UnitOfWork.CaseFile.GetCaseFilefForAllPayedBills(await _c.UnitOfWork.BillsElektraServices.GetBillsElektraServicesWithLettersAndCaseFiles()));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList(bool isFilteredForIndex, string klasa, string urbroj)
        {

            IEnumerable<RacunElektraIzvrsenjeUsluge> list = isFilteredForIndex
                ? await _billsElektraServicesService.GetList(_billsElektraServicesService.ParseCaseFile(klasa), _billsElektraServicesService.ParseLetter(urbroj))
                : await _billsElektraServicesService.GetCreateBills(_c.Service.GetUid(User));

            return await new DatatablesService<RacunElektraIzvrsenjeUsluge>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacunElektraIzvrsenjeUslugeForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetLettersDataForCreate(int predmetId)
        {
            return Json(await _c.UnitOfWork.Letter.GetOnlyEmptyLetters(predmetId));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RefreshCustomers()
        {
            return await _billsElektraServicesTempCreateService.RefreshCustomers(_c.Service.GetUid(User));
        }
    }
}
