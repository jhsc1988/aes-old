using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository
{
    public interface IObracunPotrosnjeRepository : IRepository<ObracunPotrosnje>
    {
        Task<IEnumerable<ObracunPotrosnje>> GetObracunPotrosnjeForBill(int billId);
    }
}