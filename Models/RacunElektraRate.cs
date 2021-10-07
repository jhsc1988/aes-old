﻿using aes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace aes.Models
{
    public class RacunElektraRate : Racun
    {
        public ElektraKupac ElektraKupac { get; set; }
        [Required]
        public int? ElektraKupacId { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Razdoblje")]
        [DataType(DataType.Date)]
        public DateTime Razdoblje { get; set; }
        private static List<RacunElektraRate> RacunElektraRateList { get; set; }
        private static readonly IRacunWorkshop racunWorkshop = new RacunWorkshop();

        public static JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context)
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

            // todo: trysave
            //return TrySave(_context);
            return null;
        }

        public static List<RacunElektraRate> GetListCreateList(string userId, ApplicationDbContext _context)
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

        public static List<RacunElektraRate> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context)
        {
            List<RacunElektraRate> RacunElektraRateList = new();

            if (predmetIdAsInt is 0 && dopisIdAsInt is 0)
            {
                RacunElektraRateList = _context.RacunElektraRate.Where(e => e.IsItTemp == null).ToList();
            }

            RacunElektraRateList = _context.RacunElektraRate
                .Include(e => e.ElektraKupac)
                .Include(e => e.ElektraKupac.Ods)
                .Include(e => e.Dopis)
                .Include(e => e.Dopis.Predmet)
                .ToList();

            if (predmetIdAsInt is not 0)
            {
                RacunElektraRateList = dopisIdAsInt is 0
                    ? _context.RacunElektraRate.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt).ToList()
                    : _context.RacunElektraRate.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt && x.Dopis.Id == dopisIdAsInt).ToList();
            }

            return RacunElektraRateList;
        }

        public static List<RacunElektraRate> GetRacuniElektraRateForDatatables(DatatablesParams Params)
        {
            RacunElektraRateList = RacunElektraRateList.
                Where(
                x => x.BrojRacuna.Contains(Params.SearchValue)
                || x.ElektraKupac.UgovorniRacun.ToString().Contains(Params.SearchValue)
                || x.Razdoblje.ToString("MM.yyyy").Contains(Params.SearchValue)
                || x.Iznos.ToString().Contains(Params.SearchValue)
                || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(Params.SearchValue))
                || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(Params.SearchValue))
                || (x.Napomena != null && x.Napomena.ToLower().Contains(Params.SearchValue.ToLower()))).ToDynamicList<RacunElektraRate>();

            RacunElektraRateList = RacunElektraRateList.AsQueryable().OrderBy(Params.SortColumnName + " " + Params.SortDirection).ToList(); // sorting
            RacunElektraRateList = RacunElektraRateList.Skip(Params.Start).Take(Params.Length).ToList(); // paging
            return RacunElektraRateList;
        }
    }
}
