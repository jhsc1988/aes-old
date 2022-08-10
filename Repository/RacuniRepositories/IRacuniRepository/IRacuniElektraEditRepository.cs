using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories.IRacuniRepository
{
    public interface IRacuniElektraEditRepository : IRepository<RacunElektraEdit>
    {
        Task<RacunElektraEdit> GetLastRacunElektraEdit(string userId);
    }
}