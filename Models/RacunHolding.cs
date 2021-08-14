using aes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace aes.Models
{
    public class RacunHolding : Racun
    {
        public Stan Stan { get; set; }
        [Required]
        public int StanId { get; set; }

        public static JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context)
        {

            if (!Validate(brojRacuna, iznos, date, dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja))
            {
                return new(new { success = false, Message = msg });
            }

            List<RacunHolding> racunHoldingList = _context.RacunHolding.Where(e => e.IsItTemp == true && e.CreatedByUserId.Equals(userId)).ToList();
            RacunHolding re = new()
            {
                BrojRacuna = brojRacuna,
                Iznos = _iznos,
                DatumIzdavanja = datumIzdavanja,
                DopisId = _dopisId is 0 ? null : _dopisId,
                CreatedByUserId = userId,
                IsItTemp = true,
            };

            re.Stan = _context.Stan.FirstOrDefault(o => o.SifraObjekta == long.Parse(re.BrojRacuna.Substring(0, 8)));
            racunHoldingList.Add(re);

            int rbr = 1;
            foreach (RacunHolding e in racunHoldingList)
            {
                e.RedniBroj = rbr++;
            }

            _ = _context.RacunHolding.Add(re);

            return TrySave(_context);
        }

        public static List<RacunHolding> GetListCreateList(string userId, ApplicationDbContext _context)
        {
            List<RacunHolding> racunHoldingList = _context.RacunHolding.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToList();

            int rbr = 1;
            foreach (RacunHolding e in racunHoldingList)
            {

                e.Stan = _context.Stan.FirstOrDefault(o => o.SifraObjekta == long.Parse(e.BrojRacuna.Substring(0, 8)));
                if (e.Stan == null)
                {
                    e.Napomena = "Šifra objekta ne postoji";
                }

                List<Racun> racunList = new();
                racunList.AddRange(_context.RacunHolding.Where(e => e.IsItTemp == null || false).ToList());
                e.Napomena = CheckIfExistsInPayed(e.BrojRacuna, racunList);
                racunList.Clear();

                racunList.AddRange(_context.RacunHolding.Where(e => e.IsItTemp == true && e.CreatedByUserId == userId).ToList());
                if (e.Napomena is null)
                {
                    e.Napomena = CheckIfExists(e.BrojRacuna, racunList);
                }

                racunList.Clear();
                e.RedniBroj = rbr++;
            }
            return racunHoldingList;
        }

        public static List<RacunHolding> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context)
        {
            List<RacunHolding> racunHoldingList = new();

            if (predmetIdAsInt is 0 && dopisIdAsInt is 0)
            {
                racunHoldingList = _context.RacunHolding.Where(e => e.IsItTemp == null).ToList();
            }

            racunHoldingList = _context.RacunHolding
                .Include(e => e.Stan)
                .Include(e => e.Dopis)
                .Include(e => e.Dopis.Predmet)
                .ToList();

            if (predmetIdAsInt is not 0)
            {
                racunHoldingList = dopisIdAsInt is 0
                    ? _context.RacunHolding.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt).ToList()
                    : _context.RacunHolding.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt && x.Dopis.Id == dopisIdAsInt).ToList();
            }

            return racunHoldingList;
        }
    }
}
