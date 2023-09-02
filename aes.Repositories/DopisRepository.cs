using aes.Data;
using aes.Models;
using aes.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace aes.Repository
{
    public class DopisRepository : Repository<Dopis>, IDopisRepository
    {
        public DopisRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Dopis>> GetOnlyEmptyDopisiAsync(int predmetId)
        {
            IEnumerable<Dopis> dopisi = await Find(e => e.PredmetId == predmetId);

            var racuniElektraDopisIds = await Context.RacunElektra.Select(e => e.DopisId).Where(id => id.HasValue).Select(id => id.Value).ToListAsync();
            var racuniElektraRateDopisIds = await Context.RacunElektraRate.Select(e => e.DopisId).Where(id => id.HasValue).Select(id => id.Value).ToListAsync();
            var racuniElektraIzvrsenjeUslugeDopisIds = await Context.RacunElektraIzvrsenjeUsluge.Select(e => e.DopisId).Where(id => id.HasValue).Select(id => id.Value).ToListAsync();
            var racuniHoldingDopisIds = await Context.RacunHolding.Select(e => e.DopisId).Where(id => id.HasValue).Select(id => id.Value).ToListAsync();

            var allRacuniDopisIds = new HashSet<int>(racuniElektraDopisIds
                .Concat(racuniElektraRateDopisIds)
                .Concat(racuniElektraIzvrsenjeUslugeDopisIds)
                .Concat(racuniHoldingDopisIds));

            return dopisi.Where(d => !allRacuniDopisIds.Contains(d.Id))
                .OrderByDescending(d => d.Datum);
        }

        public async Task<IEnumerable<Dopis>> GetDopisiForPredmet(int predmetId)
        {
            return await Context.Dopis
                .Include(e => e.Predmet)
                .Where(e => e.PredmetId == predmetId)
                .OrderBy(e => e.Datum)
                .ToListAsync();
        }

        public async Task<Dopis> IncludePredmetAsync(Dopis dopis)
        {
            dopis.Predmet = await Context.Predmet.FirstOrDefaultAsync(e => e.Id == dopis.PredmetId);
            return dopis;
        }
    }
}