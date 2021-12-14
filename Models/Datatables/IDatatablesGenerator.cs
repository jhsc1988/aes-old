using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace aes.Models.Datatables
{
    public interface IDatatablesGenerator
    {
        DTParams GetParams(HttpRequest request);
        JsonResult SortingPaging<T>(IEnumerable<T> data, DTParams Params, HttpRequest request, int totalRows, int totalRowsAfterFiltering);
    }
}
