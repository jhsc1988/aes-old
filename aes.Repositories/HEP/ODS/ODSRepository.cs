using aes.Data;
using aes.Models.HEP;
using aes.Repository.IRepository.HEP;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository.HEP.ODS
{
    public class ODSRepository : Repository<Ods>, IODSRepository
    {
        public ODSRepository(ApplicationDbContext context) : base(context) { }
        public async Task<IEnumerable<Ods>> GetAllOds()
        {
            return await _context.Ods
                .Include(e => e.Stan)
                .Where(e => e.Id != 5402) // HACK: dummy entity
                .ToListAsync();
        }
        public async Task<IEnumerable<TRacun>> GetRacuniForOmm<TRacun>(int stanId) where TRacun : Models.Racuni.Elektra.Elektra
        {
            return await _context.Set<TRacun>()
                .Include(e => e.ElektraKupac)
                .Include(e => e.ElektraKupac.Ods)
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
