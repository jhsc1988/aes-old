using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories.IRacuniRepository
{
    public interface IRacuniElektraIzvrsenjeUslugeEditRepository : IRepository<RacunElektraIzvrsenjeUslugeEdit>
    {
        Task<RacunElektraIzvrsenjeUslugeEdit> GetLastRacunElektraServiceEdit(string userId);
    }
}