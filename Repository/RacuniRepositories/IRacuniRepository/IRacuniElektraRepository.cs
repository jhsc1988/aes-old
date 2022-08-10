using aes.Models;
using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories.IRacuniRepository
{
    public interface IRacuniElektraRepository : IElektraRepository<RacunElektra>
    {
        Task<IEnumerable<RacunElektra>> TempList(string userId);
        Task<IEnumerable<RacunElektra>> GetRacuniElektraWithDopisiAndPredmeti();
        Task<IEnumerable<Predmet>> GetPredmetiForCreate();
        Task<IEnumerable<Dopis>> GetDopisiForPayedRacuniElektra(int predmetId);
        Task<RacunElektra> IncludeAll(int id);
        Task<IEnumerable<RacunElektra>> GetRacuniForCustomer(int kupacId);
    }
}
