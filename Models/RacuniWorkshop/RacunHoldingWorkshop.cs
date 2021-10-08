using aes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace aes.Models
{
    public class RacunHoldingWorkshop : IRacunHoldingWorkshop
    {
        private static readonly IRacunWorkshop racunWorkshop = new RacunWorkshop();

        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context)
        {
            if (!racunWorkshop.Validate(brojRacuna, iznos, date, dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja))
            {
                return new(new { success = false, Message = msg });
            }

            // TODO: zasto ne koristim staticni clan (RacunHoldingList) za ovo ?
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
            return racunWorkshop.TrySave(_context);
        }

        public List<RacunHolding> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context)
        {
            IQueryable<RacunHolding> racunHoldingList = predmetIdAsInt is 0
                ? _context.RacunHolding.Where(e => e.IsItTemp == null)
                : dopisIdAsInt is 0
                    ? _context.RacunHolding.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt)
                    : _context.RacunHolding.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt && x.Dopis.Id == dopisIdAsInt);
            return racunHoldingList
                .Include(e => e.Stan)
                .Include(e => e.Dopis)
                .Include(e => e.Dopis.Predmet)
                .ToList();
        }

        public List<RacunHolding> GetListCreateList(string userId, ApplicationDbContext _context)
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
                e.Napomena = racunWorkshop.CheckIfExistsInPayed(e.BrojRacuna, racunList);
                racunList.Clear();

                racunList.AddRange(_context.RacunHolding.Where(e => e.IsItTemp == true && e.CreatedByUserId == userId).ToList());
                if (e.Napomena is null)
                {
                    e.Napomena = racunWorkshop.CheckIfExists(e.BrojRacuna, racunList);
                }

                racunList.Clear();
                e.RedniBroj = rbr++;
            }
            return racunHoldingList;
        }

        public List<RacunHolding> GetRacuniHoldingForDatatables(DatatablesParams Params, ApplicationDbContext _context, List<RacunHolding> CreateRRacuniHoldingList)
        {
            CreateRRacuniHoldingList = CreateRRacuniHoldingList.Where(
                x => x.BrojRacuna.Contains(Params.SearchValue)
                || x.Stan.SifraObjekta.ToString().Contains(Params.SearchValue)
                || x.Stan.StanId.ToString().Contains(Params.SearchValue)
                || x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(Params.SearchValue)
                || x.Iznos.ToString().Contains(Params.SearchValue)
                || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(Params.SearchValue))
                || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(Params.SearchValue))
                || (x.Napomena != null && x.Napomena.ToLower().Contains(Params.SearchValue.ToLower()))).ToDynamicList<RacunHolding>();
            return CreateRRacuniHoldingList;
        }
    }
}
