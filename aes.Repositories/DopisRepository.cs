using aes.Data;
using aes.Models;
using aes.Models.Racuni;
using aes.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace aes.Repositories
{
    public class DopisRepository : Repository<Dopis>, IDopisRepository
    {
        public DopisRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Dopis>> GetOnlyEmptyDopisi(int predmetId)
        {
            IEnumerable<Dopis> dopisi = await Find(e => e.PredmetId == predmetId);

            List<Racun> sviRacuni = new();

            sviRacuni.AddRange(Context.RacunElektra.Include(e => e.Dopis));
            sviRacuni.AddRange(Context.RacunElektraRate.Include(e => e.Dopis));
            sviRacuni.AddRange(Context.RacunElektraIzvrsenjeUsluge.Include(e => e.Dopis));
            sviRacuni.AddRange(Context.RacunHolding.Include(e => e.Dopis));

            IEnumerable<Dopis> dopisiZaPlaceneRacune = sviRacuni.Select(e => e.Dopis).Distinct();

            // returns only empty Dopisi
            return dopisi.Except(dopisiZaPlaceneRacune).OrderByDescending(e => e.Datum);
        }

        public async Task<IEnumerable<Dopis>> GetDopisiForPredmet(int predmetId)
        {
            return await Context.Dopis
                .Include(e => e.Predmet)
                .Where(e => e.PredmetId == predmetId)
                .OrderBy(e => e.Datum)
                .ToListAsync();
        }

        public async Task<Dopis> IncludePredmet(Dopis dopis)
        {
            dopis.Predmet = await Context.Predmet.FirstOrDefaultAsync(e => e.Id == dopis.PredmetId);
            return dopis;
        }
    }
}