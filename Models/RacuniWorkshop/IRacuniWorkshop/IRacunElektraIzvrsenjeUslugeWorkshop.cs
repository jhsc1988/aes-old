using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IRacunElektraIzvrsenjeUslugeWorkshop
    {
        List<RacunElektraIzvrsenjeUsluge> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context);
        List<RacunElektraIzvrsenjeUsluge> GetListCreateList(string userId, ApplicationDbContext _context);
        JsonResult AddNewTemp(string brojRacuna, string iznos, string datumPotvrde, string datumIzvrsenja, string usluga, string dopisId, string userId, ApplicationDbContext _context);
        List<RacunElektraIzvrsenjeUsluge> GetRacunElektraIzvrsenjeUslugeForDatatables(IDatatablesParams Params, ApplicationDbContext _context, List<RacunElektraIzvrsenjeUsluge> CreateRacuniElektraIzvrsenjeUslugeList);
        JsonResult GetList(bool isFiltered, string klasa, string urbroj, IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, HttpRequest Request, IRacunElektraIzvrsenjeUslugeWorkshop racunElektraIzvrsenjeUslugeWorkshop, string Uid);
    }
}
