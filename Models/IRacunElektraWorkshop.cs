using aes.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IRacunElektraWorkshop
    {
        JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context);
        List<RacunElektra> GetListCreateList(string userId, ApplicationDbContext _context);
        List<RacunElektra> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context);
        List<RacunElektra> GetRacuniElektraForDatatables(DatatablesParams Params);
    }
}
