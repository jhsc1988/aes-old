using aes.Data;
using aes.Models;
using aes.Models.Racuni;
using aes.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository
{
    public class ODSRepository : Repository<Ods>, IODSRepository
    {
        public ODSRepository(ApplicationDbContext context) : base(context) { }
        public async Task<IEnumerable<Ods>> GetAllOds()
        {
            return await _context.Ods
                .Include(e => e.Stan)
                .Where(e => e.Id != 5402) // HACK: ods id : dummy entity
                .ToListAsync();
        }
        public async Task<IEnumerable<TBill>> GetBillsForOmm<TBill>(int stanId) where TBill : Elektra
        {
            return await _context.Set<TBill>()
                .Include(e => e.ElektraKupac)
                .Include(e => e.ElektraKupac.Ods)
                .Include(e => e.ElektraKupac.Ods.Stan)
                .Where(e => e.ElektraKupac.Ods.StanId == stanId && e.IsItTemp != true)
                .ToListAsync();
        }

        public async Task<Ods> IncludeAppartment(Ods ods)
        {
            ods.Stan = await _context.Stan
                .FirstOrDefaultAsync(e => e.Id == ods.StanId);
            return ods;
        }
    }

}
