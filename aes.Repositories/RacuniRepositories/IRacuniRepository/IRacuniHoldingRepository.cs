using aes.Models;
using aes.Models.Racuni.Holding;
using aes.Repositories.IRepository;

namespace aes.Repositories.RacuniRepositories.IRacuniRepository
{
public interface IRacuniHoldingRepository : IRepository<RacunHolding>
{
    Task<Models.Stan?> GetStanBySifraObjekta(long sifraObjekta);
    Task<IEnumerable<RacunHolding>> GetRacuni(int predmetId, int dopisId);
    Task<IEnumerable<RacunHolding>> GetRacuniForStan(int stanId);
    Task<IEnumerable<RacunHolding>> GetRacuniHoldingWithDopisiAndPredmeti();
    Task<IEnumerable<Predmet>> GetPredmetiForCreate();
    Task<IEnumerable<Dopis>> GetDopisiForPayedRacuni(int predmetId);
    Task<IEnumerable<RacunHolding>> TempList(string userId);
    Task<RacunHolding?> IncludeAll(int id);
}
}