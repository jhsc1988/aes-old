using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace aes.Models
{
    public interface IRacunElektraIzvrsenjeUslugeWorkshop : IRacunWorkshop
    {
        JsonResult AddNewTemp(string brojRacuna, string iznos, string datumPotvrde, string datumIzvrsenja, string usluga, string dopisId, string userId, ApplicationDbContext _context);
        List<RacunElektraIzvrsenjeUsluge> GetRacunElektraIzvrsenjeUslugeForDatatables(IDatatablesParams Params, List<RacunElektraIzvrsenjeUsluge> CreateRacuniElektraIzvrsenjeUslugeList);
    }
}
