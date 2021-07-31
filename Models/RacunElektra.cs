﻿using aes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace aes.Models
{
    public class RacunElektra : Racun
    {
        public ElektraKupac ElektraKupac { get; set; }
        public int ElektraKupacId { get; set; }

        public static JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context)
        {

            if (!Validate(brojRacuna, iznos, date, dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja))
                return new(new { success = false, Message = msg });

            List<RacunElektra> racunElektraList = _context.RacunElektra.Where(e => e.IsItTemp == true && e.CreatedByUserId.Equals(userId)).ToList();
            RacunElektra re = new()
            {
                BrojRacuna = brojRacuna,
                Iznos = _iznos,
                DatumIzdavanja = datumIzdavanja,
                DopisId = _dopisId == 0 ? null : _dopisId,
                CreatedByUserId = userId,
                IsItTemp = true,
            };

            if (_context.ElektraKupac.Any(e => e.UgovorniRacun == long.Parse(re.BrojRacuna.Substring(0, 10))))
                re.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(re.BrojRacuna.Substring(0, 10)));
            else
                re.ElektraKupac = null;

            racunElektraList.Add(re);

            int rbr = 1;
            foreach (RacunElektra e in racunElektraList)
            {
                e.RedniBroj = rbr++;
            }

            _ = _context.RacunElektra.Add(re);

            return TrySave(_context);
        }

        public static List<RacunElektra> GetListCreateList(string userId, ApplicationDbContext _context)
        {
            List<RacunElektra> racunElektraList = _context.RacunElektra.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToList();

            int rbr = 1;
            foreach (RacunElektra e in racunElektraList)
            {

                e.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(e.BrojRacuna.Substring(0, 10)));

                List<Racun> racunList = new();
                racunList.AddRange(_context.RacunElektra.Where(e => e.IsItTemp == null || false).ToList());
                e.Napomena = CheckIfExistsInPayed(e.BrojRacuna, racunList);
                racunList.Clear();

                racunList.AddRange(_context.RacunElektra.Where(e => e.IsItTemp == true && e.CreatedByUserId == userId).ToList());
                if (e.Napomena is null)
                {
                    e.Napomena = CheckIfExists(e.BrojRacuna, racunList);
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
            return racunElektraList;
        }

        public static List<RacunElektra> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context)
        {
            List<RacunElektra> racunElektraList = new();

            if (predmetIdAsInt == 0 && dopisIdAsInt == 0)
            {
                racunElektraList = _context.RacunElektra.ToList();
            }

            if (predmetIdAsInt != 0)
            {
                racunElektraList = dopisIdAsInt == 0
                    ? _context.RacunElektra.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt).ToList()
                    : _context.RacunElektra.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt && x.Dopis.Id == dopisIdAsInt).ToList();
            }


            foreach (RacunElektra racunElektra in racunElektraList)
            {
                racunElektra.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.Id == racunElektra.ElektraKupacId);
                racunElektra.Dopis = _context.Dopis.FirstOrDefault(o => o.Id == racunElektra.DopisId);

                if (racunElektra.Dopis != null)
                {
                    racunElektra.Dopis.Predmet = _context.Predmet.FirstOrDefault(o => o.Id == racunElektra.Dopis.PredmetId);
                }
            }
            return racunElektraList;
        }
    }
}