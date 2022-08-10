using aes.Models;
using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories.IRacuniRepository
{
    public interface IRacuniHoldingRepository : IRepository<RacunHolding>
    {
        Task<Stan> GetStanBySifraObjekta(long SifraObjekta);
        Task<IEnumerable<RacunHolding>> GetRacuni(int predmetId, int dopisId);
        Task<IEnumerable<RacunHolding>> GetRacuniForStan(int StanId);
        Task<IEnumerable<RacunHolding>> GetRacuniHoldingWithDopisiAndPredmeti();
        Task<IEnumerable<Predmet>> GetPredmetiForCreate();
        Task<IEnumerable<Dopis>> GetDopisiForPayedRacuni(int predmetId);
        Task<IEnumerable<RacunHolding>> TempList(string userId);
        Task<RacunHolding> IncludeAll(int id);
    }
}