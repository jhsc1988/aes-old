using aes.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IOdsWorkshop
    {
        JsonResult GetStanData(string sid, ApplicationDbContext _context);
        List<Ods> GetStanoviForDatatables(DatatablesParams Params, List<Ods> OdsList);
    }
}
