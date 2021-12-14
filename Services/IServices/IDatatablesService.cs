﻿using aes.Models.Datatables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.IServices
{
    public interface IDatatablesService<TEntity> where TEntity : class
    {
        Task<JsonResult> GetData(HttpRequest Request, IEnumerable<TEntity> list, IDatatablesGenerator datatablesGenerator, Func<IEnumerable<TEntity>, DTParams, IEnumerable<TEntity>> dtData);
    }
}