using aes.Models.Racuni.Elektra;
using aes.Repositories.IRepository;

namespace aes.Repositories.RacuniRepositories.IRacuniRepository.Elektra
{
    public interface IRacuniElektraRateEditRepository : IRepository<RacunElektraRateEdit>
    {
        Task<RacunElektraRateEdit?> GetLastRacunElektraRateEdit(string userId);
    }
}