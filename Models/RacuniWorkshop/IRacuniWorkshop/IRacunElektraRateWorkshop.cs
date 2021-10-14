using aes.Data;
using aes.Models.RacuniWorkshop.IRacuniWorkshop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace aes.Models
{
    public interface IRacunElektraRateWorkshop : IRacuniElektraIRateWorkshop
    {
        JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context);
        List<RacunElektraRate> GetRacuniElektraRateForDatatables(IDatatablesParams Params, List<RacunElektraRate> CreateRacuniElektraRateList);
    }
}
