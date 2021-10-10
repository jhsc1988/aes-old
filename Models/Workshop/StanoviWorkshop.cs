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
    public class StanoviWorkshop : IStanoviWorkshop
    {
        public List<Stan> GetStanoviForDatatables(IDatatablesParams Params, ApplicationDbContext _context, List<Stan> stanList)
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
            return GetStanoviForDatatables(Params, _context, StanList);
        }
        public JsonResult GetRacuniForStan(IRacunWorkshop racunWorkshop,
            IElektraKupacWorkshop elektraKupacWorkshop, HttpRequest Request, IDatatablesGenerator datatablesGenerator,
            IRacunElektraWorkshop racunElektraWorkshop, ApplicationDbContext context, int param)
        {
            ElektraKupac elektraKupac = racunWorkshop.GetKupacForStanId(context.ElektraKupac, param);
            if (elektraKupac is not null) param = elektraKupac.Id;
            return elektraKupacWorkshop.GetRacuniForKupac(param, datatablesGenerator, Request, racunWorkshop, context, racunElektraWorkshop);
        }
        public JsonResult GetRacuniRateForStan(IRacunWorkshop racunWorkshop, 
            IElektraKupacWorkshop elektraKupacWorkshop, HttpRequest Request, IDatatablesGenerator datatablesGenerator,
            IRacunElektraRateWorkshop racunElektraRateWorkshop, ApplicationDbContext context, int param)
        {
            ElektraKupac elektraKupac = racunWorkshop.GetKupacForStanId(context.ElektraKupac, param);
            if (elektraKupac is not null) param = elektraKupac.Id;
            return elektraKupacWorkshop.GetRacuniRateForKupac(param, datatablesGenerator, Request, racunWorkshop, context, racunElektraRateWorkshop);
        }
        public JsonResult GetRacuniElektraIzvrsenjeForStan(IRacunWorkshop racunWorkshop,
    IElektraKupacWorkshop elektraKupacWorkshop, HttpRequest Request, IDatatablesGenerator datatablesGenerator,
    IRacunElektraIzvrsenjeUslugeWorkshop racunElektraIzvrsenjeUslugeWorkshop, ApplicationDbContext context, int param)
        {
            ElektraKupac elektraKupac = racunWorkshop.GetKupacForStanId(context.ElektraKupac, param);
            if (elektraKupac is not null) param = elektraKupac.Id;
            return elektraKupacWorkshop.GetRacuniElektraIzvrsenjeForKupac(param, datatablesGenerator, Request, racunWorkshop, context, racunElektraIzvrsenjeUslugeWorkshop);
        }
        public JsonResult GetList(bool IsFiltered,IDatatablesGenerator datatablesGenerator, HttpRequest Request, ApplicationDbContext context)
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<Stan> StanList;
            if(IsFiltered) StanList = context.Stan.Where(p => !context.Ods.Any(o => o.StanId == p.Id)).ToList();
            else StanList = context.Stan.ToList();

            int totalRows = StanList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                StanList = GetStanoviForDatatables(Params, context, StanList);
            }

            int totalRowsAfterFiltering = StanList.Count;
            return datatablesGenerator.SortingPaging(StanList, Params, Request, totalRows, totalRowsAfterFiltering);

        }
    }
}
