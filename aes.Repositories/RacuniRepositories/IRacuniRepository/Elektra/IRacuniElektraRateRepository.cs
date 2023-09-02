using aes.Models;
using aes.Models.Racuni.Elektra;
using aes.Repository.IRepository.HEP;

namespace aes.Repository.RacuniRepositories.IRacuniRepository.Elektra;

public interface IRacuniElektraRateRepository : IElektraRepository<RacunElektraRate>
{
    Task<IEnumerable<RacunElektraRate>> GetRacuniElektraRateWithDopisiAndPredmeti();
    Task<IEnumerable<Dopis>> GetDopisiForPayedRacuniElektraRate(int predmetId);
    Task<IEnumerable<RacunElektraRate>> GetRacuniForCustomer(int kupacId);
    Task<RacunElektraRate?> IncludeAll(int id);
    Task<IEnumerable<RacunElektraRate>> TempList(string userId);
}