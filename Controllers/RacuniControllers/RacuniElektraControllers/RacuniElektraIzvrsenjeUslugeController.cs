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

namespace aes.Controllers.RacuniControllers.RacuniElektraControllers
{
    public class RacuniElektraIzvrsenjeUslugeController : Controller, IRacuniController
    {
        private readonly IRacuniElektraIzvrsenjeUslugeTempCreateService _racuniElektraIzvrsenjeUslugeTempCreateService;
        private readonly IRacuniElektraIzvrsenjeUslugeService _racuniElektraIzvrsenjeUslugeService;
        private readonly IRacuniCommonDependecies _c;
        private readonly ILogger _logger;

        public RacuniElektraIzvrsenjeUslugeController(
            IRacuniElektraIzvrsenjeUslugeTempCreateService racuniElektraIzvrsenjeUslugeTempCreateService,
            IRacuniElektraIzvrsenjeUslugeService racuniElektraIzvrsenjeUslugeService,
            IRacuniCommonDependecies c, ILogger logger)
        {
            _c = c;
            _racuniElektraIzvrsenjeUslugeTempCreateService = racuniElektraIzvrsenjeUslugeTempCreateService;
            _racuniElektraIzvrsenjeUslugeService = racuniElektraIzvrsenjeUslugeService;
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

            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge =
                await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.IncludeAll((int)id);

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
        public async Task Create(
            [Bind(
                "Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa, Napomena")]
            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge)
        {
            if (ModelState.IsValid)
            {
                racunElektraIzvrsenjeUsluge.VrijemeUnosa = DateTime.Now;
                await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Add(racunElektraIzvrsenjeUsluge);
                _ = await _c.UnitOfWork.Complete();
                RedirectToAction(nameof(Index));
            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj",
                racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraKupac.GetAll(), "Id", "Id",
                racunElektraIzvrsenjeUsluge.ElektraKupacId);
        }

        // GET: RacuniElektraIzvrsenjeUsluge/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge =
                await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.IncludeAll((int)id);

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

                await _c.UnitOfWork.RacuniElektraIzvrsenjeUslugeEdit.Add(racunElektraIzvrsenjeUslugeEdit);
                _ = await _c.UnitOfWork.Complete();
            }
            catch (Exception)
            {
                // ignored
            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj",
                racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraKupac.GetAll(), "Id", "Id",
                racunElektraIzvrsenjeUsluge.ElektraKupacId);
            return View(racunElektraIzvrsenjeUsluge);
        }

        // POST: RacuniElektraIzvrsenjeUsluge/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id,BrojRacuna,ElektraKupacId,DatumIzdavanja,DatumIzvrsenja,Usluga,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa,Napomena, IsItTemp, CreatedByUserId")]
            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge)
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
                    if (!await RacunElektraIzvrsenjeUslugeExists(racunElektraIzvrsenjeUsluge.Id))
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
                    _c.UnitOfWork.RacuniElektraIzvrsenjeUslugeEdit.RemoveRange(
                        await _c.UnitOfWork.RacuniElektraIzvrsenjeUslugeEdit.Find(e =>
                            e.EditingByUserId.Equals(_c.Service.GetUid(User))));
                    _ = await _c.UnitOfWork.Complete();
                }

                return racunElektraIzvrsenjeUsluge.IsItTemp == true
                    ? RedirectToAction("Create")
                    : RedirectToAction(nameof(Index));
            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj",
                racunElektraIzvrsenjeUsluge.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraKupac.GetAll(), "Id", "Id",
                racunElektraIzvrsenjeUsluge.ElektraKupacId);
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

            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge =
                await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.IncludeAll((int)id);

            return racunElektraIzvrsenjeUsluge == null ? NotFound() : View(racunElektraIzvrsenjeUsluge);
        }

        // POST: RacuniElektraIzvrsenjeUsluge/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunElektraIzvrsenjeUsluge racunElektraIzvrsenjeUsluge =
                await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Get(id);
            _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Remove(racunElektraIzvrsenjeUsluge);
            _ = await _c.UnitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RacunElektraIzvrsenjeUslugeExists(int id)
        {
            return await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Any(e => e.Id == id);
        }

        // validation
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {
            RacunElektraIzvrsenjeUslugeEdit racunElektraUslugeEdit =
                await _c.UnitOfWork.RacuniElektraIzvrsenjeUslugeEdit.GetLastRacunElektraServiceEdit(
                    _c.Service.GetUid(User));

            if (brojRacuna.Length is not 19
                || brojRacuna[10] is not '-'
                || brojRacuna[17] is not '-'
                || !int.TryParse(brojRacuna.AsSpan(11, 6), out _)
                || !int.TryParse(brojRacuna.AsSpan(18, 1), out _))
            {
                return Json($"Broj računa nije ispravan");
            }

            if (racunElektraUslugeEdit != null && brojRacuna.Length >= 10 && !brojRacuna[..10]
                    .Equals(racunElektraUslugeEdit.RacunElektraIzvrsenjeUsluge.BrojRacuna[..10]))
            {
                return Json($"Ugovorni račun unutar broja računa ne smije se razlikovati");
            }

            RacunElektraIzvrsenjeUsluge db =
                await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.FindExact(x => x.BrojRacuna.Equals(brojRacuna));
            return racunElektraUslugeEdit != null && (db != null && db.IsItTemp != true &&
                                                      !racunElektraUslugeEdit.RacunElektraIzvrsenjeUsluge.BrojRacuna
                                                          .Equals(brojRacuna))
                ? Json($"Račun {brojRacuna} već postoji.")
                : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetDopisiDataForFilter(int predmetId)
        {
            return Json(
                await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.GetDopisiForPayedRacuniElektraIzvrsenjeUsluge(
                    predmetId));
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
            return await _c.RacuniInlineEditorService.UpdateDbForInline<RacunElektraIzvrsenjeUsluge>(
                await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.FindExact(e => e.Id == int.Parse(id)), updatedColumn,
                x);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveToDB(string dopisId)
        {
            if ((await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            return await _racuniElektraIzvrsenjeUslugeTempCreateService.CheckTempTableForRacuniWithousElectraCustomer(
                _c.Service.GetUid(User)) != 0
                ? (new(new { success = false, Message = "U tablici postoje računi bez kupca!" }))
                : await _c.RacuniTempEditorService.SaveToDb<RacunElektraIzvrsenjeUsluge>(await _c.UnitOfWork
                    .RacuniElektraIzvrsenjeUsluge
                    .Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))), dopisId);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveRow(string racunId)
        {
            _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Remove(
                await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.FindExact(e => e.Id == int.Parse(racunId)));
            _ = await _c.UnitOfWork.Complete();

            IEnumerable<RacunElektraIzvrsenjeUsluge> list = await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Find(e =>
                e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User)));
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

            _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.RemoveRange(
                await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.Find(e =>
                    e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))));
            return await _c.Service.TrySave(true);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string date, string datumIzvrsenja,
            string usluga, string dopisId)
        {
            var declaringType = System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType;
            if (declaringType != null)
            {
                if (User.Identity != null)
                {
                    string loggerTemplate =
                        declaringType.FullName + ", " + "User: " + User.Identity.Name + ", " + "msg: ";

                    if (brojRacuna is null)
                    {
                        string message = "brojRacuna ne može biti prazan";

                        _logger.Information(loggerTemplate + message);

                        return new(new
                        {
                            success = false,
                            message
                        });
                    }
                }
            }

            return await _racuniElektraIzvrsenjeUslugeTempCreateService.AddNewTemp(brojRacuna, iznos, date,
                datumIzvrsenja, usluga, dopisId, _c.Service.GetUid(User));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetPredmetiDataForFilter()
        {
            return Json(_c.UnitOfWork.Predmet.GetPredmetForAllPaidRacuni(await _c.UnitOfWork
                .RacuniElektraIzvrsenjeUsluge.GetRacuniElektraIzvrsenjeUslugeWithDopisiAndPredmeti()));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList(bool isFilteredForIndex, string klasa, string urbroj)
        {
            IEnumerable<RacunElektraIzvrsenjeUsluge> list = isFilteredForIndex
                ? await _racuniElektraIzvrsenjeUslugeService.GetList(
                    _racuniElektraIzvrsenjeUslugeService.ParsePredmet(klasa),
                    _racuniElektraIzvrsenjeUslugeService.ParseDopis(urbroj))
                : await _racuniElektraIzvrsenjeUslugeService.GetCreateRacuni(_c.Service.GetUid(User));

            return new DatatablesService<RacunElektraIzvrsenjeUsluge>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacunElektraIzvrsenjeUslugeForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetDopisiDataForCreate(int predmetId)
        {
            return Json(await _c.UnitOfWork.Dopis.GetOnlyEmptyDopisiAsync(predmetId));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RefreshCustomers()
        {
            return await _racuniElektraIzvrsenjeUslugeTempCreateService.RefreshCustomers(_c.Service.GetUid(User));
        }
    }
}