using aes.Data;
using aes.Models.Racuni.Holding;
using aes.Repositories.IRepository;
using aes.Repositories.RacuniRepositories.IRacuniRepository;
using Microsoft.EntityFrameworkCore;

namespace aes.Repositories.RacuniRepositories
{
public class RacuniHoldingEditRepository : Repository<RacunHoldingEdit>, IRacuniHoldingEditRepository
{
    public RacuniHoldingEditRepository(ApplicationDbContext context) : base(context) { }

    public async Task<RacunHoldingEdit?> GetLastRacunHoldingEdit(string userId)
    {
        return await Context.RacunHoldingEdit
            .Include(e => e.RacunHolding)
            .Where(e => e.EditingByUserId == userId)
            .OrderByDescending(e => e.EditTime)
            .FirstOrDefaultAsync();
    }
}
}
