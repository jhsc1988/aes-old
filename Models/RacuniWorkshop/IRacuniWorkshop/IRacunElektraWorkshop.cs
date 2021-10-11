using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace aes.Models
{
    public interface IRacunElektraWorkshop : IRacunWorkshop
    {
        JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context);
        List<RacunElektra> GetRacuniElektraForDatatables(IDatatablesParams Params, List<RacunElektra> CreateRacuniElektraList);
    }
}
