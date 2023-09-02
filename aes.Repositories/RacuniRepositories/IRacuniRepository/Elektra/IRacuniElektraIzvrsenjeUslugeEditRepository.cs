using aes.Models.Racuni.Elektra;
using aes.Repository.IRepository;

namespace aes.Repository.RacuniRepositories.IRacuniRepository.Elektra;

public interface IRacuniElektraIzvrsenjeUslugeEditRepository : IRepository<RacunElektraIzvrsenjeUslugeEdit>
{
    Task<RacunElektraIzvrsenjeUslugeEdit> GetLastRacunElektraServiceEdit(string userId);
}