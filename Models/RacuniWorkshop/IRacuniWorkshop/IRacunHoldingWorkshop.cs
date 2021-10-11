using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace aes.Models
{
    public interface IRacunHoldingWorkshop : IRacunWorkshop
    {
        JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context);
        List<RacunHolding> GetListCreateList(string userId, ApplicationDbContext _context);
        List<RacunHolding> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context);
        // List<RacunHolding> GetRacuniHoldingForDatatables(IDatatablesParams Params, ApplicationDbContext _context, List<RacunHolding> CreateRacuniHoldingList);
        JsonResult GetList(bool isFiltered, string klasa, string urbroj, IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, HttpRequest Request, string Uid, int param = 0);
    }
}
