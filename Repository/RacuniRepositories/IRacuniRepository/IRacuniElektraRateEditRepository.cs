using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories.IRacuniRepository
{
    public interface IRacuniElektraRateEditRepository : IRepository<RacunElektraRateEdit>
    {
        Task<RacunElektraRateEdit> GetLastRacunElektraRateEdit(string userId);
    }
}