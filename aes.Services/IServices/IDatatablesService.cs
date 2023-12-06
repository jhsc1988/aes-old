using aes.Models.Datatables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace aes.Services.IServices
{
    public interface IDatatablesService<TEntity> where TEntity : class
    {
        JsonResult GetData(HttpRequest Request, IEnumerable<TEntity> list, IDatatablesGenerator datatablesGenerator, Func<IEnumerable<TEntity>, DtParams, IEnumerable<TEntity>> dtData);
    }
}