using aes.Models;
using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories.IBillsRepository
{
    public interface IBillsHoldingRepository : IRepository<RacunHolding>
    {
        Task<Stan> GetApartmentBySifraObjekta(long SifraObjekta);
        Task<IEnumerable<RacunHolding>> GetBills(int predmetId, int dopisId);
        Task<IEnumerable<RacunHolding>> GetBillsForApartment(int apartmentId);
        Task<IEnumerable<RacunHolding>> GetBillsHoldingWithLettersAndCaseFiles();
        Task<IEnumerable<Predmet>> GetCaseFilesForCreate();
        Task<IEnumerable<Dopis>> GetLettersForPayedBills(int predmetId);
        Task<IEnumerable<RacunHolding>> TempList(string userId);
        Task<RacunHolding> IncludeAll(int id);
    }
}