using aes.Models.Racuni.Holding;
using aes.Repositories.IRepository;

namespace aes.Repositories.RacuniRepositories.IRacuniRepository
{
public interface IRacuniHoldingEditRepository : IRepository<RacunHoldingEdit>
{
    Task<RacunHoldingEdit?> GetLastRacunHoldingEdit(string userId);
}
}