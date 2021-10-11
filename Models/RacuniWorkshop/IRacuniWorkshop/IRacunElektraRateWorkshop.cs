using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace aes.Models
{
    public interface IRacunElektraRateWorkshop : IRacunWorkshop
    {
        JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context);
        //List<RacunElektraRate> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context);

        //List<RacunElektraRate> GetListCreateList(string userId, ApplicationDbContext _context);
        //List<RacunElektraRate> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context);
        List<RacunElektraRate> GetRacuniElektraRateForDatatables(IDatatablesParams Params, List<RacunElektraRate> CreateRacuniElektraRateList);
        //JsonResult GetList(bool isFiltered, string klasa, string urbroj, IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, HttpRequest Request, string Uid);
    }
}
