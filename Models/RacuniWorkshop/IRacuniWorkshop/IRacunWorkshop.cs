using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IRacunWorkshop
    {
        string CheckIfExists(string brojRacuna, List<Racun> racunList);
        string CheckIfExistsInPayed(string brojRacuna, List<Racun> racunList);
        JsonResult RemoveRow<T>(string racunId, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;
        bool Validate(string brojRacuna, string iznos, string date, string dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja);
        JsonResult UpdateDbForInline<T>(string racunId, string updatedColumn, string x, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;
        JsonResult SaveToDb<T>(string userId, string _dopisId, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;
        JsonResult TrySave(ApplicationDbContext context);
        JsonResult TryDelete(ApplicationDbContext _context);
        JsonResult RemoveAllFromDb<T>(string userId, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;
        List<T> GetRacuniFromDb<T>(DbSet<T> modelcontext, int param = 0) where T : Elektra;
        ElektraKupac GetKupacForStanId<T>(DbSet<T> modelcontext, int param) where T : ElektraKupac;
    }
}
