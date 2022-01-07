using aes.Models;
using aes.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository
{
    public interface IObracunPotrosnjeRepository : IRepository<ObracunPotrosnje>
    {
        Task<ObracunPotrosnje> GetLastForRacunId(int billId);
        Task<IEnumerable<ObracunPotrosnje>> GetObracunForUgovorniRacun(long ugovorniRacun);
        Task<IEnumerable<ObracunPotrosnje>> GetObracunPotrosnjeForBill(int billId);
    }
}