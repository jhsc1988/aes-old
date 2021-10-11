using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace aes.Models
{
    public class StanoviWorkshop : IStanoviWorkshop
    {
        private static List<Stan> GetStanoviForDatatables(IDatatablesParams Params, List<Stan> stanList)
        {
            return stanList.Where(
                                x => x.StanId.ToString().Contains(Params.SearchValue)
                                    || x.SifraObjekta.ToString().Contains(Params.SearchValue)
                                    || (x.Adresa != null && x.Adresa.ToLower().Contains(Params.SearchValue.ToLower()))
                                    || (x.Kat != null && x.Kat.ToLower().Contains(Params.SearchValue.ToLower()))
                                    || (x.BrojSTana != null && x.BrojSTana.ToLower().Contains(Params.SearchValue.ToLower()))
                                    || (x.Četvrt != null && x.Četvrt.ToLower().Contains(Params.SearchValue.ToLower()))
                                    || x.Površina.ToString().Contains(Params.SearchValue)
                                    || (x.StatusKorištenja != null &&
                                    x.StatusKorištenja.ToLower().Contains(Params.SearchValue.ToLower()))
                                    || (x.Korisnik != null && x.Korisnik.ToLower().Contains(Params.SearchValue.ToLower()))
                                    || (x.Vlasništvo != null && x.Vlasništvo.ToLower().Contains(Params.SearchValue.ToLower())))
                            .ToDynamicList<Stan>();
        }
        public List<Stan> GetFilteredListForOds(IDatatablesParams Params, ApplicationDbContext _context)
        {
            List<Stan> StanList = _context.Stan.Where(p => !_context.Ods.Any(o => o.StanId == p.Id)).ToList();
            return GetStanoviForDatatables(Params, StanList);
        }
        public JsonResult GetRacuniForStan(IRacunWorkshop racunWorkshop,
            IElektraKupacWorkshop elektraKupacWorkshop, HttpRequest Request, IDatatablesGenerator datatablesGenerator,
            IRacunElektraWorkshop racunElektraWorkshop, ApplicationDbContext context, int param)
        {
            ElektraKupac elektraKupac = elektraKupacWorkshop.GetElektraKupacForStanId(context.ElektraKupac, param);
            if (elektraKupac is not null) param = elektraKupac.Id;
            return elektraKupacWorkshop.GetRacuniForKupac(param, datatablesGenerator, Request, context, racunElektraWorkshop, context.RacunElektra);
        }
        public JsonResult GetRacuniRateForStan(IRacunWorkshop racunWorkshop,
            IElektraKupacWorkshop elektraKupacWorkshop, HttpRequest Request, IDatatablesGenerator datatablesGenerator,
            IRacunElektraRateWorkshop racunElektraRateWorkshop, ApplicationDbContext context, int param)
        {
            ElektraKupac elektraKupac = elektraKupacWorkshop.GetElektraKupacForStanId(context.ElektraKupac, param);
            if (elektraKupac is not null) param = elektraKupac.Id;
            return elektraKupacWorkshop.GetRacuniForKupac(param, datatablesGenerator, Request, context, racunElektraRateWorkshop, context.RacunElektraRate);
        }
        public JsonResult GetRacuniElektraIzvrsenjeForStan(IRacunWorkshop racunWorkshop,
            IElektraKupacWorkshop elektraKupacWorkshop, HttpRequest Request, IDatatablesGenerator datatablesGenerator,
            IRacunElektraIzvrsenjeUslugeWorkshop racunElektraIzvrsenjeUslugeWorkshop, ApplicationDbContext context, int param)
        {
            ElektraKupac elektraKupac = elektraKupacWorkshop.GetElektraKupacForStanId(context.ElektraKupac, param);
            if (elektraKupac is not null) param = elektraKupac.Id;
            return elektraKupacWorkshop.GetRacuniForKupac(param, datatablesGenerator, Request, context, racunElektraIzvrsenjeUslugeWorkshop, context.RacunElektraIzvrsenjeUsluge);
        }
        public JsonResult GetList(bool IsFiltered, IDatatablesGenerator datatablesGenerator, HttpRequest Request, ApplicationDbContext context)
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<Stan> StanList = IsFiltered ? context.Stan.Where(p => !context.Ods.Any(o => o.StanId == p.Id)).ToList() : context.Stan.ToList();
            int totalRows = StanList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                StanList = GetStanoviForDatatables(Params, StanList);
            }
            int totalRowsAfterFiltering = StanList.Count;
            return datatablesGenerator.SortingPaging(StanList, Params, Request, totalRows, totalRowsAfterFiltering);
        }
    }
}
