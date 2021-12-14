using aes.Data;
using aes.Models;
using aes.Models.Racuni;
using aes.Repository.BillsRepositories.IBillsRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories
{
    public class BillsHoldingRepository : Repository<RacunHolding>, IBillsHoldingRepository
    {
        public BillsHoldingRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<RacunHolding>> GetBillsHoldingWithLettersAndCaseFiles()
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

        public async Task<IEnumerable<RacunHolding>> GetBills(int predmetId, int dopisId)
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

        public async Task<Stan> GetApartmentBySifraObjekta(long SifraObjekta)
        {
            return await _context.Stan.FirstOrDefaultAsync(e => e.SifraObjekta == SifraObjekta);
        }

        public async Task<IEnumerable<Dopis>> GetLettersForPayedBills(int predmetId)
        {
            // returns only letters for payed bills
            return (await GetBillsHoldingWithLettersAndCaseFiles())
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

        public async Task<IEnumerable<Predmet>> GetCaseFilesForCreate()
        {
            return await _context.Predmet
                .Where(e => e.Archived == false)
                .OrderByDescending(e => e.VrijemeUnosa)
                .ToListAsync();
        }

        public async Task<IEnumerable<RacunHolding>> GetBillsForApartment(int apartmentId)
        {
            return await _context.RacunHolding
                .Include(e => e.Stan)
                .Where(e => e.StanId == apartmentId && e.IsItTemp != true)
                .ToListAsync();
        }
    }
}
