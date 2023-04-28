using aes.CommonDependecies.ICommonDependencies;
using aes.Controllers.IControllers;
using aes.Models.Racuni.Elektra;
using aes.Services;
using aes.Services.RacuniServices.Elektra.RacuniElektraIzvrsenjeUsluge.Is;
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
    public class RacuniElektraIzvrsenjeUslugeController : Controller, IRacuniController
    {
        private readonly IRacuniElektraIzvrsenjeUslugeTempCreateService _RacuniElektraIzvrsenjeUslugeTempCreateService;
        private readonly IRacuniElektraIzvrsenjeUslugeService _RacuniElektraIzvrsenjeUslugeService;
        private readonly IRacuniCommonDependecies _c;
        private readonly ILogger _logger;

        public RacuniElektraIzvrsenjeUslugeController(IRacuniElektraIzvrsenjeUslugeTempCreateService RacuniElektraIzvrsenjeUslugeTempCreateService, IRacuniElektraIzvrsenjeUslugeService RacuniElektraIzvrsenjeUslugeService,
            IRacuniCommonDependecies c, ILogger logger)
        {
            _c = c;
            _RacuniElektraIzvrsenjeUslugeTempCreateService = RacuniElektraIzvrsenjeUslugeTempCreateService;
            _RacuniElektraIzvrsenjeUslugeService = RacuniElektraIzvrsenjeUslugeService;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: RacuniElektraIzvrsenjeUsluge/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge = await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.IncludeAll((int)id);

            return racunElektraIzvrsenjeUsluge == null ? NotFound() : View(racunElektraIzvrsenjeUsluge);
        }

        // GET: RacuniElektraIzvrsenjeUsluge/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj");
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraKupac.GetAll(), "Id", "Id");
            return View();
        }

        // POST: RacuniElektraIzvrsenjeUsluge/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task Create([Bind("Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa, Napomena")] RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge)
        {
            if (ModelState.IsValid)
            {
                racunElektraIzvrsenjeUsluge.VrijemeUnosa = DateTime.Now;
                _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Add(racunElektraIzvrsenjeUsluge);
                _ = await _c.UnitOfWork.Complete();
                RedirectToAction(nameof(Index));
            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj", racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraKupac.GetAll(), "Id", "Id", racunElektraIzvrsenjeUsluge.ElektraKupacId);
        }

        // GET: RacuniElektraIzvrsenjeUsluge/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge = await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.IncludeAll((int)id);

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

                _c.UnitOfWork.RacuniElektraIzvrsenjeUslugeEdit.Add(racunElektraIzvrsenjeUslugeEdit);
                _ = await _c.UnitOfWork.Complete();
            }
            catch (Exception)
            {

            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj", racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraKupac.GetAll(), "Id", "Id", racunElektraIzvrsenjeUsluge.ElektraKupacId);
            return View(racunElektraIzvrsenjeUsluge);
        }

        // POST: RacuniElektraIzvrsenjeUsluge/Edit/5
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
                    _ = await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Update(racunElektraIzvrsenjeUsluge);

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
                finally
                {
                    _c.UnitOfWork.RacuniElektraIzvrsenjeUslugeEdit.RemoveRange(await _c.UnitOfWork.RacuniElektraIzvrsenjeUslugeEdit.Find(e => e.EditingByUserId.Equals(_c.Service.GetUid(User))));
                    _ = await _c.UnitOfWork.Complete();
                }

                return racunElektraIzvrsenjeUsluge.IsItTemp == true ? RedirectToAction("Create") : RedirectToAction(nameof(Index));
            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj", racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraKupac.GetAll(), "Id", "Id", racunElektraIzvrsenjeUsluge.ElektraKupacId);
            return View(racunElektraIzvrsenjeUsluge);
        }

        // GET: RacuniElektraIzvrsenjeUsluge/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge = await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.IncludeAll((int)id);

            return racunElektraIzvrsenjeUsluge == null ? NotFound() : View(racunElektraIzvrsenjeUsluge);
        }

        // POST: RacuniElektraIzvrsenjeUsluge/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge = await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Get(id);
            _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Remove(racunElektraIzvrsenjeUsluge);
            _ = await _c.UnitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }

        private bool RacunElektraIzvrsenjeUslugeExists(int id)
        {
            return _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Any(e => e.Id == id);
        }

        // validation
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {
            RacunElektraIzvrsenjeUslugeEdit racunElektraUslugeEdit = await _c.UnitOfWork.RacuniElektraIzvrsenjeUslugeEdit.GetLastRacunElektraServiceEdit(_c.Service.GetUid(User));

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

            RacunElektraIzvrsenjeUsluge db = await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.FindExact(x => x.BrojRacuna.Equals(brojRacuna));
            return (db != null && db.IsItTemp != true && !racunElektraUslugeEdit.RacunElektraIzvrsenjeUsluge.BrojRacuna.Equals(brojRacuna)) ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetDopisiDataForFilter(int predmetId)
        {
            return Json(await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.GetDopisiForPayedRacuniElektraIzvrsenjeUsluge(predmetId));
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
            return await _c.RacuniInlineEditorService.UpdateDbForInline<RacunElektraIzvrsenjeUsluge>(await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.FindExact(e => e.Id == int.Parse(id)), updatedColumn, x);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveToDB(string _dopisId)
        {
            if ((await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            return await _RacuniElektraIzvrsenjeUslugeTempCreateService.CheckTempTableForRacuniWithousElectraCustomer(_c.Service.GetUid(User)) != 0
                ? (new(new { success = false, Message = "U tablici postoje računi bez kupca!" }))
                : await _c.RacuniTempEditorService.SaveToDb<RacunElektraIzvrsenjeUsluge>(await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge
                .Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))), _dopisId);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveRow(string racunId)
        {
            _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Remove(await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.FindExact(e => e.Id == int.Parse(racunId)));
            _ = await _c.UnitOfWork.Complete();

            IEnumerable<RacunElektraIzvrsenjeUsluge> list = await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User)));
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
            if ((await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.RemoveRange(await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))));
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

            return await _RacuniElektraIzvrsenjeUslugeTempCreateService.AddNewTemp(brojRacuna, iznos, date, datumIzvrsenja, usluga, dopisId, _c.Service.GetUid(User));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetPredmetiDataForFilter()
        {
            return Json(_c.UnitOfWork.Predmet.GetPredmetfForAllPayedRacuni(await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.GetRacuniElektraIzvrsenjeUslugeWithDopisiAndPredmeti()));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList(bool isFilteredForIndex, string klasa, string urbroj)
        {

            IEnumerable<RacunElektraIzvrsenjeUsluge> list = isFilteredForIndex
                ? await _RacuniElektraIzvrsenjeUslugeService.GetList(_RacuniElektraIzvrsenjeUslugeService.ParsePredmet(klasa), _RacuniElektraIzvrsenjeUslugeService.ParseDopis(urbroj))
                : await _RacuniElektraIzvrsenjeUslugeService.GetCreateRacuni(_c.Service.GetUid(User));

            return new DatatablesService<RacunElektraIzvrsenjeUsluge>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacunElektraIzvrsenjeUslugeForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetDopisiDataForCreate(int predmetId)
        {
            return Json(await _c.UnitOfWork.Dopis.GetOnlyEmptyDopisi(predmetId));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RefreshCustomers()
        {
            return await _RacuniElektraIzvrsenjeUslugeTempCreateService.RefreshCustomers(_c.Service.GetUid(User));
        }
    }
}
