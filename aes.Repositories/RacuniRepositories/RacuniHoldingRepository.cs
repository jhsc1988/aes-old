using aes.Data;
using aes.Models;
using aes.Models.Racuni.Holding;
using aes.Repositories.IRepository;
using aes.Repositories.RacuniRepositories.IRacuniRepository;
using Microsoft.EntityFrameworkCore;

namespace aes.Repositories.RacuniRepositories
{
public class RacuniHoldingRepository : Repository<RacunHolding>, IRacuniHoldingRepository
{
        public RacuniHoldingRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<RacunHolding>> GetRacuniHoldingWithDopisiAndPredmeti()
    {
        return await Context.RacunHolding
            .Include(e => e.Dopis)
            .Include(e => e.Dopis.Predmet)
            .Where(e => e.IsItTemp == null || e.IsItTemp == false)
            .ToListAsync();
    }

    public async Task<IEnumerable<RacunHolding>> TempList(string userId)
    {
        return await Context.RacunHolding
            .Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true)
            .Include(r => r.Stan)
            .Include(r => r.Dopis)
            .Include(r => r.Dopis.Predmet)
            .ToListAsync();
    }

        public async Task<IEnumerable<RacunHolding>> GetRacuni(int predmetId, int dopisId) =>
            predmetId is 0 && dopisId is 0
            ? await Context.RacunHolding
                .Include(e => e.Stan)
                .Where(e => e.IsItTemp == null)
                .ToListAsync()
            : dopisId is 0
                ? await Context.RacunHolding
                    .Include(e => e.Stan)
                    .Include(e => e.Dopis)
                    .Where(e => e.Dopis.PredmetId == predmetId)
                    .ToListAsync()
                : await Context.RacunHolding
                    .Include(e => e.Stan)
                    .Include(e => e.Dopis)
                    .Where(e => e.DopisId == dopisId && e.Dopis.PredmetId == predmetId)
                    .ToListAsync();
    }

        public async Task<Models.Stan?> GetStanBySifraObjekta(long sifraObjekta) => await Context.Stan.FirstOrDefaultAsync(e => e.SifraObjekta == sifraObjekta);

        public async Task<IEnumerable<Dopis>> GetDopisiForPayedRacuni(int predmetId) =>
        // returns only Dopisi for payed Racuni
            (await GetRacuniHoldingWithDopisiAndPredmeti())
            .Where(e => e.Dopis.PredmetId == predmetId)
            .Select(e => e.Dopis)
            .OrderByDescending(e => e.Datum)
            .Distinct();
    }

        public async Task<RacunHolding?> IncludeAll(int id) =>
            await Context.RacunHolding
            .Include(r => r.Stan)
            .Include(r => r.Dopis)
            .Include(r => r.Dopis.Predmet)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

        public async Task<IEnumerable<Predmet>> GetPredmetiForCreate() =>
            await Context.Predmet
            .Where(e => e.Archived == false)
            .OrderByDescending(e => e.VrijemeUnosa)
            .ToListAsync();
    }

        public async Task<IEnumerable<RacunHolding>> GetRacuniForStan(int stanId) =>
            await Context.RacunHolding
            .Include(e => e.Stan)
            .Where(e => e.StanId == stanId && e.IsItTemp != true)
            .ToListAsync();
    }
}