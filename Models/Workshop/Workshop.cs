using aes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aes.Models.Workshop
{
    public class Workshop : IWorkshop
    {
        public JsonResult TrySave(ApplicationDbContext _context, bool delete) // false for save, true for delete
        {
            try
            {
                _ = _context.SaveChanges();
                return delete ? (new(new { success = true, Message = "Obrisano" })) : (new(new { success = true, Message = "Spremljeno" }));
            }
            catch (DbUpdateException)
            {
                return new(new { success = false, Message = "Greška" });
            }
        }
    }
}
