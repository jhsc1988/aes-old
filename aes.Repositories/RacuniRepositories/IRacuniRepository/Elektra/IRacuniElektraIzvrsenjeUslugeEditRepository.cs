using aes.Models.Racuni.Elektra;
using aes.Repositories.IRepository;

namespace aes.Repositories.RacuniRepositories.IRacuniRepository.Elektra
{
    public interface IRacuniElektraIzvrsenjeUslugeEditRepository : IRepository<RacunElektraIzvrsenjeUslugeEdit>
    {
        Task<RacunElektraIzvrsenjeUslugeEdit?> GetLastRacunElektraServiceEdit(string userId);
    }
}