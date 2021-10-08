using aes.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IRacunHoldingWorkshop
    {
        JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context);
        List<RacunHolding> GetListCreateList(string userId, ApplicationDbContext _context);
        List<RacunHolding> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context);
        List<RacunHolding> GetRacuniHoldingForDatatables(DatatablesParams Params);
    }
}
