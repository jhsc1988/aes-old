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
    public class DopisRepository : Repository<Dopis>, IDopisRepository
    {
        public DopisRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Dopis>> GetOnlyEmptyDopisi(int predmetId)
        {
            IEnumerable<Dopis> Dopisi = await Find(e => e.PredmetId == predmetId);

            List<Racun> AllRacuni = new();

            AllRacuni.AddRange(_context.RacunElektra.Include(e => e.Dopis));
            AllRacuni.AddRange(_context.RacunElektraRate.Include(e => e.Dopis));
            AllRacuni.AddRange(_context.RacunElektraIzvrsenjeUsluge.Include(e => e.Dopis));
            AllRacuni.AddRange(_context.RacunHolding.Include(e => e.Dopis));

            IEnumerable<Dopis> DopisiForPayedRacuni = AllRacuni.Select(e => e.Dopis).Distinct();

            // returns only empty Dopisi
            return Dopisi.Except(DopisiForPayedRacuni).OrderByDescending(e => e.Datum);
        }

        public async Task<IEnumerable<Dopis>> GetDopisiForPredmet(int predmetId)
        {
            return await _context.Dopis
                .Include(e => e.Predmet)
                .Where(e => e.PredmetId == predmetId)
                .OrderBy(e => e.Datum)
                .ToListAsync();
        }

        public async Task<Dopis> IncludePredmet(Dopis Dopis)
        {
            Dopis.Predmet = await _context.Predmet.FirstOrDefaultAsync(e => e.Id == Dopis.PredmetId);
            return Dopis;
        }
    }
}