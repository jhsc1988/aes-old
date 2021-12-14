using aes.Data;
using aes.Models;
using aes.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository
{
    public class ElektraCustomerRepository : Repository<ElektraKupac>, IElektraCustomerRepository
    {
        public ElektraCustomerRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ElektraKupac>> GetAllCustomers()
        {
            return await _context.ElektraKupac
                .Include(e => e.Ods)
                .Include(e => e.Ods.Stan)
                .Where(e => e.Id != 2002)
                .ToListAsync();
        }

        public async Task<ElektraKupac> IncludeOdsAndApartment(ElektraKupac elektraKupac)
        {
            elektraKupac.Ods = await _context.Ods.FirstOrDefaultAsync(e => e.Id == elektraKupac.OdsId);
            elektraKupac.Ods.Stan = await _context.Stan.FirstOrDefaultAsync(e => e.Id == elektraKupac.Ods.StanId);
            return elektraKupac;
        }

        public async Task<ElektraKupac> FindExact(long ugovorniRacun)
        {
            return await _context.ElektraKupac
                .Where(e => e.UgovorniRacun == ugovorniRacun)
                .FirstOrDefaultAsync();
        }

        public async Task<ElektraKupac> FindExactById(int id)
        {
            return await _context.ElektraKupac
                .Include(e => e.Ods)
                .Include(e => e.Ods.Stan)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

    }
}
