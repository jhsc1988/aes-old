using aes.Data;
using aes.Models.Workshop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aes.Models
{
    public interface IDopisWorkshop : IWorkshop
    {
        JsonResult GetList(IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, HttpRequest Request, int predmetId);
        JsonResult SaveToDB(string predmetId, string urbroj, string datumDopisa, ApplicationDbContext _context);
    }
}
