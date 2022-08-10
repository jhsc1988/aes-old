using aes.Data;
using aes.Models.Racuni;
using aes.Repository.RacuniRepositories.IRacuniRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories
{
    public class RacuniElektraRateEditRepository : Repository<RacunElektraRateEdit>, IRacuniElektraRateEditRepository
    {
        public RacuniElektraRateEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RacunElektraRateEdit> GetLastRacunElektraRateEdit(string userId)
        {
            return await _context.RacunElektraRateEdit
                .Include(e => e.RacunElektraRate)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
        }
    }
}
