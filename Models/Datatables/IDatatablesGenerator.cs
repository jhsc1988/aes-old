using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace aes.Models
{
    public interface IDatatablesGenerator
    {
        IDatatablesParams GetParams(HttpRequest request);
        JsonResult SortingPaging<T>(List<T> list, IDatatablesParams Params, HttpRequest request, int totalRows, int totalRowsAfterFiltering);
    }
}
