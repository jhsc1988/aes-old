using aes.Models.Racuni.Elektra;
using aes.Repository.IRepository;

namespace aes.Repository.RacuniRepositories.IRacuniRepository.Elektra;

public interface IRacuniElektraEditRepository : IRepository<RacunElektraEdit>
{
    Task<RacunElektraEdit> GetLastRacunElektraEdit(string userId);
}