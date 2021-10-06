using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public class DatatablesParamsGenerator : IDatatablesParamsGenerator
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
    }
}
