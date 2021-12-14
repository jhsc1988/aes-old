using aes.Data;
using aes.Models.Racuni;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories
{
    public class BillsElektraAdvancesEditRepository : Repository<RacunElektraRateEdit>, IBillsElektraAdvancesEditRepository
    {
        public BillsElektraAdvancesEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RacunElektraRateEdit> GetLastBillElektraAdvancesEdit(string userId)
        {
            return await _context.RacunElektraRateEdit
                .Include(e => e.RacunElektraRate)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
        }
    }
}
