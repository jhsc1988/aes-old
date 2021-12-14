using aes.Models;
using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories.IBillsRepository
{
    public interface IBillsElektraServicesRepository : IElektraRepository<RacunElektraIzvrsenjeUsluge>
    {
        Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetBillsElektraServicesWithLettersAndCaseFiles();
        Task<IEnumerable<Dopis>> GetLettersForPayedBillsElektraServices(int predmetId);
        Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetRacuniForCustomer(int kupacId);
        Task<RacunElektraIzvrsenjeUsluge> IncludeAll(int id);
        Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> TempList(string userId);
    }
}
