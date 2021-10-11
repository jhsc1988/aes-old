using aes.Data;
using aes.Models.Workshop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace aes.Models
{
    public class RacunElektraWorkshop : RacunWorkshop, IRacunElektraWorkshop
    {
        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context)
        {

            if (!Validate(brojRacuna, iznos, date, dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja))
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

            return TrySave(_context, false);
        }
        public List<RacunElektra> GetRacuniElektraForDatatables(IDatatablesParams Params, List<RacunElektra> CreateRacuniElektraList)
        {
            CreateRacuniElektraList = CreateRacuniElektraList.Where(
                       x => x.BrojRacuna.Contains(Params.SearchValue)
                            || x.ElektraKupac.UgovorniRacun.ToString().Contains(Params.SearchValue)
                            || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(Params.SearchValue)
                            || x.Iznos.ToString().Contains(Params.SearchValue)
                            || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(Params.SearchValue))
                            || (x.DatumPotvrde != null &&
                                x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(Params.SearchValue))
                            || (x.Napomena != null && x.Napomena.ToLower().Contains(Params.SearchValue.ToLower())))
                   .ToDynamicList<RacunElektra>();
            return CreateRacuniElektraList;
        }
    }
}
