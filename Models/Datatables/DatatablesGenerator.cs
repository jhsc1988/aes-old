using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace aes.Models.Datatables
{
    public class DatatablesGenerator : IDatatablesGenerator
    {
        public DtParams GetParams(HttpRequest request)
        {
            var form = request.Form;

            var startStr = form["start"].FirstOrDefault();
            var lengthStr = form["length"].FirstOrDefault();
            var sortColumnIndex = form["order[0][column]"].FirstOrDefault();

            if (int.TryParse(startStr, out int start) && int.TryParse(lengthStr, out int length))
            {
                return new DtParams
                {
                    Start = start,
                    Length = length,
                    SearchValue = form["search[value]"].FirstOrDefault(),
                    SortColumnName = form[$"columns[{sortColumnIndex}][name]"].FirstOrDefault(),
                    SortDirection = form["order[0][dir]"].FirstOrDefault(),
                };
            }

            // You could throw an exception here or handle this case appropriately.
            return null;
        }

        public JsonResult SortingPaging<T>(IEnumerable<T> data, DtParams Params, HttpRequest request, int totalRows,
            int totalRowsAfterFiltering)
        {
            var queryableData = data as IQueryable<T> ?? data.AsQueryable();

            var sortedData = queryableData.OrderBy($"{Params.SortColumnName} {Params.SortDirection}");
            var pagedData = sortedData.Skip(Params.Start).Take(Params.Length);

            var draw = int.TryParse(request.Form["draw"].FirstOrDefault(), out var drawInt) ? drawInt : 0;

            return new JsonResult(new
            {
                data = pagedData,
                draw,
                recordsTotal = totalRows,
                recordsFiltered = totalRowsAfterFiltering
            });
        }
    }
}