using aes.Data;
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
        Task<IActionResult> GetList(IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, HttpRequest Request, IOdsWorkshop odsWorkshop);
        JsonResult GetStanData(string sid, ApplicationDbContext _context);
        List<Ods> GetStanoviForDatatables(IDatatablesParams Params, List<Ods> OdsList);
    }
}
