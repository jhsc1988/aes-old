using aes.Data;
using aes.Models.HEP;
using aes.Repositories.IRepository;
using aes.Repositories.IRepository.HEP;
using Microsoft.EntityFrameworkCore;

namespace aes.Repositories.HEP.Elektra
{
    public class ElektraKupacEditRepository : Repository<ElektraKupacEdit>, IElektraKupacEditRepository
    {
        public ElektraKupacEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ElektraKupacEdit?> GetLastElektraKupacEdit(string userId) =>
            await Context.ElektraKupacEdit
                .Include(e => e.ElektraKupac)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
    }
}
