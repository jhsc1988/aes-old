﻿using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IOdsWorkshop
    {
        Task<IActionResult> GetList(IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, HttpRequest Request);
        JsonResult GetStanData(string sid, ApplicationDbContext _context);
    }
}
