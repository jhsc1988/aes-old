using aes.Models.Racuni.Elektra;
using aes.Repositories.IRepository;

namespace aes.Repositories.RacuniRepositories.IRacuniRepository.Elektra
{
public interface IRacuniElektraEditRepository : IRepository<RacunElektraEdit>
{
    Task<RacunElektraEdit?> GetLastRacunElektraEdit(string userId);
}
}