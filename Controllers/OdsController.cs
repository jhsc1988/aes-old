﻿using aes.CommonDependecies;
using aes.Controllers.IControllers;
using aes.Models;
using aes.Services;
using aes.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Controllers
{
    public class OdsController : Controller, IOdsController
    {
        private readonly ICommonDependencies _c;
        private readonly IOdsService _odsService;
        public OdsController(IOdsService odsService, ICommonDependencies c)
        {
            _odsService = odsService;
            _c = c;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Ods/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ods ods = await _c.UnitOfWork.Ods.IncludeAppartment(await _c.UnitOfWork.Ods.Get((int)id));
            return ods == null ? NotFound() : View(ods);
        }

        // GET: Ods/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,StanId,Omm,Napomena,VrijemeUnosa")] Ods ods)
        {
            if (ModelState.IsValid)
            {
                ods.VrijemeUnosa = DateTime.Now;
                _c.UnitOfWork.Ods.Add(ods);
                _ = await _c.UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(ods);
        }

        // GET: Ods/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ods ods = await _c.UnitOfWork.Ods.IncludeAppartment(await _c.UnitOfWork.Ods.Get((int)id));

            if (ods == null)
            {
                return NotFound();
            }
            ViewData["StanId"] = new SelectList(await _c.UnitOfWork.Apartment.GetAll(), "Id", "Adresa", ods.StanId);
            return View(ods);
        }

        // POST: Ods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StanId,Omm,Napomena,VrijemeUnosa")] Ods ods)
        {
            if (id != ods.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = await _c.UnitOfWork.Ods.Update(ods);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OdsExists(ods.Id))
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
            ViewData["StanId"] = new SelectList(await _c.UnitOfWork.Apartment.GetAll(), "Id", "Adresa", ods.StanId);
            return View(ods);
        }

        // GET: Ods/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ods ods = await _c.UnitOfWork.Ods.IncludeAppartment(await _c.UnitOfWork.Ods.Get((int)id));
            return ods == null ? NotFound() : View(ods);
        }

        // POST: Ods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Ods ods = await _c.UnitOfWork.Ods.Get(id);
            _c.UnitOfWork.Ods.Remove(ods);
            _ = await _c.UnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool OdsExists(int id)
        {
            return _c.UnitOfWork.Ods.Any(e => e.Id == id);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // validation

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> OmmValidation(int omm)
        {
            if (omm is < 10000000 or > 99999999)
            {
                return Json($"Broj obračunskog mjernog mjesta nije ispravan");
            }

            Ods db = await _c.UnitOfWork.Ods.FindExact(x => x.Omm == omm);
            return db != null ? Json($"Obračunsko mjerno mjesto {omm} već postoji.") : Json(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetStanData(string sid)
        {
            return await _odsService.GetStanData(sid);
        }

        [Authorize]
        [HttpPost]
        // todo: GetStanDataForKupci - preimenovati - stan data for omm
        public async Task<JsonResult> GetStanDataForKupci(string OdsId)
        {
            return await _odsService.GetStanDataForKupci(OdsId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetList()
        {
            IEnumerable<Ods> list = await _c.UnitOfWork.Ods.GetAllOds();

            return await new DatatablesService<Ods>().GetData(Request, list,
                _c.DatatablesGenerator, _c.DatatablesSearch.GetStanoviOdsForDatatables);
        }
    }
}
