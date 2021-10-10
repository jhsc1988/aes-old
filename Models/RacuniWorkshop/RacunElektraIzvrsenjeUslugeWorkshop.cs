using aes.Data;
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
    public class RacunElektraIzvrsenjeUslugeWorkshop : IRacunElektraIzvrsenjeUslugeWorkshop
    {
        private static readonly IRacunWorkshop racunWorkshop = new RacunWorkshop();
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

            if (!racunWorkshop.Validate(brojRacuna, iznos, datumPotvrde, dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja))
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

            return racunWorkshop.TrySave(_context);
        }

        public List<RacunElektraIzvrsenjeUsluge> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context)
        {
            IQueryable<RacunElektraIzvrsenjeUsluge> racunElektraIzvrsenjeList = predmetIdAsInt is 0
                ? _context.RacunElektraIzvrsenjeUsluge.Where(e => e.IsItTemp == null)
                : dopisIdAsInt == 0
                    ? _context.RacunElektraIzvrsenjeUsluge.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt)
                    : _context.RacunElektraIzvrsenjeUsluge.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt && x.Dopis.Id == dopisIdAsInt);
            return racunElektraIzvrsenjeList
                .Include(e => e.ElektraKupac)
                .Include(e => e.ElektraKupac.Ods)
                .Include(e => e.ElektraKupac.Ods.Stan)
                .Include(e => e.Dopis)
                .Include(e => e.Dopis.Predmet)
                .ToList();
        }

        public List<RacunElektraIzvrsenjeUsluge> GetListCreateList(string userId, ApplicationDbContext _context)
        {
            List<RacunElektraIzvrsenjeUsluge> racunElektraIzvrsenjeList = _context.RacunElektraIzvrsenjeUsluge.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToList();

            int rbr = 1;
            foreach (RacunElektraIzvrsenjeUsluge e in racunElektraIzvrsenjeList)
            {

                e.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(e.BrojRacuna.Substring(0, 10)));

                List<Racun> racunList = new();
                racunList.AddRange(_context.RacunElektraIzvrsenjeUsluge.Where(e => e.IsItTemp == null || false).ToList());
                e.Napomena = racunWorkshop.CheckIfExistsInPayed(e.BrojRacuna, racunList);
                racunList.Clear();

                racunList.AddRange(_context.RacunElektraIzvrsenjeUsluge.Where(e => e.IsItTemp == true && e.CreatedByUserId == userId).ToList());
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
            return racunElektraIzvrsenjeList;
        }

        public List<RacunElektraIzvrsenjeUsluge> GetRacunElektraIzvrsenjeUslugeForDatatables(IDatatablesParams Params, ApplicationDbContext _context, List<RacunElektraIzvrsenjeUsluge> CreateRacuniElektraIzvrsenjeUslugeList)
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

        public JsonResult GetList(bool isFiltered, string klasa, string urbroj, IDatatablesGenerator datatablesGenerator,
ApplicationDbContext _context, HttpRequest Request, string Uid)
        {
            List<RacunElektraIzvrsenjeUsluge> racuniElektraIzvrsenjeUslugeList;
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            if (isFiltered)
            {
                int predmetIdAsInt = klasa is null ? 0 : int.Parse(klasa);
                int dopisIdAsInt = urbroj is null ? 0 : int.Parse(urbroj);
                racuniElektraIzvrsenjeUslugeList = GetList(predmetIdAsInt, dopisIdAsInt, _context);
            }
            else
            {
                racuniElektraIzvrsenjeUslugeList = GetListCreateList(Uid, _context);
            }
            int totalRows = racuniElektraIzvrsenjeUslugeList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                racuniElektraIzvrsenjeUslugeList = GetRacunElektraIzvrsenjeUslugeForDatatables(Params, _context, racuniElektraIzvrsenjeUslugeList);
            }
            int totalRowsAfterFiltering = racuniElektraIzvrsenjeUslugeList.Count;
            return datatablesGenerator.SortingPaging(racuniElektraIzvrsenjeUslugeList, Params, Request, totalRows, totalRowsAfterFiltering);
        }
    }
}
