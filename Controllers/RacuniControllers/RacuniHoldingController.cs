using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class RacuniHoldingController : Controller, IRacunController
    {
        private readonly IDatatablesGenerator _datatablesGenerator;
        private readonly IPredmetWorkshop _predmetWorkshop;
        private readonly IRacunHoldingWorkshop _racunHoldingWorkshop;
        private readonly ApplicationDbContext _context;

        public RacuniHoldingController(ApplicationDbContext context, IDatatablesGenerator datatablesGenerator,
            IRacunHoldingWorkshop racunHoldingWorkshop, IPredmetWorkshop predmetWorkshop)
        {
            _context = context;
            _predmetWorkshop = predmetWorkshop;
            _racunHoldingWorkshop = racunHoldingWorkshop;
            _datatablesGenerator = datatablesGenerator;
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

            RacunHolding racunHolding = await _context.RacunHolding
                .Include(r => r.Dopis)
                .Include(r => r.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            return racunHolding == null ? NotFound() : View(racunHolding);
        }

        // GET: RacuniHolding/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj");
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa");
            return View();
        }

        // POST: RacuniHolding/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,BrojRacuna,StanId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunHolding racunHolding)
        {
            if (ModelState.IsValid)
            {
                _ = _context.Add(racunHolding);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", racunHolding.StanId);
            return View(racunHolding);
        }

        // GET: RacuniHolding/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RacunHolding racunHolding = await _context.RacunHolding.FindAsync(id);
            if (racunHolding == null)
            {
                return NotFound();
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", racunHolding.StanId);
            return View(racunHolding);
        }

        // POST: RacuniHolding/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BrojRacuna,StanId,DatumIzdavanja,Iznos,DopisId,RedniBroj,KlasaPlacanja,DatumPotvrde,VrijemeUnosa")] RacunHolding racunHolding)
        {
            if (id != racunHolding.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // error fix
                    // The instance of entity type 'RacunElektra' cannot be tracked because another
                    // instance with the same key value for {'Id'} is already being tracked.
                    RacunHolding rh = _context.RacunHolding.FirstOrDefault(e => e.Id == id);
                    _context.Entry(rh).State = EntityState.Detached;
                    _context.Entry(racunHolding).State = EntityState.Modified;

                    _ = _context.Update(racunHolding);
                    _ = await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["DopisId"] = new SelectList(_context.Dopis, "Id", "Urbroj", racunHolding.DopisId);
            ViewData["StanId"] = new SelectList(_context.Stan, "Id", "Adresa", racunHolding.StanId);
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

            RacunHolding racunHolding = await _context.RacunHolding
                .Include(r => r.Dopis)
                .Include(r => r.Stan)
                .FirstOrDefaultAsync(m => m.Id == id);
            return racunHolding == null ? NotFound() : View(racunHolding);
        }

        // POST: RacuniHolding/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            RacunHolding racunHolding = await _context.RacunHolding.FindAsync(id);
            _ = _context.RacunHolding.Remove(racunHolding);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacunHoldingExists(int id)
        {
            return _context.RacunHolding.Any(e => e.Id == id);
        }

        // validation
        [HttpGet]
        public async Task<IActionResult> BrojRacunaValidation(string brojRacuna)
        {
            if (brojRacuna.Length is < 20 or > 20)
            {
                return Json($"Broj računa nije ispravan");
            }

            RacunHolding db = await _context.RacunHolding.FirstOrDefaultAsync(x => x.BrojRacuna.Equals(brojRacuna));
            return db != null ? Json($"Račun {brojRacuna} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string GetUid() => _racunHoldingWorkshop.GetUid(User);
        public JsonResult GetDopisiDataForFilter(int predmetId) => Json(_context.Dopis.Where(element => element.PredmetId == predmetId).ToList());
        public JsonResult GetPredmetiCreate() => Json(_context.Predmet.ToList());
        public string GetKupci() => JsonConvert.SerializeObject(_context.Stan.ToList());
        public JsonResult UpdateDbForInline(string id, string updatedColumn, string x)
            => _racunHoldingWorkshop.UpdateDbForInline(id, updatedColumn, x, _context.RacunHolding, _context);
        public JsonResult SaveToDB(string _dopisId) => _racunHoldingWorkshop.SaveToDb(GetUid(), _dopisId, _context.RacunHolding, _context);
        public JsonResult RemoveRow(string racunId) => _racunHoldingWorkshop.RemoveRow(racunId, _context.RacunHolding, _context);
        public JsonResult RemoveAllFromDb() => _racunHoldingWorkshop.RemoveAllFromDbTemp(GetUid(), _context.RacunHolding, _context);
        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId)
            => new JsonResult(_racunHoldingWorkshop.AddNewTemp(brojRacuna, iznos, date, dopisId, GetUid(), _context));
        public JsonResult GetPredmetiDataForFilter() => Json(_predmetWorkshop.GetPredmetiDataForFilter(_context.RacunHolding, _context));
        public JsonResult GetList(bool isFIltered, string klasa, string urbroj)
            => _racunHoldingWorkshop.GetList(isFIltered, klasa, urbroj, _datatablesGenerator, _context, Request, GetUid());
    }
}
