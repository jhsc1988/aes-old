using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace aes.Models.Datatables
{
    public interface IDatatablesGenerator
    {
        DtParams GetParams(HttpRequest request);
        JsonResult SortingPaging<T>(IEnumerable<T> data, DtParams @params, HttpRequest request, int totalRows, int totalRowsAfterFiltering);
    }
}
