using aes.Data;
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

            re.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(re.BrojRacuna.Substring(0, 10)));
            racunElektraList.Add(re);

            int rbr = 1;
            foreach (RacunElektra e in racunElektraList)
            {
                e.RedniBroj = rbr++;
            }

            _ = _context.RacunElektra.Add(re);

            try
            {
                _ = _context.SaveChanges();
                return new(new { success = true, Message = "Spremljeno" });

            }
            catch (DbUpdateException)
            {
                return new(new { success = false, Message = "Greška" });
            }
        }
        public static JsonResult RemoveAllFromDb(string userId, ApplicationDbContext _context)
        {

            List<RacunElektra> racunElektraList = _context.RacunElektra.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToList();
            _context.RacunElektra.RemoveRange(racunElektraList);

            return TryDelete(_context);
        }
        public static JsonResult SaveToDB(string _dopisid, string userId, ApplicationDbContext _context)
        {
            int dopisId = int.Parse(_dopisid);

            if (dopisId is 0)
            {
                return new(new { success = false, Message = "Nije odabran dopis!" });
            }

            List<RacunElektra> racunElektraList = _context.RacunElektra.Where(e => e.IsItTemp == true && e.CreatedByUserId.Equals(userId)).ToList();
            foreach (RacunElektra e in racunElektraList)
            {
                e.IsItTemp = null;
                e.DopisId = dopisId;
            }

            return TrySave(_context);
        }
    }
}