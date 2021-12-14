using aes.Data;
using aes.Models.Racuni;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository
{
    public class ObracunPotrosnjeRepository : Repository<ObracunPotrosnje>, IObracunPotrosnjeRepository
    {
        public ObracunPotrosnjeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ObracunPotrosnje>> GetObracunPotrosnjeForBill(int billId)
        {
            return await _context.ObracunPotrosnje.
                Where(e => e.RacunElektraId == billId)
                .Include(e => e.TarifnaStavka)
                .ToListAsync();
        }
    }
}
