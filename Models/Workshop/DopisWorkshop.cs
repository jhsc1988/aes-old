using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace aes.Models.Workshop
{
    public class DopisWorkshop : IDopisWorkshop
    {
        private static List<Dopis> GetDopisiForDatatables(IDatatablesParams Params, List<Dopis> DopisList)
        {
            return DopisList
                .Where(
                    x => x.Predmet.Klasa.Contains(Params.SearchValue)
                    || x.Predmet.Naziv.ToLower().Contains(Params.SearchValue.ToLower())
                    || x.Datum.ToString().Contains(Params.SearchValue)
                    || x.Urbroj.Contains(Params.SearchValue)).ToDynamicList<Dopis>();
        }
        public JsonResult GetList(IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context,
            HttpRequest Request, int predmetId)
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<Dopis> DopisList = _context.Dopis
                .Include(e => e.Predmet)
                .Where(e => e.PredmetId == predmetId)
                .ToList();

            int totalRows = DopisList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                DopisList = GetDopisiForDatatables(Params, DopisList);
            }
            int totalRowsAfterFiltering = DopisList.Count;
            return datatablesGenerator.SortingPaging(DopisList, Params, Request, totalRows, totalRowsAfterFiltering);
        }
        public JsonResult SaveToDB(string predmetId, string urbroj, string datumDopisa, ApplicationDbContext _context)
        {
            Dopis dTemp = new();
            dTemp.PredmetId = int.Parse(predmetId);
            dTemp.Urbroj = urbroj;
            dTemp.Datum = DateTime.Parse(datumDopisa);
            _ = _context.Dopis.Add(dTemp);
            return PredmetWorkshop.TrySave(_context);
        }

    }
}
