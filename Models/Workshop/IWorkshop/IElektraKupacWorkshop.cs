using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IElektraKupacWorkshop
    {
        List<ElektraKupac> GetKupciForDatatables(IDatatablesParams Params, List<ElektraKupac> ElektraKupacList);
        JsonResult GetRacuniForKupac(int param, IDatatablesGenerator datatablesGenerator, HttpRequest Request, IRacunWorkshop racunWorkshop, ApplicationDbContext _context, IRacunElektraWorkshop racunElektraWorkshop);
        JsonResult GetRacuniRateForKupac(int param, IDatatablesGenerator datatablesGenerator, HttpRequest Request, IRacunWorkshop racunWorkshop, ApplicationDbContext _context, IRacunElektraRateWorkshop racunElektraRateWorkshop);
        JsonResult GetRacuniElektraIzvrsenjeForKupac(int param, IDatatablesGenerator datatablesGenerator, HttpRequest Request, IRacunWorkshop racunWorkshop, ApplicationDbContext _context, IRacunElektraIzvrsenjeUslugeWorkshop racunElektraIzvrsenjeWorkshop);

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća listu kupaca Elektre u JSON obliku za server side processing</returns>
        Task<IActionResult> GetList(IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, HttpRequest Request);
    }
}
