using aes.Models.Racuni.Holding;
using aes.Repository.IRepository;

namespace aes.Repository.RacuniRepositories.IRacuniRepository;

public interface IRacuniHoldingEditRepository : IRepository<RacunHoldingEdit>
{
    Task<RacunHoldingEdit?> GetLastRacunHoldingEdit(string userId);
}