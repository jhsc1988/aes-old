using aes.Data;
using aes.Models.Workshop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace aes.Models
{
    public interface IRacunWorkshop : IWorkshop
    {
        string CheckIfExists(string brojRacuna, List<Racun> racunList);
        string CheckIfExistsInPayed(string brojRacuna, List<Racun> racunList);
        JsonResult RemoveRow<T>(string racunId, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;
        bool Validate(string brojRacuna, string iznos, string date, string dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja);
        JsonResult UpdateDbForInline<T>(string racunId, string updatedColumn, string x, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;
        JsonResult SaveToDb<T>(string userId, string _dopisId, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;
        JsonResult RemoveAllFromDbTemp<T>(string userId, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;
        List<T> GetRacuniFromDb<T>(DbSet<T> modelcontext, int param = 0) where T : Elektra;
        string GetUid(ClaimsPrincipal User);
        List<T> GetListCreateList<T>(string uid, ApplicationDbContext context, DbSet<T> modelcontext) where T : Elektra;
        JsonResult GetListMe<T>(bool isFiltered, string klasa, string urbroj, IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, IRacunWorkshop workshop, DbSet<T> modelcontext, HttpRequest Request, string Uid) where T : Elektra;
        List<T> GetList<T>(int predmetIdAsInt, int dopisIdAsInt, DbSet<T> modelcontext) where T : Elektra;

    }
}
