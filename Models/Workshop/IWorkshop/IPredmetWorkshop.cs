using aes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IPredmetWorkshop
    {
        JsonResult SaveToDB(string klasa, string naziv, ApplicationDbContext _context);
        JsonResult TrySave(ApplicationDbContext _context);
        List<Predmet> GetPredmetiForDatatables(DatatablesParams Params, List<Predmet> PredmetiList);
        List<Predmet> GetPredmetiDataForFilter<T>(DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;
    }
}
