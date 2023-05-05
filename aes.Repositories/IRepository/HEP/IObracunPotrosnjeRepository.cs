using aes.Models.HEP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.IRepository.HEP
{
    public interface IObracunPotrosnjeRepository : IRepository<ObracunPotrosnje>
    {
        Task<ObracunPotrosnje> GetLastForRacunId(int RacunId);
        Task<IEnumerable<ObracunPotrosnje>> GetObracunForUgovorniRacun(long ugovorniRacun);
        Task<IEnumerable<ObracunPotrosnje>> GetObracunPotrosnjeForRacun(int RacunId);
    }
}