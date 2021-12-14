using aes.CommonDependecies;
using aes.Models;
using aes.Models.Datatables;
using aes.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Services
{
    public class LetterService : ILetterService
    {
        private readonly ICommonDependencies _c;

        public LetterService(ICommonDependencies c)
        {
            _c = c;
        }

        public async Task<JsonResult> GetList(int predmetId, HttpRequest Request)
        {
            DTParams dTParams = _c.DatatablesGenerator.GetParams(Request);

            IEnumerable<Dopis> DopisList = await _c.UnitOfWork.Letter.GetLettersForCaseFile(predmetId);

            int totalRows = DopisList.Count();
            if (!string.IsNullOrEmpty(dTParams.SearchValue))
            {
                DopisList = _c.DatatablesSearch.GetLettersForDatatables(DopisList, dTParams);
            }

            return _c.DatatablesGenerator.SortingPaging(DopisList, dTParams, Request, totalRows, DopisList.Count());
        }

        public async Task<JsonResult> SaveToDB(string predmetId, string urbroj, string datumDopisa)
        {
            Dopis dTemp = new();

            dTemp.PredmetId = int.Parse(predmetId);
            dTemp.Urbroj = urbroj;
            dTemp.Datum = DateTime.Parse(datumDopisa);

            _c.UnitOfWork.Letter.Add(dTemp);

            int numOfSaved = await _c.UnitOfWork.Complete();

            return numOfSaved == 0
                ? (new(new { success = false, Message = "Error" }))
                : (new(new { success = true, Message = numOfSaved }));
        }
    }
}
