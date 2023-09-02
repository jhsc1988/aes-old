using aes.Data;
using aes.Models.HEP;
using aes.Repository.IRepository.HEP;
using Microsoft.EntityFrameworkCore;

namespace aes.Repository.HEP.ODS
{
    public class OdsRepository : Repository<Ods>, IOdsRepository
    {
        public OdsRepository(ApplicationDbContext context) : base(context) { }
        public async Task<IEnumerable<Ods>> GetAllOds()
        {
            return await Context.Ods
                .Include(e => e.Stan)
                .Where(e => e.Id != 5402) // HACK: dummy entity
                .ToListAsync();
        }
        public async Task<IEnumerable<TRacun>> GetRacuniForOmm<TRacun>(int stanId) where TRacun : Models.Racuni.Elektra.Elektra
        {
            return await Context.Set<TRacun>()
                .Include(e => e.ElektraKupac)
                .Include(e => e.ElektraKupac.Ods)
                .Where(e => e.ElektraKupac.Ods.StanId == stanId && e.IsItTemp != true)
                .ToListAsync();
        }

        public async Task<Ods> IncludeApartment(Ods ods)
        {
            ods.Stan = await Context.Stan
                .FirstOrDefaultAsync(e => e.Id == ods.StanId);
            return ods;
        }
    }
}
