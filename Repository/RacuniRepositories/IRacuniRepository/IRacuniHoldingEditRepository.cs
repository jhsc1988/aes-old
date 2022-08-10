using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories.IRacuniRepository
{
    public interface IRacuniHoldingEditRepository : IRepository<RacunHoldingEdit>
    {
        Task<RacunHoldingEdit> GetLastRacunHoldingEdit(string userId);
    }
}