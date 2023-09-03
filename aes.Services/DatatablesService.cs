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
        public JsonResult GetData(HttpRequest request, IEnumerable<TEntity> list,
            IDatatablesGenerator datatablesGenerator, Func<IEnumerable<TEntity>, DtParams, IEnumerable<TEntity>> dtData)
        {
            DtParams dTParams = datatablesGenerator.GetParams(request);
    
            var dataList = list as IList<TEntity> ?? list.ToList();
    
            int totalRows = dataList.Count;

            if (string.IsNullOrEmpty(dTParams.SearchValue))
            {
                return datatablesGenerator.SortingPaging(dataList, dTParams, request, totalRows, totalRows);
            }

            var filteredData = dtData(dataList, dTParams).ToList();
            int filteredRows = filteredData.Count;

            return datatablesGenerator.SortingPaging(filteredData, dTParams, request, totalRows, filteredRows);
        }

    }
}