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
    public class ElektraKupacWorkshop : IElektraKupacWorkshop
    {
        public List<ElektraKupac> GetKupciForDatatables(IDatatablesParams Params, List<ElektraKupac> ElektraKupacList)
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

        public JsonResult GetRacuniElektraIzvrsenjeForKupac(int param, IDatatablesGenerator datatablesGenerator,
            HttpRequest Request, IRacunWorkshop racunWorkshop, ApplicationDbContext _context, IRacunElektraIzvrsenjeUslugeWorkshop racunElektraIzvrsenjeWorkshop)
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<RacunElektraIzvrsenjeUsluge> RacunElektraIzvrsenjeList = racunWorkshop.GetRacuniFromDb(_context.RacunElektraIzvrsenjeUsluge, param);
            int totalRows = RacunElektraIzvrsenjeList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                RacunElektraIzvrsenjeList = racunElektraIzvrsenjeWorkshop.GetRacunElektraIzvrsenjeUslugeForDatatables(Params, _context, RacunElektraIzvrsenjeList);
            }
            return datatablesGenerator.SortingPaging(RacunElektraIzvrsenjeList, Params, Request, totalRows, RacunElektraIzvrsenjeList.Count);
        }

        public JsonResult GetRacuniForKupac(int param, IDatatablesGenerator datatablesGenerator,
            HttpRequest Request, IRacunWorkshop racunWorkshop, ApplicationDbContext _context, IRacunElektraWorkshop racunElektraWorkshop)
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<RacunElektra> RacunElektraList = racunWorkshop.GetRacuniFromDb(_context.RacunElektra, param);
            int totalRows = RacunElektraList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                RacunElektraList = racunElektraWorkshop.GetRacuniElektraForDatatables(Params, _context, RacunElektraList);
            }
            return datatablesGenerator.SortingPaging(RacunElektraList, Params, Request, totalRows, RacunElektraList.Count);
        }

        public JsonResult GetRacuniRateForKupac(int param, IDatatablesGenerator datatablesGenerator,
            HttpRequest Request, IRacunWorkshop racunWorkshop, ApplicationDbContext _context, IRacunElektraRateWorkshop racunElektraRateWorkshop)
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<RacunElektraRate> RacunElektraRateList = racunWorkshop.GetRacuniFromDb(_context.RacunElektraRate, param);
            int totalRows = RacunElektraRateList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                RacunElektraRateList = racunElektraRateWorkshop.GetRacuniElektraRateForDatatables(Params, _context, RacunElektraRateList);
            }
            return datatablesGenerator.SortingPaging(RacunElektraRateList, Params, Request, totalRows, RacunElektraRateList.Count);
        }
    }
}
