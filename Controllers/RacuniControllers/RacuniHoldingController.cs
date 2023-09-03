using aes.CommonDependecies.ICommonDependencies;
using aes.Controllers.IControllers;
using aes.Models.Racuni.Holding;
using aes.Services;
using aes.Services.RacuniServices.RacuniHoldingService.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace aes.Controllers.RacuniControllers
{
    public class RacuniHoldingController : Controller, IRacuniController
    {

        private readonly IRacuniHoldingService _racunHoldingService;
        private readonly IRacuniHoldingTempCreateService _racuniHoldingTempCreateService;
        private readonly IRacuniHoldingUploadService _racuniHoldingUploadService;
        private readonly IRacuniCommonDependecies _c;
        private readonly ILogger _logger;


        public RacuniHoldingController(IRacuniHoldingService racunHoldingService, IRacuniHoldingTempCreateService racuniHoldingTempCreateService,
            IRacuniCommonDependecies c, IRacuniHoldingUploadService racuniHoldingUploadService, ILogger logger)
        {
            _c = c;
            _racunHoldingService = racunHoldingService;
            _racuniHoldingTempCreateService = racuniHoldingTempCreateService;
            _racuniHoldingUploadService = racuniHoldingUploadService;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: RacuniHolding/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RacunHolding racunHolding = await _c.UnitOfWork.RacuniHolding
                .IncludeAll((int)id);

            return racunHolding == null ? NotFound() : View(racunHolding);
        }

        // GET: RacuniHolding/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj");
            ViewData["StanId"] = new SelectList(await _c.UnitOfWork.Stan.GetAll(), "Id", "Adresa");
            return View();
        }

        // POST: RacuniHolding/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task Create([Bind("Id,BrojRacuna,StanId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunHolding racunHolding)
        {
            if (ModelState.IsValid)
            {
                racunHolding.VrijemeUnosa = DateTime.Now;
                await _c.UnitOfWork.RacuniHolding.Add(racunHolding);
                _ = _c.UnitOfWork.Complete(); 
                RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(await _c.UnitOfWork.Stan.GetAll(), "Id", "Adresa", racunHolding.StanId);
        }

        // GET: RacuniHolding/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunHolding racunHolding = await _c.UnitOfWork.RacuniHolding
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

                await _c.UnitOfWork.RacuniHoldingEdit.Add(racunHoldingEdit);
                _ = await _c.UnitOfWork.Complete();
            }
            catch (Exception)
            {
                // ignored
            }

            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(await _c.UnitOfWork.Stan.GetAll(), "Id", "Adresa", racunHolding.StanId);
            return View(racunHolding);
        }

        // POST: RacuniHolding/Edit/5
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
                    _ = await _c.UnitOfWork.RacuniHolding.Update(racunHolding);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await RacunHoldingExists(racunHolding.Id))
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
                    _c.UnitOfWork.RacuniHoldingEdit.RemoveRange(await _c.UnitOfWork.RacuniHoldingEdit.Find(e => e.EditingByUserId.Equals(_c.Service.GetUid(User))));
                    _ = await _c.UnitOfWork.Complete();
                }

                return racunHolding.IsItTemp == true ? RedirectToAction("Create") : RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(await _c.UnitOfWork.Stan.GetAll(), "Id", "Adresa", racunHolding.StanId);

            return View(racunHolding);
        }

        // GET: RacuniHolding/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunHolding racunHolding = await _c.UnitOfWork.RacuniHolding
                .IncludeAll((int)id);

            return racunHolding == null ? NotFound() : View(racunHolding);
        }

        // POST: RacuniHolding/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunHolding racunHolding = await _c.UnitOfWork.RacuniHolding.Get(id);
            _c.UnitOfWork.RacuniHolding.Remove(racunHolding);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RacunHoldingExists(int id)
        {
            return await _c.UnitOfWork.RacuniHolding.Any(e => e.Id == id);
        }

        // validation
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {
            RacunHoldingEdit racunHoldingEdit = await _c.UnitOfWork.RacuniHoldingEdit.GetLastRacunHoldingEdit(_c.Service.GetUid(User));

            if (brojRacuna.Length is not 20
                || brojRacuna[8] is not '-'
                || brojRacuna[18] is not '-'
                || !int.TryParse(brojRacuna.AsSpan(9, 9), out _)
                || !int.TryParse(brojRacuna.AsSpan(19, 1), out _))
            {
                return Json($"Broj računa nije ispravan");
            }

            if (racunHoldingEdit != null && brojRacuna.Length >= 8 && !brojRacuna[..8].Equals(racunHoldingEdit.RacunHolding.BrojRacuna[..8]))
            {
                return Json($"Šifra objekta unutar broja računa ne smije se razlikovati");
            }

            RacunHolding db = await _c.UnitOfWork.RacuniHolding.FindExact(x => x.BrojRacuna.Equals(brojRacuna));
            return racunHoldingEdit != null && (db != null && db.IsItTemp != true && !racunHoldingEdit.RacunHolding.BrojRacuna.Equals(brojRacuna)) ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Upload

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            return await _racuniHoldingUploadService.Upload(Request, _c.Service.GetUid(User));
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetDopisiDataForFilter(int predmetId)
        {
            return Json(await _c.UnitOfWork.RacuniHolding.GetDopisiForPayedRacuni(predmetId));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetDopisiDataForCreate(int predmetId)
        {
            return Json(await _c.UnitOfWork.Dopis.GetOnlyEmptyDopisiAsync(predmetId));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetPredmetiForCreate()
        {
            return Json(await _c.UnitOfWork.RacuniHolding.GetPredmetiForCreate());
        }

        [Authorize]
        [HttpPost]
        public async Task<string> GetCustomers()
        {
            return JsonConvert.SerializeObject(await _c.UnitOfWork.Stan.GetStanovi());
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> UpdateDbForInline(string id, string updatedColumn, string x)
        {
            return await _c.RacuniInlineEditorService.UpdateDbForInline<RacunHolding>(await _c.UnitOfWork.RacuniHolding.FindExact(e => e.Id == int.Parse(id)), updatedColumn, x);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveToDB(string dopisId)
        {
            return (await _c.UnitOfWork.RacuniHolding.TempList(_c.Service.GetUid(User))).Count() is 0
                ? (new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                }))
                : await _racuniHoldingTempCreateService.CheckTempTableForRacuniWithouCustomer(_c.Service.GetUid(User)) != 0
                ? (new(new { success = false, Message = "U tablici postoje računi bez kupca!" }))
                : await _c.RacuniTempEditorService.SaveToDb<RacunHolding>(await _c.UnitOfWork.RacuniHolding.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))), dopisId);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveRow(string racunId)
        {

            _c.UnitOfWork.RacuniHolding.Remove(await _c.UnitOfWork.RacuniHolding.FindExact(e => e.Id == int.Parse(racunId)));
            _ = await _c.UnitOfWork.Complete();

            IEnumerable<RacunHolding> racuni = await _c.UnitOfWork.RacuniHolding.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User)));
            int rbr = 1;
            foreach (RacunHolding e in racuni)
            {
                e.RedniBroj = rbr++;
            }
            return await _c.Service.TrySave(true);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveAllFromDb()
        {
            if ((await _c.UnitOfWork.RacuniHolding.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            _c.UnitOfWork.RacuniHolding.RemoveRange(await _c.UnitOfWork.RacuniHolding.Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))));
            return await _c.Service.TrySave(true);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string date)
        {
            var declaringType = System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType;
            if (declaringType != null)
            {
                if (User.Identity != null)
                {
                    string loggerTemplate = declaringType.FullName + ", " + "User: " + User.Identity.Name + ", " + "msg: ";

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

            return await _racuniHoldingTempCreateService.AddNewTemp(brojRacuna, iznos, date, _c.Service.GetUid(User));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetPredmetiDataForFilter()
        {
            return Json(_c.UnitOfWork.Predmet.GetPredmetForAllPaidRacuni(await _c.UnitOfWork.RacuniHolding.GetRacuniHoldingWithDopisiAndPredmeti()));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList(bool isFilteredForIndex, string klasa, string urbroj)
        {
            IEnumerable<RacunHolding> list = isFilteredForIndex
                ? await _racunHoldingService.GetList(_racunHoldingService.ParsePredmet(klasa), _racunHoldingService.ParseDopis(urbroj))
                : await _racunHoldingService.GetCreateRacuni(_c.Service.GetUid(User));

            return new DatatablesService<RacunHolding>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniHoldingForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RefreshCustomers()
        {
            return await _racuniHoldingTempCreateService.RefreshCustomers(_c.Service.GetUid(User));
        }
    }
}
