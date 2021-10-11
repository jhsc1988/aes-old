using aes.Data;
using aes.Models.Workshop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IPredmetWorkshop : IWorkshop
    {
        JsonResult SaveToDB(string klasa, string naziv, ApplicationDbContext _context);
        List<Predmet> GetPredmetiDataForFilter<T>(DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;
        Task<IActionResult> GetList(IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, HttpRequest Request);
    }
}
