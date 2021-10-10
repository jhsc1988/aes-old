using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IStanoviWorkshop
    {
         List<Stan> GetFilteredListForOds(IDatatablesParams Params, ApplicationDbContext _context);
        JsonResult GetList(bool IsFiltered, IDatatablesGenerator datatablesGenerator, HttpRequest Request, ApplicationDbContext context);
        JsonResult GetRacuniElektraIzvrsenjeForStan(IRacunWorkshop racunWorkshop, IElektraKupacWorkshop elektraKupacWorkshop, HttpRequest Request, IDatatablesGenerator datatablesGenerator, IRacunElektraIzvrsenjeUslugeWorkshop racunElektraIzvrsenjeUslugeWorkshop, ApplicationDbContext context, int param);
        JsonResult GetRacuniForStan(IRacunWorkshop racunWorkshop, IElektraKupacWorkshop elektraKupacWorkshop, HttpRequest Request, IDatatablesGenerator datatablesGenerator, IRacunElektraWorkshop racunElektraWorkshop, ApplicationDbContext context, int param);
        JsonResult GetRacuniRateForStan(IRacunWorkshop racunWorkshop, IElektraKupacWorkshop elektraKupacWorkshop, HttpRequest Request, IDatatablesGenerator datatablesGenerator, IRacunElektraRateWorkshop racunElektraRateWorkshop, ApplicationDbContext context, int param);
        // List<Stan> GetStanoviForDatatables(IDatatablesParams Params, ApplicationDbContext _context, List<Stan> stanList);
    }
}
