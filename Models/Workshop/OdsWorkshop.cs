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
    public class OdsWorkshop : IOdsWorkshop
    {
        public JsonResult GetStanData(string sid, ApplicationDbContext _context)
        {
            int idInt;
            if (sid is not null)
            {
                idInt = int.Parse(sid);
            }
            else
            {
                return new JsonResult(new { success = false, Message = "Greška, prazan id" });
            }
            Stan stan = _context.Stan.FirstOrDefault(o => o.Id == idInt);
            return new JsonResult(stan);
        }

        private List<Ods> GetStanoviForDatatables(IDatatablesParams Params, List<Ods> OdsList)
        {
            return OdsList
                .Where(
                    x => x.Omm.ToString().Contains(Params.SearchValue)
                    || x.Stan.StanId.ToString().Contains(Params.SearchValue)
                    || x.Stan.SifraObjekta.ToString().Contains(Params.SearchValue)
                    || (x.Stan.Adresa != null && x.Stan.Adresa.ToLower().Contains(Params.SearchValue.ToLower()))
                    || (x.Stan.Kat != null && x.Stan.Kat.ToLower().Contains(Params.SearchValue.ToLower()))
                    || (x.Stan.BrojSTana != null && x.Stan.BrojSTana.ToLower().Contains(Params.SearchValue.ToLower()))
                    || (x.Stan.Četvrt != null && x.Stan.Četvrt.ToLower().Contains(Params.SearchValue.ToLower()))
                    || x.Stan.Površina.ToString().Contains(Params.SearchValue)
                    || (x.Stan.StatusKorištenja != null && x.Stan.StatusKorištenja.ToLower().Contains(Params.SearchValue.ToLower()))
                    || (x.Stan.Korisnik != null && x.Stan.Korisnik.ToLower().Contains(Params.SearchValue.ToLower()))
                    || (x.Stan.Vlasništvo != null && x.Stan.Vlasništvo.ToLower().Contains(Params.SearchValue.ToLower()))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(Params.SearchValue.ToLower()))).ToDynamicList<Ods>();
        }

        public async Task<IActionResult> GetList(IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context,
    HttpRequest Request)
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<Ods> OdsList = await _context.Ods
                .Include(e => e.Stan)
                .ToListAsync();

            int totalRows = OdsList.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                OdsList = GetStanoviForDatatables(Params, OdsList);
            }
            int totalRowsAfterFiltering = OdsList.Count;
            return datatablesGenerator.SortingPaging(OdsList, Params, Request, totalRows, totalRowsAfterFiltering);
        }
    }
}
