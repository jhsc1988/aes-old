using aes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace aes.Models.Workshop
{
    public interface IWorkshop
    {
        JsonResult TrySave(ApplicationDbContext _context, bool delete);
    }
}
