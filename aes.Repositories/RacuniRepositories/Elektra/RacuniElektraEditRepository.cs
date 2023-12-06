using aes.Data;
using aes.Models.Racuni.Elektra;
using aes.Repositories.IRepository;
using aes.Repositories.RacuniRepositories.IRacuniRepository.Elektra;
using Microsoft.EntityFrameworkCore;

namespace aes.Repositories.RacuniRepositories.Elektra
{
public class RacuniElektraEditRepository : Repository<RacunElektraEdit>, IRacuniElektraEditRepository
{
    public RacuniElektraEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RacunElektraEdit?> GetLastRacunElektraEdit(string userId) =>
            await Context.RacunElektraEdit
            .Include(e => e.RacunElektra)
            .Where(e => e.EditingByUserId == userId)
            .OrderByDescending(e => e.EditTime)
            .FirstOrDefaultAsync();
    }
}