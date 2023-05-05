using aes.Data;
using aes.Models.Racuni.Elektra;
using aes.Repositories.IRepository;
using aes.Repositories.RacuniRepositories.IRacuniRepository.Elektra;
using Microsoft.EntityFrameworkCore;

namespace aes.Repositories.RacuniRepositories.Elektra
{
    public class RacuniElektraRateEditRepository : Repository<RacunElektraRateEdit>, IRacuniElektraRateEditRepository
    {
        public RacuniElektraRateEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RacunElektraRateEdit?> GetLastRacunElektraRateEdit(string userId) =>
            await Context.RacunElektraRateEdit
                .Include(e => e.RacunElektraRate)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
    }
}
