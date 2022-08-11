using aes.Data;
using aes.Models.HEP;
using aes.Repository.IRepository.HEP;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository.HEP.Elektra
{
    public class ElektraKupacEditRepository : Repository<ElektraKupacEdit>, IElektraKupacEditRepository
    {
        public ElektraKupacEditRepository(ApplicationDbContext context) : base(context) { }

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
