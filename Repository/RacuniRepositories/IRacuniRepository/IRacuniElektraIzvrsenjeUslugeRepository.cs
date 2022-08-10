using aes.Models;
using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories.IRacuniRepository
{
    public interface IRacuniElektraIzvrsenjeUslugeRepository : IElektraRepository<RacunElektraIzvrsenjeUsluge>
    {
        Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetRacuniElektraIzvrsenjeUslugeWithDopisiAndPredmeti();
        Task<IEnumerable<Dopis>> GetDopisiForPayedRacuniElektraIzvrsenjeUsluge(int predmetId);
        Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetRacuniForCustomer(int kupacId);
        Task<RacunElektraIzvrsenjeUsluge> IncludeAll(int id);
        Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> TempList(string userId);
    }
}
