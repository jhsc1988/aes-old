using aes.Models.Racuni.Elektra;
using aes.Repository.IRepository;

namespace aes.Repository.RacuniRepositories.IRacuniRepository.Elektra;

public interface IRacuniElektraRateEditRepository : IRepository<RacunElektraRateEdit>
{
    Task<RacunElektraRateEdit> GetLastRacunElektraRateEdit(string userId);
}