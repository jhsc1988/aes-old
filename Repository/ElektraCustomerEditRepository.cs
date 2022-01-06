using aes.Data;
using aes.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository
{
    public class ElektraCustomerEditRepository : Repository<ElektraKupacEdit>, IElektraCustomerEditRepository
    {
        public ElektraCustomerEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ElektraKupacEdit> GetLastElektraKupacEdit(string userId)
        {
            return await _context.ElektraKupacEdit
                .Include(e => e.ElektraKupac)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
        }
    }
}
