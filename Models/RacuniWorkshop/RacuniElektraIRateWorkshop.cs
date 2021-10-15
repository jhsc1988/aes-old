using aes.Data;
using aes.Models.Workshop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models.RacuniWorkshop.IRacuniWorkshop
{
    public class RacuniElektraIRateWorkshop : RacunWorkshop, IRacuniElektraIRateWorkshop
    {
        public List<T> GetList<T>(int predmetIdAsInt, int dopisIdAsInt, DbSet<T> modelcontext) where T : Elektra
        {
            // todo: RacunElektraRateList ne saljem u GetRacuniFromDb -> vraca mi cijelu listu racuna, ne filtrira po predmetu
            IQueryable<T> RacunElektraRateList = predmetIdAsInt is 0 && dopisIdAsInt is 0
                ? modelcontext.Where(e => e.IsItTemp == null)
                : dopisIdAsInt is 0
                    ? modelcontext.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt)
                    : modelcontext.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt && x.Dopis.Id == dopisIdAsInt);
            return RacunElektraRateList
                .Include(e => e.ElektraKupac)
                .Include(e => e.ElektraKupac.Ods)
                .Include(e => e.ElektraKupac.Ods.Stan)
                .ToList();
        }
        public JsonResult GetListMe<T>(bool isFiltered, string klasa, string urbroj, IDatatablesGenerator datatablesGenerator,
ApplicationDbContext _context, DbSet<T> modelcontext, HttpRequest Request, string Uid) where T : Elektra
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<T> list;
            if (isFiltered)
            {
                int predmetIdAsInt = klasa is null ? 0 : int.Parse(klasa);
                int dopisIdAsInt = urbroj is null ? 0 : int.Parse(urbroj);
                list = GetList(predmetIdAsInt, dopisIdAsInt, modelcontext);
            }
            else
            {
                list = GetListCreateList(Uid, _context, modelcontext);
            }

            int totalRows = list.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                switch (list)
                {
                    case List<RacunElektra>:
                        list = new RacunElektraWorkshop().GetRacuniElektraForDatatables(Params, list as List<RacunElektra>) as List<T>;
                        break;
                    case List<RacunElektraRate>:
                        list = new RacunElektraRateWorkshop().GetRacuniElektraRateForDatatables(Params, list as List<RacunElektraRate>) as List<T>;
                        break;
                    case List<RacunElektraIzvrsenjeUsluge>:
                        list = new RacunElektraIzvrsenjeUslugeWorkshop().GetRacunElektraIzvrsenjeUslugeForDatatables(Params, list as List<RacunElektraIzvrsenjeUsluge>) as List<T>;
                        break;
                    default:
                        break;
                }
            }
            int totalRowsAfterFiltering = list.Count;
            return datatablesGenerator.SortingPaging(list, Params, Request, totalRows, totalRowsAfterFiltering);
        }

        public List<T> GetListCreateList<T>(string userId, ApplicationDbContext _context, DbSet<T> modelcontext) where T : Elektra
        {
            List<T> list = modelcontext.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToList();

            int rbr = 1;
            foreach (Elektra e in list)
            {
                e.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(e.BrojRacuna.Substring(0, 10)));

                List<Racun> racunList = new();
                racunList.AddRange(modelcontext.Where(e => e.IsItTemp == null || false).ToList());
                e.Napomena = CheckIfExistsInPayed(e.BrojRacuna, racunList);
                racunList.Clear();

                racunList.AddRange(modelcontext.Where(e => e.IsItTemp == true && e.CreatedByUserId == userId).ToList());
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
            return list;
        }

        public List<T> GetRacuniFromDb<T>(DbSet<T> modelcontext, int param = 0) where T : Elektra
            => param switch
            {
                not 0 => modelcontext
                                .Include(e => e.ElektraKupac)
                                .Include(e => e.ElektraKupac.Ods)
                                .Include(e => e.ElektraKupac.Ods.Stan)
                                .Where(e => e.ElektraKupac.Id == param)
                                .ToList(),
                _ => modelcontext
                        .Include(e => e.ElektraKupac)
                        .Include(e => e.ElektraKupac.Ods)
                        .Include(e => e.ElektraKupac.Ods.Stan)
                        .ToList()
            };
    }
}
