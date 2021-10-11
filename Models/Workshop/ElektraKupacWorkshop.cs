using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace aes.Models
{
    public class ElektraKupacWorkshop : IElektraKupacWorkshop
    {
        private static List<ElektraKupac> GetKupciForDatatables(IDatatablesParams Params, List<ElektraKupac> ElektraKupacList)
        {
            return ElektraKupacList
                .Where(
                    x => x.UgovorniRacun.ToString().Contains(Params.SearchValue)
                    || x.Ods.Omm.ToString().Contains(Params.SearchValue)
                    || x.Ods.Stan.StanId.ToString().Contains(Params.SearchValue)
                    || x.Ods.Stan.SifraObjekta.ToString().Contains(Params.SearchValue)
                    || (x.Ods.Stan.Adresa != null && x.Ods.Stan.Adresa.ToLower().Contains(Params.SearchValue.ToLower()))
                    || (x.Ods.Stan.Kat != null && x.Ods.Stan.Kat.ToLower().Contains(Params.SearchValue.ToLower()))
                    || (x.Ods.Stan.BrojSTana != null && x.Ods.Stan.BrojSTana.ToLower().Contains(Params.SearchValue.ToLower()))
                    || (x.Ods.Stan.Četvrt != null && x.Ods.Stan.Četvrt.ToLower().Contains(Params.SearchValue.ToLower()))
                    || x.Ods.Stan.Površina.ToString().Contains(Params.SearchValue)
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(Params.SearchValue.ToLower()))).ToDynamicList<ElektraKupac>();
        }

        public async Task<IActionResult> GetList(IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context,
            HttpRequest Request)
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<ElektraKupac> ElektraKupacList = await _context.ElektraKupac
                .Include(e => e.Ods)
                .Include(e => e.Ods.Stan)
                .ToListAsync();

            int totalRows = ElektraKupacList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                ElektraKupacList = GetKupciForDatatables(Params, ElektraKupacList);
            }
            int totalRowsAfterFiltering = ElektraKupacList.Count;
            return datatablesGenerator.SortingPaging(ElektraKupacList, Params, Request, totalRows, totalRowsAfterFiltering);
        }

        public JsonResult GetRacuniForKupac<T>(int param, IDatatablesGenerator datatablesGenerator,
            HttpRequest Request, ApplicationDbContext _context, IRacunWorkshop workshop, DbSet<T> modelcontext) where T : Elektra
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<T> list = workshop.GetRacuniFromDb(modelcontext, param);
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
            return datatablesGenerator.SortingPaging(list, Params, Request, totalRows, list.Count);
        }
        public ElektraKupac GetElektraKupacForStanId<T>(DbSet<T> modelcontext, int param) where T : ElektraKupac
        {
            return modelcontext
                .Include(e => e.Ods)
                .Include(e => e.Ods.Stan)
                .FirstOrDefault(e => e.Ods.Stan.Id == param);
        }
    }
}
