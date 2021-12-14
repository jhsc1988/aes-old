using aes.Models;
using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories.IBillsRepository
{
    public interface IBillsElektraAdvancesRepository : IElektraRepository<RacunElektraRate>
    {
        Task<IEnumerable<RacunElektraRate>> GetBillsElektraAdvancesWithLettersAndCaseFiles();
        Task<IEnumerable<Dopis>> GetLettersForPayedBillsElektraAdvances(int predmetId);
        Task<IEnumerable<RacunElektraRate>> GetRacuniForCustomer(int kupacId);
        Task<RacunElektraRate> IncludeAll(int id);
        Task<IEnumerable<RacunElektraRate>> TempList(string userId);
    }
}
