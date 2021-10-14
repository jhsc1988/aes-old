using aes.Data;
using aes.Models.RacuniWorkshop.IRacuniWorkshop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IElektraKupacWorkshop
    {
        Task<IActionResult> GetList(IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, HttpRequest Request);
        ElektraKupac GetElektraKupacForStanId<T>(DbSet<T> modelcontext, int param) where T : ElektraKupac;
        JsonResult GetRacuniForKupac<T>(int param, IDatatablesGenerator datatablesGenerator, HttpRequest Request, ApplicationDbContext _context, IRacuniElektraIRateWorkshop workshop, DbSet<T> modelcontext) where T : Elektra;
    }
}
