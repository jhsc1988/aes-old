using aes.Data;
using aes.Models;
using aes.Models.Racuni.Elektra;
using aes.Repositories.HEP.Elektra;
using aes.Repositories.RacuniRepositories.IRacuniRepository.Elektra;
using Microsoft.EntityFrameworkCore;

namespace aes.Repositories.RacuniRepositories.Elektra
{
    public class RacuniElektraRateRepository : ElektraRepository<RacunElektraRate>, IRacuniElektraRateRepository
    {
        public RacuniElektraRateRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<RacunElektraRate>> GetRacuniElektraRateWithDopisiAndPredmeti()
        {
            return await Context.RacunElektraRate
                .Include(e => e.Dopis)
                .Include(e => e.Dopis.Predmet)
                .Where(e => e.IsItTemp == null || e.IsItTemp == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Dopis>> GetDopisiForPayedRacuniElektraRate(int predmetId)
        {
            // returns only Dopisi for payed Racuni
            return (await GetRacuniElektraRateWithDopisiAndPredmeti())
                .Where(e => e.Dopis.PredmetId == predmetId)
                .Select(e => e.Dopis)
                .OrderByDescending(e => e.Datum)
                .Distinct();
        }

        public async Task<RacunElektraRate?> IncludeAll(int id) =>
            await Context.RacunElektraRate
                .Include(r => r.ElektraKupac)
                .Include(r => r.ElektraKupac.Ods)
                .Include(r => r.ElektraKupac.Ods.Stan)
                .Include(r => r.Dopis)
                .Include(r => r.Dopis.Predmet)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<IEnumerable<RacunElektraRate>> TempList(string userId)
        {
            return await Context.RacunElektraRate
                .Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true)
                .Include(r => r.ElektraKupac)
                .Include(r => r.ElektraKupac.Ods)
                .Include(r => r.ElektraKupac.Ods.Stan)
                .Include(r => r.Dopis)
                .Include(r => r.Dopis.Predmet)
                .ToListAsync();
        }

        public async Task<IEnumerable<RacunElektraRate>> GetRacuniForCustomer(int kupacId)
        {
            return await Context.RacunElektraRate
                .Include(e => e.ElektraKupac)
                .Where(e => e.ElektraKupacId == kupacId && (e.IsItTemp == null))
                .ToListAsync();
        }
    }
}
