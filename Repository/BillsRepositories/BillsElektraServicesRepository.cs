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
    public class BillsElektraServicesRepository : ElektraRepository<RacunElektraIzvrsenjeUsluge>, IBillsElektraServicesRepository
    {
        public BillsElektraServicesRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetBillsElektraServicesWithLettersAndCaseFiles()
        {
            return await _context.RacunElektraIzvrsenjeUsluge
                .Include(e => e.Dopis)
                .Include(e => e.Dopis.Predmet)
                .Where(e => e.IsItTemp == null || e.IsItTemp == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Dopis>> GetLettersForPayedBillsElektraServices(int predmetId)
        {
            // returns only letters for payed bills
            return (await GetBillsElektraServicesWithLettersAndCaseFiles())
                .Where(e => e.Dopis.PredmetId == predmetId)
                .Select(e => e.Dopis)
                .OrderByDescending(e => e.Datum)
                .Distinct();
        }

        public async Task<RacunElektraIzvrsenjeUsluge> IncludeAll(int id)
        {
            return await _context.RacunElektraIzvrsenjeUsluge
                .Include(r => r.ElektraKupac)
                .Include(r => r.ElektraKupac.Ods)
                .Include(r => r.ElektraKupac.Ods.Stan)
                .Include(r => r.Dopis)
                .Include(r => r.Dopis.Predmet)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> TempList(string userId)
        {
            return await _context.RacunElektraIzvrsenjeUsluge
                .Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true)
                .Include(r => r.ElektraKupac)
                .Include(r => r.ElektraKupac.Ods)
                .Include(r => r.ElektraKupac.Ods.Stan)
                .Include(r => r.Dopis)
                .Include(r => r.Dopis.Predmet)
                .ToListAsync();
        }

        public async Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetRacuniForCustomer(int kupacId)
        {
            return await _context.RacunElektraIzvrsenjeUsluge
                .Include(e => e.ElektraKupac)
                .Where(e => e.ElektraKupacId == kupacId && (e.IsItTemp == null || false))
                .ToListAsync();
        }
    }
}
