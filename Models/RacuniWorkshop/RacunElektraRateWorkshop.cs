using aes.Data;
using aes.Models.Workshop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace aes.Models
{
    public class RacunElektraRateWorkshop : RacunWorkshop, IRacunElektraRateWorkshop
    {
        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context)
        {
            if (!Validate(brojRacuna, iznos, date, dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja))
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

            return TrySave(_context, false);
        }
        public List<RacunElektraRate> GetRacuniElektraRateForDatatables(IDatatablesParams Params, List<RacunElektraRate> CreateRacuniElektraRateList)
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
    }
}
