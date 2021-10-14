using aes.Data;
using aes.Models.RacuniWorkshop.IRacuniWorkshop;
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
    public class RacunElektraIzvrsenjeUslugeWorkshop : RacuniElektraIRateWorkshop, IRacunElektraIzvrsenjeUslugeWorkshop
    {

        public JsonResult AddNewTemp(string brojRacuna, string iznos, string datumPotvrde, string datumIzvrsenja, string usluga, string dopisId, string userId, ApplicationDbContext _context)
        {
            DateTime datumIzvrsenjaDT = new();
            if (usluga == null)
            {
                return new(new { success = false, Message = "Usluga je obavezna" });
            }

            if (datumIzvrsenja != null)
            {
                datumIzvrsenjaDT = DateTime.Parse(datumIzvrsenja);
            }

            if (!Validate(brojRacuna, iznos, datumPotvrde, dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja))
            {
                return new(new { success = false, Message = msg });
            }

            List<RacunElektraIzvrsenjeUsluge> racunElektraIzvrsenjeList = _context.RacunElektraIzvrsenjeUsluge.Where(e => e.IsItTemp == true && e.CreatedByUserId.Equals(userId)).ToList();
            RacunElektraIzvrsenjeUsluge re = new()
            {
                BrojRacuna = brojRacuna,
                Iznos = _iznos,
                DatumIzdavanja = datumIzdavanja,
                DatumIzvrsenja = datumIzvrsenjaDT,
                Usluga = usluga,
                DopisId = _dopisId == 0 ? null : _dopisId,
                CreatedByUserId = userId,
                IsItTemp = true,
            };

            re.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(re.BrojRacuna.Substring(0, 10)));
            racunElektraIzvrsenjeList.Add(re);

            int rbr = 1;
            foreach (RacunElektraIzvrsenjeUsluge e in racunElektraIzvrsenjeList)
            {
                e.RedniBroj = rbr++;
            }
            _ = _context.RacunElektraIzvrsenjeUsluge.Add(re);

            return TrySave(_context, false);
        }
        public List<RacunElektraIzvrsenjeUsluge> GetRacunElektraIzvrsenjeUslugeForDatatables(IDatatablesParams Params, List<RacunElektraIzvrsenjeUsluge> CreateRacuniElektraIzvrsenjeUslugeList)
        {
            CreateRacuniElektraIzvrsenjeUslugeList = CreateRacuniElektraIzvrsenjeUslugeList
            .Where(
                x => x.BrojRacuna.Contains(Params.SearchValue)
                || x.ElektraKupac.UgovorniRacun.ToString().Contains(Params.SearchValue)
                || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(Params.SearchValue)
                || x.DatumIzvrsenja.ToString("dd.MM.yyyy").Contains(Params.SearchValue)
                || (x.Usluga != null && x.Usluga.ToLower().Contains(Params.SearchValue.ToLower()))
                || x.Iznos.ToString().Contains(Params.SearchValue)
                || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(Params.SearchValue))
                || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(Params.SearchValue))
                || (x.Napomena != null && x.Napomena.ToLower().Contains(Params.SearchValue)))
            .ToDynamicList<RacunElektraIzvrsenjeUsluge>();
            return CreateRacuniElektraIzvrsenjeUslugeList;
        }
    }
}