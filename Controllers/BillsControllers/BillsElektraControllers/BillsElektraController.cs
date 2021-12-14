﻿using aes.CommonDependecies;
using aes.Controllers.IControllers;
using aes.Data;
using aes.Models.Racuni;
using aes.Services;
using aes.Services.BillsServices.BillsElektraServices.BillsElektra.Is;
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
    public class BillsElektraController : Controller, IBillsController
    {
        private readonly IBillsElektraTempCreateService _billsElektraTempCreateService;
        private readonly IBillsElektraService _billsElektraService;
        private readonly IBillsElektraUploadService _billsElektraUploadService;
        private readonly IBillsCommonDependecies _c;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public BillsElektraController(IBillsElektraTempCreateService billsElektraTempCreateService,
            IBillsElektraService racuniElektraIRateWorkshop, IBillsCommonDependecies c,
            IBillsElektraUploadService billsElektraUploadService, ILogger logger, ApplicationDbContext context)
        {
            _c = c;
            _billsElektraTempCreateService = billsElektraTempCreateService;
            _billsElektraService = racuniElektraIRateWorkshop;
            _billsElektraUploadService = billsElektraUploadService;
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: BillsElektra/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["RacunElektraId"] = new SelectList(_context.RacunElektra, "Id", "BrojRacuna");
            ViewData["TarifnaStavkaId"] = new SelectList(_context.TarifnaStavka, "Id", "Naziv");

            RacunElektra racunElektra = await _c.UnitOfWork.BillsElektra.IncludeAll((int)id);

            ObracunPotrosnje obracunPotrosnje = new()
            {
                Id = racunElektra.Id,
                BrojBrojila = 0,
                DatumOd = DateTime.Now,
                DatumDo = DateTime.Now,
                StanjeOd = 0,
                StanjeDo = 0,
                TarifnaStavka = new TarifnaStavka(),
            };

            Tuple<RacunElektra, ObracunPotrosnje> tupleModel = new(racunElektra, obracunPotrosnje);

            return racunElektra == null ? NotFound() : View(tupleModel);
        }

        // GET: BillsElektra/Create
        [Authorize]
        public IActionResult CreateAsync() // todo: treba vidjeti sto ovaj CreateAsync tocno radi
        {
            //List<RacunElektra> applicationDbContext = await _context.RacunElektra.ToListAsync();
            return View(new RacunElektra());
        }

        // POST: BillsElektra/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(
            [Bind("BrojRacuna,DatumIzdavanja,Iznos")] RacunElektra racunElektra)
        {
            if (ModelState.IsValid)
            {
                _ = await _billsElektraTempCreateService.AddNewTemp(racunElektra.BrojRacuna, racunElektra.Iznos.ToString(), racunElektra.DatumIzdavanja?.ToString(), _c.Service.GetUid(User));
            }

            ModelState.Clear();

            return View();
        }

        // GET: BillsElektra/Edit/5

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektra racunElektra = await _c.UnitOfWork.BillsElektra.IncludeAll((int)id);

            if (racunElektra == null)
            {
                return NotFound();
            }

            try
            {

                RacunElektraEdit racunElektraEdit = new()
                {
                    RacunElektra = racunElektra,
                    RacunElektraId = racunElektra.Id,
                    EditingByUserId = _c.Service.GetUid(User),
                    EditTime = DateTime.Now,
                };

                _c.UnitOfWork.BillsElektraEdit.Add(racunElektraEdit);
                _ = await _c.UnitOfWork.Complete();
            }
            catch (Exception e)
            {

            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Letter.GetAll(), "Id", "Urbroj", racunElektra.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraCustomer.GetAll(), "Id", "Id", racunElektra.ElektraKupacId);

            return View(racunElektra);
        }

        // POST: BillsElektra/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa,Napomena, IsItTemp, CreatedByUserId")]
            RacunElektra racunElektra)
        {
            if (id != racunElektra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = await _c.UnitOfWork.BillsElektra.Update(racunElektra);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacunElektraExists(racunElektra.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return racunElektra.IsItTemp == true ? RedirectToAction("Create") : RedirectToAction(nameof(Index));
            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Letter.GetAll(), "Id", "Urbroj", racunElektra.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraCustomer.GetAll(), "Id", "Id", racunElektra.ElektraKupacId);

            return View(racunElektra);
        }

        // GET: BillsElektra/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektra racunElektra = await _c.UnitOfWork.BillsElektra.IncludeAll((int)id);

            return racunElektra == null ? NotFound() : View(racunElektra);
        }

        // POST: BillsElektra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunElektra racunElektra = await _c.UnitOfWork.BillsElektra.Get(id);
            _c.UnitOfWork.BillsElektra.Remove(racunElektra);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraExists(int id)
        {
            return _c.UnitOfWork.BillsElektra.Any(e => e.Id == id);
        }

        [Authorize]
        [HttpGet]
        public async Task<JsonResult> BrojRacunaValidation(string brojRacuna)
        {
            RacunElektraEdit racunElektraEdit = await _c.UnitOfWork.BillsElektraEdit.GetLastBillElektraEdit(_c.Service.GetUid(User));

            if (brojRacuna.Length is not 19
                || brojRacuna[10] is not '-'
                || brojRacuna[17] is not '-'
                || !int.TryParse(brojRacuna.AsSpan(11, 6), out _)
                || !int.TryParse(brojRacuna.AsSpan(18, 1), out _))
            {
                return Json($"Broj računa nije ispravan");
            }

            if (racunElektraEdit != null && brojRacuna.Length >= 10 && !brojRacuna[..10].Equals(racunElektraEdit.RacunElektra.BrojRacuna[..10]))
            {
                return Json($"Ugovorni račun unutar broja računa ne smije se razlikovati");
            }

            RacunElektra db = await _c.UnitOfWork.BillsElektra.FindExact(x => x.BrojRacuna.Equals(brojRacuna));
            return (db != null && db.IsItTemp != true && !racunElektraEdit.RacunElektra.BrojRacuna.Equals(brojRacuna)) ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Upload

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            return await _billsElektraUploadService.Upload(Request, _c.Service.GetUid(User));
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetLettersDataForFilter(int predmetId)
        {
            return Json(await _c.UnitOfWork.BillsElektra.GetLettersForPayedBillsElektra(predmetId));
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
            return await _c.BillsInlineEditorService.UpdateDbForInline<RacunElektra>(await _c.UnitOfWork.BillsElektra.FindExact(e => e.Id == int.Parse(id)), updatedColumn, x);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveToDB(string _dopisId)
        {
            if ((await _c.UnitOfWork.BillsElektra.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            return await _billsElektraTempCreateService.CheckTempTableForBillsWithousElektraCustomer(_c.Service.GetUid(User)) != 0
                ? (new(new { success = false, Message = "U tablici postoje računi bez kupca!" }))
                : await _c.BillsTempEditorService.SaveToDb<RacunElektra>(await _c.UnitOfWork.BillsElektra
                .Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))), _dopisId);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveRow(string racunId)
        {
            _c.UnitOfWork.BillsElektra.Remove(await _c.UnitOfWork.BillsElektra.FindExact(e => e.Id == int.Parse(racunId)));
            _ = await _c.UnitOfWork.Complete();

            IEnumerable<RacunElektra> list = await _c.UnitOfWork.BillsElektra.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User)));
            int rbr = 1;
            foreach (RacunElektra e in list)
            {
                e.RedniBroj = rbr++;
            }
            return await _c.Service.TrySave(true);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveAllFromDb()
        {
            if ((await _c.UnitOfWork.BillsElektra.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            _c.UnitOfWork.BillsElektra.RemoveRange(await _c.UnitOfWork.BillsElektra.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))));
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

            return await _billsElektraTempCreateService.AddNewTemp(brojRacuna, iznos, date, _c.Service.GetUid(User));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCaseFilesDataForFilter()
        {
            return Json(_c.UnitOfWork.CaseFile.GetCaseFilefForAllPayedBills(await _c.UnitOfWork.BillsElektra.GetBillsElektraWithLettersAndCaseFiles()));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList(bool isFilteredForIndex, string klasa, string urbroj)
        {

            IEnumerable<RacunElektra> list = isFilteredForIndex
                ? await _billsElektraService.GetList(_billsElektraService.ParseCaseFile(klasa), _billsElektraService.ParseLetter(urbroj))
                : await _billsElektraService.GetCreateBills(_c.Service.GetUid(User));

            return await new DatatablesService<RacunElektra>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniElektraForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RefreshCustomers()
        {
            return await _billsElektraTempCreateService.RefreshCustomers(_c.Service.GetUid(User));
        }

        [Authorize]
        public async Task<JsonResult> GetObracunPotrosnjeForBill(int billId)
        {
            IEnumerable<ObracunPotrosnje> list = await _c.UnitOfWork.ObracunPotrosnje.GetObracunPotrosnjeForBill(billId);

            return await new DatatablesService<ObracunPotrosnje>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetObracunPotrosnjeDatatables);
        }

        [Authorize]
        public async Task<IActionResult> CreateObracunPotrosnje([Bind("Id,RacunElektraId,BrojBrojila,TarifnaStavkaId,DatumOd,DatumDo,StanjeOd,StanjeDo")] ObracunPotrosnje obracunPotrosnje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(obracunPotrosnje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RacunElektraId"] = new SelectList(_context.RacunElektra, "Id", "BrojRacuna", obracunPotrosnje.RacunElektraId);
            ViewData["TarifnaStavkaId"] = new SelectList(_context.TarifnaStavka, "Id", "Id", obracunPotrosnje.TarifnaStavkaId);
            return View(obracunPotrosnje);
        }


    }
}
