﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;

using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public class DatatablesGenerator : IDatatablesGenerator
    {
        public DatatablesParams GetParams(HttpRequest request)
        {
            return new DatatablesParams
            {
                Start = int.Parse(request.Form["start"].FirstOrDefault()),
                Length = int.Parse(request.Form["length"].FirstOrDefault()),
                SearchValue = request.Form["search[value]"].FirstOrDefault(),
                SortColumnName = request.Form["columns[" + request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(),
                SortDirection = request.Form["order[0][dir]"].FirstOrDefault(),
            };
        }
        public JsonResult SortingPaging<T>(List<T> data, DatatablesParams Params, HttpRequest request, int totalRows, int totalRowsAfterFiltering)
        {
            // todo: if(Params.SortDirection) - da maknem ovaj AsQueryable dependency
            data = data.AsQueryable().OrderBy(Params.SortColumnName + " " + Params.SortDirection).ToList(); // sorting
            data = data.Skip(Convert.ToInt32(Params.Start)).Take(Convert.ToInt32(Params.Length)).ToList(); // paging

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