using aes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace aes.Models.Workshop
{
    public interface IWorkshop
    {

        //List<T> GetListCreateList<T>(string userId, ApplicationDbContext _context, DbSet<T> modelcontext) where T : Elektra;
        JsonResult TrySave(ApplicationDbContext _context, bool delete);
        //List<Racun> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext context);
        //List<T> GetListCreateList<T>(string uid, ApplicationDbContext context, DbSet<T> modelcontext) where T : Racun;
    }
}
