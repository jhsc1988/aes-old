using aes.Data;
using aes.Models;
using aes.Models.Racuni;
using aes.Repository.RacuniRepositories.IRacuniRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories
{
    public class RacuniHoldingRepository : Repository<RacunHolding>, IRacuniHoldingRepository
    {
        public RacuniHoldingRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<RacunHolding>> GetRacuniHoldingWithDopisiAndPredmeti()
        {
            return await _context.RacunHolding
                .Include(e => e.Dopis)
                .Include(e => e.Dopis.Predmet)
                .Where(e => e.IsItTemp == null || e.IsItTemp == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<RacunHolding>> TempList(string userId)
        {
            return await _context.RacunHolding
                .Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true)
                .Include(r => r.Stan)
                .Include(r => r.Dopis)
                .Include(r => r.Dopis.Predmet)
                .ToListAsync();
        }

        public async Task<IEnumerable<RacunHolding>> GetRacuni(int predmetId, int dopisId)
        {
            return predmetId is 0 && dopisId is 0
                ? await _context.RacunHolding
                    .Include(e => e.Stan)
                    .Where(e => e.IsItTemp == null)
                    .ToListAsync()
                : dopisId is 0
                    ? await _context.RacunHolding
                                    .Include(e => e.Stan)
                                    .Include(e => e.Dopis)
                                    .Where(e => e.Dopis.PredmetId == predmetId)
                                    .ToListAsync()
                    : await _context.RacunHolding
                                    .Include(e => e.Stan)
                                    .Include(e => e.Dopis)
                                    .Where(e => e.DopisId == dopisId && e.Dopis.PredmetId == predmetId)
                                    .ToListAsync();
        }

        public async Task<Stan> GetStanBySifraObjekta(long SifraObjekta)
        {
            return await _context.Stan.FirstOrDefaultAsync(e => e.SifraObjekta == SifraObjekta);
        }

        public async Task<IEnumerable<Dopis>> GetDopisiForPayedRacuni(int predmetId)
        {
            // returns only Dopisi for payed Racuni
            return (await GetRacuniHoldingWithDopisiAndPredmeti())
                .Where(e => e.Dopis.PredmetId == predmetId)
                .Select(e => e.Dopis)
                .OrderByDescending(e => e.Datum)
                .Distinct();
        }

        public async Task<RacunHolding> IncludeAll(int id)
        {
            return await _context.RacunHolding
                .Include(r => r.Stan)
                .Include(r => r.Dopis)
                .Include(r => r.Dopis.Predmet)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Predmet>> GetPredmetiForCreate()
        {
            return await _context.Predmet
                .Where(e => e.Archived == false)
                .OrderByDescending(e => e.VrijemeUnosa)
                .ToListAsync();
        }

        public async Task<IEnumerable<RacunHolding>> GetRacuniForStan(int StanId)
        {
            return await _context.RacunHolding
                .Include(e => e.Stan)
                .Where(e => e.StanId == StanId && e.IsItTemp != true)
                .ToListAsync();
        }
    }
}
