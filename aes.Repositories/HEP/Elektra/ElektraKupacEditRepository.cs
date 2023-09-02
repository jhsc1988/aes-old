using aes.Data;
using aes.Models.HEP;
using aes.Repository.IRepository.HEP;
using Microsoft.EntityFrameworkCore;

namespace aes.Repository.HEP.Elektra
{
    public class ElektraKupacEditRepository : Repository<ElektraKupacEdit>, IElektraKupacEditRepository
    {
        public ElektraKupacEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ElektraKupacEdit> GetLastElektraKupacEdit(string userId)
        {
            return await Context.ElektraKupacEdit
                .Include(e => e.ElektraKupac)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
        }
    }
}
