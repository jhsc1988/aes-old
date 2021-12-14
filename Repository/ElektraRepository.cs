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
    public class ElektraRepository<T> : Repository<T>, IElektraRepository<T> where T : Elektra
    {
        public ElektraRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<T>> GetRacuni(int predmetId, int dopisId)
        {
            return predmetId is 0 && dopisId is 0
                ? await _context.Set<T>()
                    .Include(e => e.ElektraKupac)
                    .Include(e => e.ElektraKupac.Ods)
                    .Include(e => e.ElektraKupac.Ods.Stan)
                    .Where(e => e.IsItTemp == null)
                    .ToListAsync()
                : dopisId is 0
                    ? await _context.Set<T>()
                                    .Include(e => e.ElektraKupac)
                                    .Include(e => e.ElektraKupac.Ods)
                                    .Include(e => e.ElektraKupac.Ods.Stan)
                                    .Include(e => e.Dopis)
                                    .Where(e => e.Dopis.PredmetId == predmetId)
                                    .ToListAsync()
                    : await _context.Set<T>()
                                    .Include(e => e.ElektraKupac)
                                    .Include(e => e.ElektraKupac.Ods)
                                    .Include(e => e.ElektraKupac.Ods.Stan)
                                    .Include(e => e.Dopis)
                                    .Where(e => e.DopisId == dopisId && e.Dopis.PredmetId == predmetId)
                                    .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetRacuniTemp(string userId)
        {
            return await Find(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true);
        }

        public async Task<ElektraKupac> GetKupacByUgovorniRacun(long URacun)
        {
            return await _context.ElektraKupac
                .Include(e => e.Ods)
                .Include(e => e.Ods.Stan)
                .FirstOrDefaultAsync(e => e.UgovorniRacun == URacun);
        }
    }
}
