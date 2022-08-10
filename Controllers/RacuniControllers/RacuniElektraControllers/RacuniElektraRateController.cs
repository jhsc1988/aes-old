using aes.CommonDependecies;
using aes.Controllers.IControllers;
using aes.Models.Racuni;
using aes.Services;
using aes.Services.RacuniServices.RacuniElektraIzvrsenjeUsluge.RacuniElektraRate.Is;
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

namespace aes.Controllers.RacuniControllers.RacuniElektraControllers
{
    public class RacuniElektraRateController : Controller, IRacuniController
    {
        private readonly IRacuniElektraRateTempCreateService _RacuniElektraRateTempCreateService;
        private readonly IRacuniElektraRateService _RacuniElektraRateService;
        private readonly IRacuniElektraRateUploadService _RacuniElektraRateUploadService;
        private readonly IRacuniCommonDependecies _c;
        private readonly ILogger _logger;


        public RacuniElektraRateController(IRacuniElektraRateTempCreateService RacuniElektraRateTempCreateService, IRacuniElektraRateService RacuniElektraRateService,
            IRacuniCommonDependecies c, IRacuniElektraRateUploadService RacuniElektraRateUploadService, ILogger logger)
        {
            _c = c;
            _RacuniElektraRateTempCreateService = RacuniElektraRateTempCreateService;
            _RacuniElektraRateService = RacuniElektraRateService;
            _RacuniElektraRateUploadService = RacuniElektraRateUploadService;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: RacuniElektraRate/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraRate racunElektraRate = await _c.UnitOfWork.RacuniElektraRate.IncludeAll((int)id);

            return racunElektraRate == null ? NotFound() : View(racunElektraRate);
        }

        // GET: RacuniElektraRate/Create
        // GET: RacuniElektra/Create
        [Authorize]
        public IActionResult CreateAsync()
        {
            //List<RacunElektraRate> applicationDbContext = await _context.RacunElektraRate.ToListAsync();

            //return View(applicationDbContext);
            return View();
        }

        // POST: RacuniElektraRate/Create
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
                _c.UnitOfWork.RacuniElektraRate.Add(racunElektraRate);
                _ = await _c.UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(racunElektraRate);
        }

        // GET: RacuniElektraRate/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraRate racunElektraRate = await _c.UnitOfWork.RacuniElektraRate.IncludeAll((int)id);

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

                _c.UnitOfWork.RacuniElektraRateEdit.Add(racunElektraRateEdit);
                _ = await _c.UnitOfWork.Complete();
            }
            catch (Exception)
            {

            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj", racunElektraRate.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraKupac.GetAll(), "Id", "Id", racunElektraRate.ElektraKupacId);
            return View(racunElektraRate);
        }

        // POST: RacuniElektraRate/Edit/5
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
                    _ = await _c.UnitOfWork.RacuniElektraRate.Update(racunElektraRate);

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
                    _c.UnitOfWork.RacuniElektraRateEdit.RemoveRange(await _c.UnitOfWork.RacuniElektraRateEdit.Find(e => e.EditingByUserId.Equals(_c.Service.GetUid(User))));
                    _ = await _c.UnitOfWork.Complete();
                }

                return racunElektraRate.IsItTemp == true ? RedirectToAction("Create") : RedirectToAction(nameof(Index));
            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj", racunElektraRate.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraKupac.GetAll(), "Id", "Id", racunElektraRate.ElektraKupacId);

            return View(racunElektraRate);
        }

        // GET: RacuniElektraRate/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraRate racunElektraRate = await _c.UnitOfWork.RacuniElektraRate.IncludeAll((int)id);

            return racunElektraRate == null ? NotFound() : View(racunElektraRate);
        }

        // POST: RacuniElektraRate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunElektraRate racunElektraRate = await _c.UnitOfWork.RacuniElektraRate.Get(id);
            _c.UnitOfWork.RacuniElektraRate.Remove(racunElektraRate);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraRateExists(int id)
        {
            return _c.UnitOfWork.RacuniElektraRate.Any(e => e.Id == id);
        }

        // validation
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {

            RacunElektraRateEdit racunElektraRateEdit = await _c.UnitOfWork.RacuniElektraRateEdit.GetLastRacunElektraRateEdit(_c.Service.GetUid(User));

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

            RacunElektraRate db = await _c.UnitOfWork.RacuniElektraRate.FindExact(x => x.BrojRacuna.Equals(brojRacuna));
            return (db != null && db.IsItTemp != true && !racunElektraRateEdit.RacunElektraRate.BrojRacuna.Equals(brojRacuna)) ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Upload

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            return await _RacuniElektraRateUploadService.Upload(Request, _c.Service.GetUid(User));
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetDopisiDataForFilter(int predmetId)
        {
            return Json(await _c.UnitOfWork.RacuniElektraRate.GetDopisiForPayedRacuniElektraRate(predmetId));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetDopisiDataForCreate(int predmetId)
        {
            return Json(await _c.UnitOfWork.Dopis.GetOnlyEmptyDopisi(predmetId));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetPredmetiForCreate()
        {
            return Json(await _c.UnitOfWork.RacuniElektra.GetPredmetiForCreate());
        }

        [Authorize]
        [HttpPost]
        public async Task<string> GetCustomers()
        {
            return JsonConvert.SerializeObject(await _c.UnitOfWork.ElektraKupac.GetAllCustomers());
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> UpdateDbForInline(string id, string updatedColumn, string x)
        {
            return await _c.RacuniInlineEditorService.UpdateDbForInline<RacunElektraRate>(await _c.UnitOfWork.RacuniElektraRate.FindExact(e => e.Id == int.Parse(id)), updatedColumn, x);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveToDB(string _dopisId)
        {
            if ((await _c.UnitOfWork.RacuniElektraRate.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            return await _RacuniElektraRateTempCreateService.CheckTempTableForRacuniWithousElectraCustomer(_c.Service.GetUid(User)) != 0
                ? (new(new { success = false, Message = "U tablici postoje računi bez kupca!" }))
                : await _c.RacuniTempEditorService.SaveToDb<RacunElektraRate>(await _c.UnitOfWork.RacuniElektraRate.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))), _dopisId);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveRow(string racunId)
        {
            _c.UnitOfWork.RacuniElektraRate.Remove(await _c.UnitOfWork.RacuniElektraRate.FindExact(e => e.Id == int.Parse(racunId)));
            _ = await _c.UnitOfWork.Complete();

            IEnumerable<RacunElektraRate> list = await _c.UnitOfWork.RacuniElektraRate.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User)));
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
            if ((await _c.UnitOfWork.RacuniElektraRate.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }
            _c.UnitOfWork.RacuniElektraRate.RemoveRange(await _c.UnitOfWork.RacuniElektraRate.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))));
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

            return await _RacuniElektraRateTempCreateService.AddNewTemp(brojRacuna, iznos, date, _c.Service.GetUid(User));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetPredmetiDataForFilter()
        {
            return Json(_c.UnitOfWork.Predmet.GetPredmetfForAllPayedRacuni(await _c.UnitOfWork.RacuniElektraRate.GetRacuniElektraRateWithDopisiAndPredmeti()));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList(bool isFilteredForIndex, string klasa, string urbroj)
        {
            IEnumerable<RacunElektraRate> list = isFilteredForIndex
                ? await _RacuniElektraRateService.GetList(_RacuniElektraRateService.ParsePredmet(klasa), _RacuniElektraRateService.ParseDopis(urbroj))
                : await _RacuniElektraRateService.GetCreateRacuni(_c.Service.GetUid(User));

            return new DatatablesService<RacunElektraRate>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniElektraRateForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RefreshCustomers()
        {
            return await _RacuniElektraRateTempCreateService.RefreshCustomers(_c.Service.GetUid(User));
        }
    }
}
