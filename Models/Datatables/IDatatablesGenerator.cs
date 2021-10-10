using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IDatatablesGenerator
    {
        IDatatablesParams GetParams(HttpRequest request);
        JsonResult SortingPaging<T>(List<T> list, IDatatablesParams Params, HttpRequest request, int totalRows, int totalRowsAfterFiltering);
    }
}
