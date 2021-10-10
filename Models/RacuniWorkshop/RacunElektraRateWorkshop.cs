﻿using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace aes.Models
{
    public class RacunElektraRateWorkshop : IRacunElektraRateWorkshop
    {
        private static readonly IRacunWorkshop racunWorkshop = new RacunWorkshop();
        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context)
        {
            if (!racunWorkshop.Validate(brojRacuna, iznos, date, dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja))
            {
                return new(new { success = false, Message = msg });
            }

            List<RacunElektraRate> RacunElektraRateList = _context.RacunElektraRate.Where(e => e.IsItTemp == true && e.CreatedByUserId.Equals(userId)).ToList();
            RacunElektraRate re = new()
            {
                BrojRacuna = brojRacuna,
                Iznos = _iznos,
                DatumIzdavanja = datumIzdavanja,
                DopisId = _dopisId == 0 ? null : _dopisId,
                CreatedByUserId = userId,
                IsItTemp = true,
            };

            re.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(re.BrojRacuna.Substring(0, 10)));
            RacunElektraRateList.Add(re);

            int rbr = 1;
            foreach (RacunElektraRate e in RacunElektraRateList)
            {
                e.RedniBroj = rbr++;
            }

            _ = _context.RacunElektraRate.Add(re);

            return racunWorkshop.TrySave(_context);
        }

        public List<RacunElektraRate> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context)
        {
            IQueryable<RacunElektraRate> RacunElektraRateList = predmetIdAsInt is 0 && dopisIdAsInt is 0
                ? _context.RacunElektraRate.Where(e => e.IsItTemp == null)
                : dopisIdAsInt is 0
                    ? _context.RacunElektraRate.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt)
                    : _context.RacunElektraRate.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt && x.Dopis.Id == dopisIdAsInt);
            return RacunElektraRateList
                .Include(e => e.ElektraKupac)
                .Include(e => e.ElektraKupac.Ods)
                .Include(e => e.ElektraKupac.Ods.Stan)
                .Include(e => e.Dopis)
                .Include(e => e.Dopis.Predmet)
                .ToList();
        }

        public List<RacunElektraRate> GetListCreateList(string userId, ApplicationDbContext _context)
        {
            List<RacunElektraRate> RacunElektraRateList = _context.RacunElektraRate.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToList();

            int rbr = 1;
            foreach (RacunElektraRate e in RacunElektraRateList)
            {

                e.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(e.BrojRacuna.Substring(0, 10)));

                List<Racun> racunList = new();
                racunList.AddRange(_context.RacunElektraRate.Where(e => e.IsItTemp == null || false).ToList());
                e.Napomena = racunWorkshop.CheckIfExistsInPayed(e.BrojRacuna, racunList);
                racunList.Clear();

                racunList.AddRange(_context.RacunElektraRate.Where(e => e.IsItTemp == true && e.CreatedByUserId == userId).ToList());
                if (e.Napomena is null)
                {
                    e.Napomena = racunWorkshop.CheckIfExists(e.BrojRacuna, racunList);
                }

                racunList.Clear();

                if (e.ElektraKupac != null)
                {
                    e.ElektraKupac.Ods = _context.Ods.FirstOrDefault(o => o.Id == e.ElektraKupac.OdsId);
                    e.ElektraKupac.Ods.Stan = _context.Stan.FirstOrDefault(o => o.Id == e.ElektraKupac.Ods.StanId);
                }
                else
                {
                    e.Napomena = "kupac ne postoji";
                }
                e.RedniBroj = rbr++;
            }
            return RacunElektraRateList;
        }

        public List<RacunElektraRate> GetRacuniElektraRateForDatatables(IDatatablesParams Params, ApplicationDbContext _context, List<RacunElektraRate> CreateRacuniElektraRateList)
        {
            CreateRacuniElektraRateList = CreateRacuniElektraRateList
                .Where(
                    x => x.BrojRacuna.Contains(Params.SearchValue)
                    || x.ElektraKupac.UgovorniRacun.ToString().Contains(Params.SearchValue)
                    || x.Razdoblje.ToString("MM.yyyy").Contains(Params.SearchValue)
                    || x.Iznos.ToString().Contains(Params.SearchValue)
                    || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(Params.SearchValue))
                    || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(Params.SearchValue))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(Params.SearchValue.ToLower())))
                .ToDynamicList<RacunElektraRate>();
            return CreateRacuniElektraRateList;
        }
        public JsonResult GetList(bool isFiltered, string klasa, string urbroj, IDatatablesGenerator datatablesGenerator,
ApplicationDbContext _context, HttpRequest Request, IRacunElektraRateWorkshop racunElektraRateWorkshop, string Uid)
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<RacunElektraRate> racunElektraRateList;
            if (isFiltered)
            {
                int predmetIdAsInt = klasa is null ? 0 : int.Parse(klasa);
                int dopisIdAsInt = urbroj is null ? 0 : int.Parse(urbroj);
                racunElektraRateList = racunElektraRateWorkshop.GetList(predmetIdAsInt, dopisIdAsInt, _context);
            }
            else
            {
                racunElektraRateList = racunElektraRateWorkshop.GetListCreateList(Uid, _context);
            }

            int totalRows = racunElektraRateList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                racunElektraRateList = racunElektraRateWorkshop.GetRacuniElektraRateForDatatables(Params, _context, racunElektraRateList);
            }
            int totalRowsAfterFiltering = racunElektraRateList.Count;
            return datatablesGenerator.SortingPaging(racunElektraRateList, Params, Request, totalRows, totalRowsAfterFiltering);
        }
    }
}
