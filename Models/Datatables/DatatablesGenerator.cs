using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace aes.Models.Datatables
{
    public class DatatablesGenerator : IDatatablesGenerator
    {
        public DTParams GetParams(HttpRequest request)
        {

            DTParams dtParams = new()
            {
                Start = int.Parse(request.Form["start"].FirstOrDefault()),
                Length = int.Parse(request.Form["length"].FirstOrDefault()),
                SearchValue = request.Form["search[value]"].FirstOrDefault(),
                SortColumnName = request.Form["columns[" + request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(),
                SortDirection = request.Form["order[0][dir]"].FirstOrDefault(),
            };

            return dtParams;
        }
        public JsonResult SortingPaging<T>(IEnumerable<T> data, DTParams Params, HttpRequest request, int totalRows, int totalRowsAfterFiltering)
        {
            // TODO: if(Params.SortDirection) - da maknem ovaj AsQueryable dependency
            data = data.AsQueryable().OrderBy(Params.SortColumnName + " " + Params.SortDirection); // sorting
            data = data.Skip(Convert.ToInt32(Params.Start)).Take(Convert.ToInt32(Params.Length)); // paging

            return new JsonResult(new
            {
                data,
                draw = Convert.ToInt32(request.Form["draw"].FirstOrDefault()),
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }
    }
}

