using aes.Models.Racuni.Elektra;
using aes.Repository.IRepository;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories.IRacuniRepository.Elektra
{
    public interface IRacuniElektraEditRepository : IRepository<RacunElektraEdit>
    {
        Task<RacunElektraEdit> GetLastRacunElektraEdit(string userId);
    }
}