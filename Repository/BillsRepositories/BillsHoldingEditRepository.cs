using aes.Data;
using aes.Models.Racuni;
using aes.Repository.BillsRepositories.IBillsRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories
{
    public class BillsHoldingEditRepository : Repository<RacunHoldingEdit>, IBillsHoldingEditRepository
    {
        public BillsHoldingEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RacunHoldingEdit> GetLastBillHoldingEdit(string userId)
        {
            return await _context.RacunHoldingEdit
                .Include(e => e.RacunHolding)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
        }
    }
}
