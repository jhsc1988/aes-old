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
    public class PredmetWorkshop : IPredmetWorkshop
    {
        public List<Predmet> GetPredmetiDataForFilter<T>(DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun
        {
            List<Predmet> predmetiList = new();
            foreach (Racun e in _modelcontext.ToList())
            {
                e.Dopis = _context.Dopis.FirstOrDefault(x => e.DopisId == x.Id);
                if (e.Dopis != null)
                {
                    predmetiList.Add(_context.Predmet.FirstOrDefault(x => e.Dopis.PredmetId == x.Id));
                }
            }
            return predmetiList.Distinct().ToList();
        }
        public List<Predmet> GetPredmetiForDatatables(IDatatablesParams Params, List<Predmet> predmetList)
        {
            return predmetList
                    .Where(
                        x => x.Klasa.Contains(Params.SearchValue)
                        || x.Naziv.Contains(Params.SearchValue)).ToDynamicList<Predmet>();
        }
        public JsonResult SaveToDB(string klasa, string naziv, ApplicationDbContext _context)
        {
            Predmet pTemp = new();
            pTemp.Klasa = klasa;
            pTemp.Naziv = naziv;
            _ = _context.Predmet.Add(pTemp);
            return TrySave(_context);
        }
        public static JsonResult TrySave(ApplicationDbContext _context)
        {
            try
            {
                _ = _context.SaveChanges();
                return new(new { success = true, Message = "Spremljeno" });
            }
            catch (DbUpdateException)
            {
                return new(new { success = false, Message = "Greška" });
            }
        }
        public async Task<IActionResult> GetList(IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context,
            HttpRequest Request)
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<Predmet> PredmetList = await _context.Predmet.ToListAsync();
            int totalRows = PredmetList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                PredmetList = GetPredmetiForDatatables(Params, PredmetList);
            }
            int totalRowsAfterFiltering = PredmetList.Count;
            return datatablesGenerator.SortingPaging(PredmetList, Params, Request, totalRows, totalRowsAfterFiltering);
        }
    }
}
