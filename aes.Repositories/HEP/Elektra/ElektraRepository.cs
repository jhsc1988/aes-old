using aes.Data;
using aes.Models.HEP;
using aes.Repositories.IRepository;
using aes.Repositories.IRepository.HEP;
using Microsoft.EntityFrameworkCore;

namespace aes.Repositories.HEP.Elektra
{
    public class ElektraRepository<T> : Repository<T>, IElektraRepository<T> where T : Models.Racuni.Elektra.Elektra
    {
        public ElektraRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<T>> GetRacuni(int predmetId, int dopisId) =>
            predmetId is 0 && dopisId is 0
                ? await Context.Set<T>()
                    .Include(e => e.ElektraKupac)
                    .Include(e => e.ElektraKupac.Ods)
                    .Include(e => e.ElektraKupac.Ods.Stan)
                    .Where(e => e.IsItTemp == null)
                    .ToListAsync()
                : dopisId is 0
                    ? await Context.Set<T>()
                                    .Include(e => e.ElektraKupac)
                                    .Include(e => e.ElektraKupac.Ods)
                                    .Include(e => e.ElektraKupac.Ods.Stan)
                                    .Include(e => e.Dopis)
                                    .Where(e => e.Dopis.PredmetId == predmetId)
                                    .ToListAsync()
                    : await Context.Set<T>()
                                    .Include(e => e.ElektraKupac)
                                    .Include(e => e.ElektraKupac.Ods)
                                    .Include(e => e.ElektraKupac.Ods.Stan)
                                    .Include(e => e.Dopis)
                                    .Where(e => e.DopisId == dopisId && e.Dopis.PredmetId == predmetId)
                                    .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetRacuniTemp(string userId) => await Find(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true);

        public async Task<ElektraKupac?> GetKupacByUgovorniRacun(long uRacun) =>
            await Context.ElektraKupac
                .Include(e => e.Ods)
                .Include(e => e.Ods.Stan)
                .FirstOrDefaultAsync(e => e.UgovorniRacun == uRacun);
        }
    }
}
