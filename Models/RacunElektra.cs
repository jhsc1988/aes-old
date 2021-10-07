using aes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace aes.Models
{
    public class RacunElektra : Racun
    {
        public ElektraKupac ElektraKupac { get; set; }
        public int? ElektraKupacId { get; set; }
        public static List<RacunElektra> RacuniElektraList { get; set; }

        private static readonly IRacunWorkshop racunWorkshop = new RacunWorkshop();
        public static JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context)
        {

            if (!racunWorkshop.Validate(brojRacuna, iznos, date, dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja))
            {
                return new(new { success = false, Message = msg });
            }

            List<RacunElektra> racunElektraList = _context.RacunElektra.Where(e => e.IsItTemp == true && e.CreatedByUserId.Equals(userId)).ToList();
            RacunElektra re = new()
            {
                BrojRacuna = brojRacuna,
                Iznos = _iznos,
                DatumIzdavanja = datumIzdavanja,
                DopisId = _dopisId is 0 ? null : _dopisId,
                CreatedByUserId = userId,
                IsItTemp = true,
            };

            re.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(re.BrojRacuna.Substring(0, 10)));
            racunElektraList.Add(re);

            int rbr = 1;
            foreach (RacunElektra e in racunElektraList)
            {
                e.RedniBroj = rbr++;
            }

            _ = _context.RacunElektra.Add(re);

            return racunWorkshop.TrySave(_context);
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
                e.Napomena = racunWorkshop.CheckIfExistsInPayed(e.BrojRacuna, racunList);
                racunList.Clear();

                racunList.AddRange(_context.RacunElektra.Where(e => e.IsItTemp == true && e.CreatedByUserId == userId).ToList());
                if (e.Napomena is null)
                {
                    e.Napomena = racunWorkshop.CheckIfExists(e.BrojRacuna, racunList);
                }

                racunList.Clear();

                if (e.ElektraKupac is not null)
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

            if (predmetIdAsInt is 0 && dopisIdAsInt is 0)
            {
                racunElektraList = _context.RacunElektra.Where(e => e.IsItTemp == null).ToList();
            }

            racunElektraList = _context.RacunElektra
                .Include(e => e.ElektraKupac)
                .Include(e => e.ElektraKupac.Ods)
                .Include(e => e.Dopis)
                .Include(e => e.Dopis.Predmet)
                .ToList();

            if (predmetIdAsInt is not 0)
            {
                racunElektraList = dopisIdAsInt is 0
                    ? _context.RacunElektra.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt).ToList()
                    : _context.RacunElektra.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt && x.Dopis.Id == dopisIdAsInt).ToList();
            }

            return racunElektraList;
        }

        public static List<RacunElektra> GetRacuniElektraForDatatables(DatatablesParams Params)
        {
            RacuniElektraList = RacuniElektraList.Where(
                        x => x.BrojRacuna.Contains(Params.SearchValue)
                             || x.ElektraKupac.UgovorniRacun.ToString().Contains(Params.SearchValue)
                             || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(Params.SearchValue)
                             || x.Iznos.ToString().Contains(Params.SearchValue)
                             || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(Params.SearchValue))
                             || (x.DatumPotvrde != null &&
                                 x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(Params.SearchValue))
                             || (x.Napomena != null && x.Napomena.ToLower().Contains(Params.SearchValue.ToLower())))
                    .ToDynamicList<RacunElektra>();

            RacuniElektraList = RacuniElektraList.AsQueryable().OrderBy(Params.SortColumnName + " " + Params.SortDirection).ToList(); // sorting
            RacuniElektraList = RacuniElektraList.Skip(Params.Start).Take(Params.Length).ToList(); // paging
            return RacuniElektraList;
        }
    }
}