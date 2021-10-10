using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IDopisWorkshop
    {
        JsonResult GetList(IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, HttpRequest Request, int predmetId);
        JsonResult SaveToDB(string predmetId, string urbroj, string datumDopisa, ApplicationDbContext _context);
    }
}
