using aes.Models;
using aes.Models.Racuni.Elektra;
using aes.Repository.IRepository.HEP;

namespace aes.Repository.RacuniRepositories.IRacuniRepository.Elektra;

public interface IRacuniElektraIzvrsenjeUslugeRepository : IElektraRepository<RacunElektraIzvrsenjeUsluge>
{
    Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetRacuniElektraIzvrsenjeUslugeWithDopisiAndPredmeti();
    Task<IEnumerable<Dopis>> GetDopisiForPayedRacuniElektraIzvrsenjeUsluge(int predmetId);
    Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetRacuniForCustomer(int kupacId);
    Task<RacunElektraIzvrsenjeUsluge> IncludeAll(int id);
    Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> TempList(string userId);
}