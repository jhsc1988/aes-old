using aes.Models;
using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories.IBillsRepository
{
    public interface IBillsElektraRepository : IElektraRepository<RacunElektra>
    {
        Task<IEnumerable<RacunElektra>> TempList(string userId);
        Task<IEnumerable<RacunElektra>> GetBillsElektraWithLettersAndCaseFiles();
        Task<IEnumerable<Predmet>> GetCaseFilesForCreate();
        Task<IEnumerable<Dopis>> GetLettersForPayedBillsElektra(int predmetId);
        Task<RacunElektra> IncludeAll(int id);
        Task<IEnumerable<RacunElektra>> GetRacuniForCustomer(int kupacId);
    }
}
