using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models.RacuniWorkshop.IRacuniWorkshop
{
    public interface IRacuniElektraIRateWorkshop : IRacunWorkshop
    {
        List<T> GetList<T>(int predmetIdAsInt, int dopisIdAsInt, DbSet<T> modelcontext) where T : Elektra;
        List<T> GetListCreateList<T>(string userId, ApplicationDbContext _context, DbSet<T> modelcontext) where T : Elektra;
        JsonResult GetListMe<T>(bool isFiltered, string klasa, string urbroj, IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, DbSet<T> modelcontext, HttpRequest Request, string Uid) where T : Elektra;
        List<T> GetRacuniFromDb<T>(DbSet<T> modelcontext, int param = 0) where T : Elektra;
    }
}
