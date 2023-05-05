using aes.Models.Datatables;
using aes.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aes.Services
{
    public class DatatablesService<TEntity> : IDatatablesService<TEntity> where TEntity : class
    {
        public JsonResult GetData(HttpRequest Request, IEnumerable<TEntity> list,
            IDatatablesGenerator datatablesGenerator, Func<IEnumerable<TEntity>, DTParams, IEnumerable<TEntity>> dtData)
        {
            DTParams dTParams = datatablesGenerator.GetParams(Request);

            int totalRows = list.Count();

            if (!string.IsNullOrEmpty(dTParams.SearchValue))
            {
                list = dtData(list, dTParams);
            }
            return datatablesGenerator.SortingPaging(list, dTParams, Request, totalRows, list.Count());
        }
    }
}
