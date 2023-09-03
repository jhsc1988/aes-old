using System.Diagnostics;
using System.Globalization;
using aes.CommonDependecies.ICommonDependencies;
using aes.Controllers.IControllers;
using aes.Data;
using aes.Models.HEP;
using aes.Models.Racuni.Elektra;
using aes.Services;
using aes.Services.RacuniServices.Elektra.RacuniElektra.Is;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace aes.Controllers.RacuniControllers.RacuniElektraControllers
{
    public class RacuniElektraController : Controller, IRacuniController
    {
        private readonly IRacuniElektraTempCreateService _racuniElektraTempCreateService;
        private readonly IRacuniElektraService _racuniElektraService;
        private readonly IRacuniElektraUploadService _racuniElektraUploadService;
        private readonly IRacuniCommonDependecies _c;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public RacuniElektraController(IRacuniElektraTempCreateService racuniElektraTempCreateService,
            IRacuniElektraService racuniElektraIRateWorkshop, IRacuniCommonDependecies c,
            IRacuniElektraUploadService racuniElektraUploadService, ILogger logger, ApplicationDbContext context)
        {
            _c = c;
            _racuniElektraTempCreateService = racuniElektraTempCreateService;
            _racuniElektraService = racuniElektraIRateWorkshop;
            _racuniElektraUploadService = racuniElektraUploadService;
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: RacuniElektra/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["RacunElektraId"] = new SelectList(_context.RacunElektra, "Id", "BrojRacuna");
            ViewData["TarifnaStavkaId"] = new SelectList(_context.TarifnaStavka, "Id", "Naziv");

            RacunElektra racunElektra = await _c.UnitOfWork.RacuniElektra.IncludeAll((int)id);

            ObracunPotrosnje lastObracunForRacun = await _c.UnitOfWork.ObracunPotrosnje.GetLastForRacunId((int)id);

            Debug.Assert(racunElektra != null, nameof(racunElektra) + " != null");
            ObracunPotrosnje obracunPotrosnje = new()
            {
                RacunElektraId = racunElektra.Id,
                BrojBrojila = 0,
                DatumOd = DateTime.Now,
                DatumDo = DateTime.Now,
                StanjeOd = 0,
                StanjeDo = 0,
                TarifnaStavka = new TarifnaStavka(),
            };

            if (lastObracunForRacun is not null)
            {
                obracunPotrosnje = lastObracunForRacun;
            }

            else
            {
                IEnumerable<ObracunPotrosnje> obracuniPotrosnjeForUgovorniRacun =
                    await _c.UnitOfWork.ObracunPotrosnje.GetObracunForUgovorniRacun(racunElektra.ElektraKupac
                        .UgovorniRacun);

                if (obracuniPotrosnjeForUgovorniRacun.Any())
                {
                    obracunPotrosnje =
                        (await _c.UnitOfWork.ObracunPotrosnje.GetObracunForUgovorniRacun(racunElektra.ElektraKupac
                            .UgovorniRacun)).ToList()[0];
                    obracunPotrosnje.Id = 0;
                    obracunPotrosnje.RacunElektraId = racunElektra.Id;
                    obracunPotrosnje.DatumOd = obracunPotrosnje.DatumDo.AddDays(1);
                    obracunPotrosnje.DatumDo = obracunPotrosnje.DatumOd.AddMonths(1);
                    obracunPotrosnje.StanjeOd = obracunPotrosnje.StanjeDo;
                    obracunPotrosnje.StanjeDo = obracunPotrosnje.StanjeOd;
                }
            }

            Tuple<RacunElektra, ObracunPotrosnje> tupleModel = new(racunElektra, obracunPotrosnje);
            return View(tupleModel);
        }

        // GET: RacuniElektra/Create
        [Authorize]
        public IActionResult CreateAsync()
        {
            return View(new RacunElektra());
        }

        // POST: RacuniElektra/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(
            [Bind("BrojRacuna,DatumIzdavanja,Iznos")]
            RacunElektra racunElektra)
        {
            if (ModelState.IsValid)
            {
                _ = await _racuniElektraTempCreateService.AddNewTemp(racunElektra.BrojRacuna,
                    racunElektra.Iznos.ToString(CultureInfo.CurrentCulture), racunElektra.DatumIzdavanja?.ToString(),
                    _c.Service.GetUid(User));
            }

            ModelState.Clear();

            return View();
        }

        // GET: RacuniElektra/Edit/5

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektra racunElektra = await _c.UnitOfWork.RacuniElektra.IncludeAll((int)id);

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

                await _c.UnitOfWork.RacuniElektraEdit.Add(racunElektraEdit);
                _ = await _c.UnitOfWork.Complete();
            }
            catch (Exception)
            {
                // ignored
            }

            ViewData["DopisId"] =
                new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj", racunElektra.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraKupac.GetAll(), "Id", "Id",
                racunElektra.ElektraKupacId);

            return View(racunElektra);
        }

        // POST: RacuniElektra/Edit/5
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
                    _ = await _c.UnitOfWork.RacuniElektra.Update(racunElektra);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!await RacunElektraExists(racunElektra.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Error(ex, "An error occurred while updating the Stan");
                        throw;
                    }
                }
                finally
                {
                    _c.UnitOfWork.RacuniElektraEdit.RemoveRange(
                        await _c.UnitOfWork.RacuniElektraEdit.Find(e =>
                            e.EditingByUserId.Equals(_c.Service.GetUid(User))));
                    _ = await _c.UnitOfWork.Complete();
                }

                return racunElektra.IsItTemp == true ? RedirectToAction("Create") : RedirectToAction(nameof(Index));
            }

            ViewData["DopisId"] =
                new SelectList(await _c.UnitOfWork.Dopis.GetAll(), "Id", "Urbroj", racunElektra.DopisId);
            ViewData["ElektraKupacId"] = new SelectList(await _c.UnitOfWork.ElektraKupac.GetAll(), "Id", "Id",
                racunElektra.ElektraKupacId);

            return View(racunElektra);
        }

        // GET: RacuniElektra/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunElektra racunElektra = await _c.UnitOfWork.RacuniElektra.IncludeAll((int)id);

            return racunElektra == null ? NotFound() : View(racunElektra);
        }

        // POST: RacuniElektra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunElektra racunElektra = await _c.UnitOfWork.RacuniElektra.Get(id);
            _c.UnitOfWork.RacuniElektra.Remove(racunElektra);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RacunElektraExists(int id)
        {
            return await _c.UnitOfWork.RacuniElektra.Any(e => e.Id == id);
        }

        [Authorize]
        [HttpGet]
        public async Task<JsonResult> BrojRacunaValidation(string brojRacuna)
        {
            RacunElektraEdit racunElektraEdit =
                await _c.UnitOfWork.RacuniElektraEdit.GetLastRacunElektraEdit(_c.Service.GetUid(User));

            if (brojRacuna.Length is not 19
                || brojRacuna[10] is not '-'
                || brojRacuna[17] is not '-'
                || !int.TryParse(brojRacuna.AsSpan(11, 6), out _)
                || !int.TryParse(brojRacuna.AsSpan(18, 1), out _))
            {
                return Json($"Broj računa nije ispravan");
            }

            if (racunElektraEdit != null && brojRacuna.Length >= 10 &&
                !brojRacuna[..10].Equals(racunElektraEdit.RacunElektra.BrojRacuna[..10]))
            {
                return Json($"Ugovorni račun unutar broja računa ne smije se razlikovati");
            }

            RacunElektra db = await _c.UnitOfWork.RacuniElektra.FindExact(x => x.BrojRacuna.Equals(brojRacuna));
            return racunElektraEdit != null && (db != null && db.IsItTemp != true &&
                                                !racunElektraEdit.RacunElektra.BrojRacuna.Equals(brojRacuna))
                ? Json($"Račun {brojRacuna} već postoji.")
                : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Upload

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            return await _racuniElektraUploadService.Upload(Request, _c.Service.GetUid(User));
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetDopisiDataForFilter(int predmetId)
        {
            return Json(await _c.UnitOfWork.RacuniElektra.GetDopisiForPayedRacuniElektra(predmetId));
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
            return await _c.RacuniInlineEditorService.UpdateDbForInline<RacunElektra>(
                await _c.UnitOfWork.RacuniElektra.FindExact(e => e.Id == int.Parse(id)), updatedColumn, x);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveToDB(string dopisId)
        {
            if ((await _c.UnitOfWork.RacuniElektra.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            return await _racuniElektraTempCreateService.CheckTempTableForRacuniWithousElektraKupac(
                _c.Service.GetUid(User)) != 0
                ? (new(new { success = false, Message = "U tablici postoje računi bez kupca!" }))
                : await _c.RacuniTempEditorService.SaveToDb<RacunElektra>(await _c.UnitOfWork.RacuniElektra
                    .Find(e => e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))), dopisId);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveRow(string racunId)
        {
            _c.UnitOfWork.RacuniElektra.Remove(
                await _c.UnitOfWork.RacuniElektra.FindExact(e => e.Id == int.Parse(racunId)));
            _ = await _c.UnitOfWork.Complete();

            IEnumerable<RacunElektra> list = await _c.UnitOfWork.RacuniElektra.Find(e =>
                e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User)));
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
            if ((await _c.UnitOfWork.RacuniElektra.TempList(_c.Service.GetUid(User))).Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            _c.UnitOfWork.RacuniElektra.RemoveRange(await _c.UnitOfWork.RacuniElektra.Find(e =>
                e.IsItTemp == true && e.CreatedByUserId.Equals(_c.Service.GetUid(User))));
            return await _c.Service.TrySave(true);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string date)
        {
            var declaringType = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType;
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

            return await _racuniElektraTempCreateService.AddNewTemp(brojRacuna, iznos, date, _c.Service.GetUid(User));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetPredmetiDataForFilter()
        {
            return Json(
                _c.UnitOfWork.Predmet.GetPredmetForAllPaidRacuni(await _c.UnitOfWork.RacuniElektra
                    .GetRacuniElektraWithDopisiAndPredmeti()));
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetList(bool isFilteredForIndex, string klasa, string urbroj)
        {
            IEnumerable<RacunElektra> list = isFilteredForIndex
                ? await _racuniElektraService.GetList(_racuniElektraService.ParsePredmet(klasa),
                    _racuniElektraService.ParseDopis(urbroj))
                : await _racuniElektraService.GetCreateRacuni(_c.Service.GetUid(User));

            return new DatatablesService<RacunElektra>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetRacuniElektraForDatatables);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RefreshCustomers()
        {
            return await _racuniElektraTempCreateService.RefreshCustomers(_c.Service.GetUid(User));
        }

        [Authorize]
        public async Task<JsonResult> GetObracunPotrosnjeForRacun(int racunId)
        {
            IEnumerable<ObracunPotrosnje> list =
                await _c.UnitOfWork.ObracunPotrosnje.GetObracunPotrosnjeForRacun(racunId);

            return new DatatablesService<ObracunPotrosnje>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetObracunPotrosnjeDatatables);
        }

        [Authorize]
        public async Task CreateObracunPotrosnje(
            [Bind("Id,RacunElektraId,BrojBrojila,TarifnaStavkaId,DatumOd,DatumDo,StanjeOd,StanjeDo")]
            ObracunPotrosnje obracunPotrosnje)
        {
            if (ModelState.IsValid)
            {
                _ = _context.Add(obracunPotrosnje);
                _ = await _context.SaveChangesAsync();
                RedirectToAction("Details", new { id = obracunPotrosnje.RacunElektraId });
            }

            ViewData["RacunElektraId"] =
                new SelectList(_context.RacunElektra, "Id", "BrojRacuna", obracunPotrosnje.RacunElektraId);
            ViewData["TarifnaStavkaId"] =
                new SelectList(_context.TarifnaStavka, "Id", "Id", obracunPotrosnje.TarifnaStavkaId);
        }
    }
}